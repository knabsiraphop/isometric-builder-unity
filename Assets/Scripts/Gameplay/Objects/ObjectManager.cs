using IsoBuilder.Core;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class ObjectManager : MonoBehaviour
    {
        [SerializeField] private PlacedObject placedObjectPrefab;
        [SerializeField] private Transform container;

        private OccupancyGrid occupancyGrid;
        private PlacementRules placementRules;
        private LoadSavePlacement loadSavePlacement;

        private List<PlacedObject> placedObjects;
        private List<int> stationPlacedObjects;

        private int currentInstanceId;

        public void Initialize(MapController mapController, LoadSavePlacement loadSavePlacement)
        {
            occupancyGrid = new OccupancyGrid();
            placementRules = new PlacementRules(mapController, occupancyGrid);
            this.loadSavePlacement = loadSavePlacement;

            placedObjects = new List<PlacedObject>();
            stationPlacedObjects = new List<int>();

            currentInstanceId = 0;
        }

        public bool TryCreatePlacedObject(PlaceableData placeableData, Sprite sprite, out PlacedObject placedObject, int saveInstanceId = -1)
        {
            if (placeableData.Type == PlaceableType.Station)
            {
                if (stationPlacedObjects.Contains(placeableData.Id))
                {
                    placedObject = null;
                    Debug.LogWarning($"Station object with ID {placeableData.Id} has already been placed. Cannot place another one.");
                    return false;
                }
            }

            placedObject = Instantiate(placedObjectPrefab, container);
            int instanceId = saveInstanceId == -1 ? GetNextInstanceId() : saveInstanceId;
            placedObject.Initialize(placeableData, sprite, instanceId);

            return true;
        }

        public void OnConfirmPlacedObject(PlacedObject placedObject)
        {
            IReadOnlyList<Vector2Int> cells = placedObject.PlaceableData.GetCells(placedObject.CellOrigin, placedObject.Flipped);
            foreach (Vector2Int cell in cells)
                occupancyGrid.Occupy(cell, placedObject.InstanceId);
            placedObjects.Add(placedObject);
            if (placedObject.PlaceableData.Type == PlaceableType.Station)
            {
                stationPlacedObjects.Add(placedObject.PlaceableData.Id);
            }
            loadSavePlacement.SaveAddOrUpdateState(placedObject);
        }

        public void OnRemovePlacedObject(PlacedObject placedObject)
        {
            IReadOnlyList<Vector2Int> cells = placedObject.PlaceableData.GetCells(placedObject.CellOrigin, placedObject.Flipped);
            foreach (Vector2Int cell in cells)
                occupancyGrid.Release(cell, placedObject.InstanceId);
            placedObjects.Remove(placedObject);
            if (placedObject.PlaceableData.Type == PlaceableType.Station)
            {
                stationPlacedObjects.Remove(placedObject.PlaceableData.Id);
            }
            loadSavePlacement.SaveRemoveState(placedObject.InstanceId);
        }

        public void OnConfirmMovePlacedObject(PlacedObject placedObject, Vector2Int previousCellOrigin, bool previousFlipped)
        {
            IReadOnlyList<Vector2Int> cells = placedObject.PlaceableData.GetCells(previousCellOrigin, previousFlipped);
            foreach (Vector2Int cell in cells)
                occupancyGrid.Release(cell, placedObject.InstanceId);
            cells = placedObject.PlaceableData.GetCells(placedObject.CellOrigin, placedObject.Flipped);
            foreach (Vector2Int cell in cells)
                occupancyGrid.Occupy(cell, placedObject.InstanceId);
            loadSavePlacement.SaveAddOrUpdateState(placedObject);
        }

        public bool IsPlaceableObjectConfirm(PlacedObject placedObject) => placedObjects.Contains(placedObject);

        public bool CanPlaceObject(PlacedObject placedObject, Vector2Int origin, bool flipped)
        {
            return placementRules.CanPlaceObject(origin, placedObject.PlaceableData, flipped, placedObject.InstanceId);
        }

        public bool CanPlaceObject(PlaceableData PlaceableData, Vector2Int origin, bool flipped, int instanceId)
        {
            return placementRules.CanPlaceObject(origin, PlaceableData, flipped, instanceId);
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