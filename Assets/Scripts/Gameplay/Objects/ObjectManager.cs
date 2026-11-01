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
        private LoadSavePlacement loadSavePlacement;

        private List<PlacedItem> placedItems;
        private List<int> stationPlacedItems;

        private int currentInstanceId;

        public void Initialize(MapController mapController, LoadSavePlacement loadSavePlacement)
        {
            occupancyGrid = new OccupancyGrid();
            placementRules = new PlacementRules(mapController, occupancyGrid);
            this.loadSavePlacement = loadSavePlacement;

            placedItems = new List<PlacedItem>();
            stationPlacedItems = new List<int>();

            currentInstanceId = 0;
        }

        public bool TryCreatePlacedItem(ItemData itemData, Sprite sprite, out PlacedItem placedItem, int saveInstanceId = -1)
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
            int instanceId = saveInstanceId == -1 ? GetNextInstanceId() : saveInstanceId;
            placedItem.Initialize(itemData, sprite, instanceId);

            return true;
        }

        public void OnConfirmPlacedItem(PlacedItem placedItem)
        {
            IReadOnlyList<Vector2Int> cells = placedItem.ItemData.GetCells(placedItem.CellOrigin, placedItem.Flipped);
            foreach (Vector2Int cell in cells)
                occupancyGrid.Occupy(cell, placedItem.InstanceId);
            placedItems.Add(placedItem);
            if (placedItem.ItemData.Type == ItemType.Station)
            {
                stationPlacedItems.Add(placedItem.ItemData.Id);
            }
            loadSavePlacement.SaveAddOrUpdateState(placedItem);
        }

        public void OnRemovePlacedItem(PlacedItem placedItem)
        {
            IReadOnlyList<Vector2Int> cells = placedItem.ItemData.GetCells(placedItem.CellOrigin, placedItem.Flipped);
            foreach (Vector2Int cell in cells)
                occupancyGrid.Release(cell, placedItem.InstanceId);
            placedItems.Remove(placedItem);
            if (placedItem.ItemData.Type == ItemType.Station)
            {
                stationPlacedItems.Remove(placedItem.ItemData.Id);
            }
            loadSavePlacement.SaveRemoveState(placedItem.InstanceId);
        }

        public void OnConfirmMovePlacedItem(PlacedItem placedItem, Vector2Int previousCellOrigin, bool previousFlipped)
        {
            IReadOnlyList<Vector2Int> cells = placedItem.ItemData.GetCells(previousCellOrigin, previousFlipped);
            foreach (Vector2Int cell in cells)
                occupancyGrid.Release(cell, placedItem.InstanceId);
            cells = placedItem.ItemData.GetCells(placedItem.CellOrigin, placedItem.Flipped);
            foreach (Vector2Int cell in cells)
                occupancyGrid.Occupy(cell, placedItem.InstanceId);
            loadSavePlacement.SaveAddOrUpdateState(placedItem);
        }

        public bool IsItemConfirm(PlacedItem placedItem) => placedItems.Contains(placedItem);

        public bool CanPlaceItem(PlacedItem placedItem, Vector2Int origin, bool flipped)
        {
            return placementRules.CanPlaceItem(origin, placedItem.ItemData, flipped, placedItem.InstanceId);
        }

        public bool CanPlaceItem(ItemData itemData, Vector2Int origin, bool flipped, int instanceId)
        {
            return placementRules.CanPlaceItem(origin, itemData, flipped, instanceId);
        }

        private int GetNextInstanceId()
        {
            currentInstanceId++;
            return currentInstanceId;
        }

        public void SetCurrentInstanceId(int instanceId)
        {
            currentInstanceId = instanceId;
        }
    }
}