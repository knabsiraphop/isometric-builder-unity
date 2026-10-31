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

        private PlacedItem currentPlacementItem;
        private CurrentSelectedItemData currentSelectedItemData;

        public void Initialize(IUIManager uiManager, ItemCatalog itemCatalog, ItemSpriteLoader itemSpriteLoader,
            MapController mapController, ObjectManager objectManager, CameraController cameraController)
        {
            this.uiManager = uiManager;
            this.itemCatalog = itemCatalog;
            this.itemSpriteLoader = itemSpriteLoader;
            this.mapController = mapController;
            this.objectManager = objectManager;
            this.cameraController = cameraController;

            pointerInput = new PointerInput();
            itemDragger = new(cameraController, pointerInput, mapController);
            itemPressDetector = new(pointerInput, SelectItem);
        }

        public async UniTask ShowItemCatalogs()
        {
            IEnumerable<ItemData> items = itemCatalog.GetItems();

            foreach (ItemData item in items)
            {
                Sprite itemSprite = await itemSpriteLoader.GetSprite(item);
                uiManager.AddItemsToList(itemSprite, () => PlaceItem(item));
            }
        }

        private void PlaceItem(ItemData item)
        {
            Sprite itemSprite = itemSpriteLoader.GetCacheSprite(item);
            if (itemSprite == null)
            {
                Debug.LogError($"Sprite not found for item: {item.Id}");
                return;
            }
            if (objectManager.TryCreatePlacedItem(item, itemSprite, out PlacedItem placedItem))
            {
                Vector2 screenCenter = cameraController.GetScreenCenter();
                Vector2 worldPosition = cameraController.ScreenToWorldPosition(screenCenter);
                Vector2Int cellOrigin = mapController.WorldToCell(worldPosition);
                SetItemPlacement(cellOrigin, placedItem);
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
                currentSelectedItemData.cellOrigin = placedItem.OriginCell;
                currentSelectedItemData.flipped = placedItem.Flipped;
                currentSelectedItemData.worldPosition = placedItem.WorldCellPosition;
            }
        }

        private void ConfirmPlacement()
        {
            if (!objectManager.CanPlaceItem(currentPlacementItem, currentPlacementItem.OriginCell, currentPlacementItem.Flipped))
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
            currentPlacementItem.SetPlacement(currentPlacementItem.OriginCell, !currentPlacementItem.Flipped, currentPlacementItem.WorldCellPosition);
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
                    SetItemPlacement(newCellOrigin, currentPlacementItem);
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

        private void SetItemPlacement(Vector2Int cellOrigin, PlacedItem placedItem)
        {
            Vector3 worldCellPosition = mapController.CellToWorld(cellOrigin);
            placedItem.SetPlacement(cellOrigin, placedItem.Flipped, worldCellPosition);
        }

        private void SetHighlight(PlacedItem placedItem)
        {
            cellHighlighter.ClearAllHighlights();
            bool canPlace = objectManager.CanPlaceItem(placedItem, placedItem.OriginCell, placedItem.Flipped);
            cellHighlighter.HighlightCells(placedItem.ItemData.GetCells(placedItem.OriginCell, placedItem.Flipped), canPlace);
        }
    }
}
