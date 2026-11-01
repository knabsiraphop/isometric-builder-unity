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

        public bool CanPlaceItem(Vector2Int originCell, ItemData itemData, bool flipped, int instanceId)
        {
            if (itemData.Type == ItemType.None)
            {
                return false;
            }

            IReadOnlyList<Vector2Int> cellsToCheck = itemData.GetCells(originCell, flipped);
            foreach (Vector2Int cell in cellsToCheck)
            {
                if (!mapController.TryGetZoneType(cell, out ZoneType zoneType) || zoneType == ZoneType.None)
                {
                    return false;
                }

                if (!CanFitInZone(itemData.Type, zoneType) || !occupancyGrid.IsFree(cell, instanceId))
                {
                    return false;
                }
            }
            return true;
        }

        private bool CanFitInZone(ItemType itemType, ZoneType zoneType)
        {
            if (zoneType == ZoneType.Mixed)
            {
                return true;
            }

            if (itemType == ItemType.Furniture && zoneType == ZoneType.Furniture)
            {
                return true;
            }

            return false;
        }
    }
}
