using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using System.Collections.Generic;
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
            if (itemCatalog != null && uiManager != null)
            {
                foreach (ItemData item in itemCatalog.GetItems())
                {
                    Sprite itemSprite = await itemSpriteLoader.GetSprite(item);
                    uiManager.AddItemsToList(itemSprite, () => PlaceItem(item).Forget());
                }
            }
        }

        private async UniTaskVoid PlaceItem(ItemData item)
        {
            if (currentPlacementStatus != null)
            {
                ClearCurrentPlacement();
            }

            Sprite itemSprite = await itemSpriteLoader.GetSprite(item);
            if (objectManager.TryCreatePlacedItem(item, itemSprite, out PlacedItem placedItem))
            {
                Vector2 screenCenter = cameraController.GetScreenCenter();
                Vector2 worldPosition = cameraController.ScreenToWorldPosition(screenCenter);

                Vector2Int cellOrigin = mapController.WorldToCell(worldPosition);
                Vector3 worldCellPosition = mapController.CellToWorld(cellOrigin);

                placedItem.SetPlacement(cellOrigin, false, worldCellPosition);
                CreateNewPlacementStatus(item, placedItem, cellOrigin);
            }
        }

        private void ClearCurrentPlacement()
        {
            if (currentPlacementStatus == null)
                return;
            Destroy(currentPlacementStatus.placedItem.gameObject);
            currentPlacementStatus = null;
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
