using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacementController : MonoBehaviour
    {
        [SerializeField] private CellHighlighter cellHighlighter;

        private IUIManager uiManager;
        private ItemCatalog itemCatalog;
        private ItemSpriteLoader itemSpriteLoader;
        private MapController mapController;
        private ObjectManager objectManager;
        private CameraController cameraController;

        private PlacedItem currentPlacementItem;

        public void Initialize(IUIManager uiManager, ItemCatalog itemCatalog, ItemSpriteLoader itemSpriteLoader,
            MapController mapController, ObjectManager objectManager, CameraController cameraController)
        {
            this.uiManager = uiManager;
            this.itemCatalog = itemCatalog;
            this.itemSpriteLoader = itemSpriteLoader;
            this.mapController = mapController;
            this.objectManager = objectManager;
            this.cameraController = cameraController;
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
            if (currentPlacementItem != null)
            {
                ClearCurrentPlacement();
            }

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
                Vector3 worldCellPosition = mapController.CellToWorld(cellOrigin);

                placedItem.SetPlacement(cellOrigin, false, worldCellPosition);

                bool canPlace = objectManager.CanPlaceItem(placedItem, cellOrigin, false);
                cellHighlighter.HighlightCells(placedItem.ItemData.GetCells(cellOrigin, false), canPlace);

                uiManager.ShowPlacementPanel(ConfirmPlacement, CancelPlacement, FlipPlacement);

                currentPlacementItem = placedItem;
            }
        }

        private void ConfirmPlacement()
        {
            if (!objectManager.CanPlaceItem(currentPlacementItem, currentPlacementItem.OriginCell, currentPlacementItem.Flipped))
            {
                Debug.LogError("Can't Place Item");
                return;
            }
            objectManager.OnConfirmPlacedItem(currentPlacementItem);
            ClearCurrentPlacement();
        }

        private void CancelPlacement()
        {
            ClearCurrentPlacement();
            uiManager.HidePlacementPanel();
            cellHighlighter.ClearAllHighlights();
        }

        private void FlipPlacement()
        {
            currentPlacementItem.SetPlacement(currentPlacementItem.OriginCell, !currentPlacementItem.Flipped, currentPlacementItem.WorldCellPosition);

            cellHighlighter.ClearAllHighlights();
            bool canPlace = objectManager.CanPlaceItem(currentPlacementItem, currentPlacementItem.OriginCell, currentPlacementItem.Flipped);
            cellHighlighter.HighlightCells(currentPlacementItem.ItemData.GetCells(currentPlacementItem.OriginCell, currentPlacementItem.Flipped), canPlace);
        }

        private void ClearCurrentPlacement()
        {
            if (currentPlacementItem == null)
                return;
            if (!objectManager.IsItemConfirm(currentPlacementItem))
                Destroy(currentPlacementItem.gameObject);
            currentPlacementItem = null;
            cellHighlighter.ClearAllHighlights();
            uiManager.HidePlacementPanel();
        }
    }
}
