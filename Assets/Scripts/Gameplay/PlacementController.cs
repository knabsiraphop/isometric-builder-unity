using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacementController : MonoBehaviour
    {
        private class PlacementStatus
        {
            public int itemId;
            public PlacedItem placedItem;
            public Vector2Int origin;
            public bool flipped = false;
        }

        [SerializeField] private CellHighlighter cellHighlighter;

        private IUIManager uiManager;
        private ItemCatalog itemCatalog;
        private ItemSpriteLoader itemSpriteLoader;
        private MapController mapController;
        private ObjectManager objectManager;
        private CameraController cameraController;

        private PlacementStatus currentPlacementStatus;

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
            if (currentPlacementStatus != null)
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
                CreateNewPlacementStatus(item, placedItem, cellOrigin);
            }
        }

        private void ClearCurrentPlacement()
        {
            if (currentPlacementStatus == null)
                return;
            Destroy(currentPlacementStatus.placedItem.gameObject);
            currentPlacementStatus = null;
            cellHighlighter.ClearAllHighlights();
        }

        private void CreateNewPlacementStatus(ItemData item, PlacedItem placedItem, Vector2Int cellOrigin)
        {
            currentPlacementStatus = new PlacementStatus
            {
                itemId = item.Id,
                placedItem = placedItem,
                origin = cellOrigin,
                flipped = false
            };
        }
    }
}
