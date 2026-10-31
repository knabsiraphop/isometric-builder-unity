using IsoBuilder.Core;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class ObjectManager : MonoBehaviour
    {
        [SerializeField] private PlacedItem placedItemPrefab;

        private OccupancyGrid occupancyGrid;
        private PlacementRules placementRules;
        private MapController mapController;

        private List<PlacedItem> placedItems;
        private List<int> stationPlacedItems;

        private int currentOwnerId = 0;

        public void Initialize(MapController mapController)
        {
            this.mapController = mapController;
            occupancyGrid = new OccupancyGrid();
            placementRules = new PlacementRules(mapController, occupancyGrid);

            placedItems = new List<PlacedItem>();
            stationPlacedItems = new List<int>();
        }

        public bool TryCreatePlacedItem(ItemData itemData, Sprite sprite, out PlacedItem placedItem)
        {
            if (itemData.Type == ItemType.Station)
            {
                if (stationPlacedItems.Contains(itemData.Id))
                {
                    placedItem = null;
                    Debug.LogWarning($"Station item with ID {itemData.Id} has already been placed. Cannot place another one.");
                    return false;
                }
            }

            placedItem = Instantiate(placedItemPrefab, transform);
            placedItem.Initialize(itemData, sprite, GetNextOwnerId());

            return true;
        }

        public void OnConfirmPlacedItem(PlacedItem placedItem)
        {
            IReadOnlyList<Vector2Int> cells = placedItem.ItemData.GetCells(placedItem.OriginCell, placedItem.Flipped);
            foreach (Vector2Int cell in cells)
                occupancyGrid.Occupy(cell, placedItem.OwnerId);
            placedItems.Add(placedItem);
            if (placedItem.ItemData.Type == ItemType.Station)
            {
                stationPlacedItems.Add(placedItem.ItemData.Id);
            }
        }

        public void OnRemovePlacedItem(PlacedItem placedItem)
        {
            IReadOnlyList<Vector2Int> cells = placedItem.ItemData.GetCells(placedItem.OriginCell, placedItem.Flipped);
            foreach (Vector2Int cell in cells)
                occupancyGrid.Release(cell, placedItem.OwnerId);
            placedItems.Remove(placedItem);
            if (placedItem.ItemData.Type == ItemType.Station)
            {
                stationPlacedItems.Remove(placedItem.ItemData.Id);
            }
        }

        public bool IsItemConfirm(PlacedItem placedItem) => placedItems.Contains(placedItem);

        public bool CanPlaceItem(PlacedItem placedItem, Vector2Int origin, bool flipped)
        {
            return placementRules.CanPlaceItem(origin, placedItem.ItemData, flipped, placedItem.OwnerId);
        }

        private int GetNextOwnerId()
        {
            currentOwnerId++;
            return currentOwnerId;
        }
    }
}
