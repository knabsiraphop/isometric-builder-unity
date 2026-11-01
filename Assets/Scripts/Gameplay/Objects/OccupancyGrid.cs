using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class OccupancyGrid
    {
        private readonly Dictionary<Vector2Int, int> occupiedCells;

        public OccupancyGrid()
        {
            occupiedCells = new Dictionary<Vector2Int, int>();
        }

        public bool IsFree(Vector2Int cell, int instanceId)
        {
            if (!occupiedCells.TryGetValue(cell, out int storedValue))
            {
                return true;
            }
            return storedValue == instanceId;
        }

        public void Occupy(Vector2Int cell, int instanceId)
        {
            if (!IsFree(cell, instanceId))
            {
                Debug.LogError($"Attempted to occupy cell {cell} with instanceId {instanceId}, but it is already occupied by {occupiedCells[cell]}.");
                return;
            }
            occupiedCells[cell] = instanceId;
        }

        public void Release(Vector2Int cell, int instanceId)
        {
            if (!occupiedCells.TryGetValue(cell, out int storedValue) || storedValue != instanceId)
            {
                Debug.LogError($"Attempted to release cell {cell} with instanceId {instanceId},but it is free or occupied by {storedValue}.");
                return;
            }
            occupiedCells.Remove(cell);
        }
    }
}
