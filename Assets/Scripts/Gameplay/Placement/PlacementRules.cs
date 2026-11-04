using IsoBuilder.Core;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacementRules
    {
        private readonly MapController mapController;
        private readonly OccupancyGrid occupancyGrid;

        public PlacementRules(MapController mapController, OccupancyGrid occupancyGrid)
        {
            this.mapController = mapController;
            this.occupancyGrid = occupancyGrid;
        }

        public bool CanPlaceObject(Vector2Int originCell, PlaceableData placeableData, bool flipped, int instanceId)
        {
            if (placeableData.Type == PlaceableType.None)
            {
                return false;
            }

            IReadOnlyList<Vector2Int> cellsToCheck = placeableData.GetCells(originCell, flipped);
            foreach (Vector2Int cell in cellsToCheck)
            {
                if (!mapController.TryGetZoneType(cell, out ZoneType zoneType) || zoneType == ZoneType.None)
                {
                    return false;
                }

                if (!CanFitInZone(placeableData.Type, zoneType) || !occupancyGrid.IsFree(cell, instanceId))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanFitInZone(PlaceableType placeableType, ZoneType zoneType)
        {
            if (zoneType == ZoneType.Mixed)
            {
                return true;
            }

            if (placeableType == PlaceableType.Furniture && zoneType == ZoneType.Furniture)
            {
                return true;
            }

            return false;
        }
    }
}
