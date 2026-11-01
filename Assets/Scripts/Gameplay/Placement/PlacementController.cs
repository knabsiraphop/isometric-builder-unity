using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacementController : MonoBehaviour
    {
        private struct CurrentSelectedItemData
        {
            public bool hasSnapShot;
            public Vector2Int cellOrigin;
            public bool flipped;
            public Vector3 worldPosition;
        }

        [SerializeField] private CellHighlighter cellHighlighter;

        private IUIManager uiManager;
        private ItemCatalog itemCatalog;
        private ItemSpriteLoader itemSpriteLoader;
        private MapController mapController;
        private ObjectManager objectManager;
        private CameraController cameraController;
        private PointerInput pointerInput;
        private ItemDragger itemDragger;
        private ItemPressDetector itemPressDetector;
        private LoadSavePlacement loadSavePlacement;

        private PlacedItem currentPlacementItem;
        private CurrentSelectedItemData currentSelectedItemData;

        public void Initialize(IUIManager uiManager, ItemCatalog itemCatalog, ItemSpriteLoader itemSpriteLoader,
            MapController mapController, ObjectManager objectManager, CameraController cameraController, LoadSavePlacement loadSavePlacement)
        {
            this.uiManager = uiManager;
            this.itemCatalog = itemCatalog;
            this.itemSpriteLoader = itemSpriteLoader;
            this.mapController = mapController;
            this.objectManager = objectManager;
            this.cameraController = cameraController;
            this.loadSavePlacement = loadSavePlacement;

            pointerInput = new PointerInput();
            itemDragger = new(cameraController, pointerInput, mapController);
            itemPressDetector = new(pointerInput, SelectItem, TapItem);
        }

        public async UniTask ApplySave()
        {
            await loadSavePlacement.ApplySave(PlaceItemFromData);
        }

        public async UniTask PlaceItemFromData(PlacedItemData placedObjectData)
        {
            objectManager.SetCurrentInstanceId(placedObjectData.InstanceId);
            if (!itemCatalog.TryGetItemData(placedObjectData.ItemId, out ItemData itemData))
            {
                Debug.LogError($"Item data not found : {placedObjectData.ItemId}");
                return;
            }
            Sprite itemSprite = await itemSpriteLoader.GetSprite(itemData);
            if (!itemSprite)
            {
                return;
            }
            if (!objectManager.CanPlaceItem(itemData, placedObjectData.CellOrigin, placedObjectData.Flipped, placedObjectData.InstanceId))
            {
                Debug.LogError("Can't Place Saved Item");
                return;
            }
            if (!objectManager.TryCreatePlacedItem(itemData, itemSprite, out PlacedItem placedItem, placedObjectData.InstanceId))
            {
                return;
            }
            SetItemPlacement(placedObjectData.CellOrigin, placedItem, placedObjectData.Flipped);

            objectManager.OnConfirmPlacedItem(placedItem);
            placedItem.InitializePointer(itemPressDetector.OnItemPressed);
            return;
        }

        public async UniTask PlaceItem(ItemData item)
        {
            Sprite itemSprite = await itemSpriteLoader.GetSprite(item);
            if (!itemSprite)
            {
                return;
            }
            if (objectManager.TryCreatePlacedItem(item, itemSprite, out PlacedItem placedItem))
            {
                Vector2 screenCenter = cameraController.GetScreenCenter();
                Vector2 worldPosition = cameraController.ScreenToWorldPosition(screenCenter);
                Vector2Int cellOrigin = mapController.WorldToCell(worldPosition);
                SetItemPlacement(cellOrigin, placedItem, false);
                SelectItem(placedItem);
            }
        }

        private void SelectItem(PlacedItem placedItem)
        {
            if (placedItem == currentPlacementItem)
                return;

            if (currentPlacementItem != null)
            {
                ClearCurrentPlacement();
            }
            SetHighlight(placedItem);
            currentPlacementItem = placedItem;
            uiManager.ShowPlacementPanel(ConfirmPlacement, CancelPlacement, FlipPlacement);
            if (objectManager.IsItemConfirm(placedItem))
            {
                currentSelectedItemData.hasSnapShot = true;
                currentSelectedItemData.cellOrigin = placedItem.CellOrigin;
                currentSelectedItemData.flipped = placedItem.Flipped;
                currentSelectedItemData.worldPosition = placedItem.WorldCellPosition;
            }
        }

        private void TapItem(PlacedItem placedItem) 
        {
            if (currentPlacementItem != null)
                return;
            if (placedItem.ItemData.Type != ItemType.Station)
                return;
        }

        private void ConfirmPlacement()
        {
            if (!objectManager.CanPlaceItem(currentPlacementItem, currentPlacementItem.CellOrigin, currentPlacementItem.Flipped))
            {
                Debug.LogError("Can't Place Item");
                return;
            }
            if (objectManager.IsItemConfirm(currentPlacementItem))
                objectManager.OnConfirmMovePlacedItem(currentPlacementItem, currentSelectedItemData.cellOrigin, currentSelectedItemData.flipped);
            else
                objectManager.OnConfirmPlacedItem(currentPlacementItem);
            currentPlacementItem.InitializePointer(itemPressDetector.OnItemPressed);
            currentSelectedItemData.hasSnapShot = false;
            ClearCurrentPlacement();
        }

        private void CancelPlacement()
        {
            ClearCurrentPlacement();
        }

        private void FlipPlacement()
        {
            currentPlacementItem.SetPlacement(currentPlacementItem.CellOrigin, !currentPlacementItem.Flipped, currentPlacementItem.WorldCellPosition);
            SetHighlight(currentPlacementItem);
        }

        private void ClearCurrentPlacement()
        {
            if (currentPlacementItem == null)
                return;
            if (!objectManager.IsItemConfirm(currentPlacementItem))
            {
                Destroy(currentPlacementItem.gameObject);
            }
            else
            {
                if (currentSelectedItemData.hasSnapShot)
                {
                    currentPlacementItem.SetPlacement(currentSelectedItemData.cellOrigin, currentSelectedItemData.flipped,
                        currentSelectedItemData.worldPosition);
                    currentSelectedItemData.hasSnapShot = false;
                }
            }

            currentPlacementItem = null;
            cellHighlighter.ClearAllHighlights();
            uiManager.HidePlacementPanel();
            itemDragger.Reset();
        }

        private void Update()
        {
            if (pointerInput == null)
                return;
            if (pointerInput.WasPressedThisFrame())
            {
                itemDragger.OnPressedThisFrame(currentPlacementItem);
                itemPressDetector.OnPressedThisFrame();
            }

            if (pointerInput.IsPressed())
            {
                if (itemDragger.TryGetNewCellOrigin(currentPlacementItem, out Vector2Int newCellOrigin))
                {
                    SetItemPlacement(newCellOrigin, currentPlacementItem, currentPlacementItem.Flipped);
                    SetHighlight(currentPlacementItem);
                }
                itemPressDetector.OnPressed();
            }

            if (pointerInput.WasReleasedThisFrame())
            {
                itemDragger.OnReleasedThisFrame();
                itemPressDetector.OnReleasedThisFrame();
            }
        }

        private void SetItemPlacement(Vector2Int cellOrigin, PlacedItem placedItem, bool flipped)
        {
            Vector3 worldCellPosition = mapController.CellToWorld(cellOrigin);
            placedItem.SetPlacement(cellOrigin, flipped, worldCellPosition);
        }

        private void SetHighlight(PlacedItem placedItem)
        {
            cellHighlighter.ClearAllHighlights();
            bool canPlace = objectManager.CanPlaceItem(placedItem, placedItem.CellOrigin, placedItem.Flipped);
            cellHighlighter.HighlightCells(placedItem.ItemData.GetCells(placedItem.CellOrigin, placedItem.Flipped), canPlace);
        }
    }
}
