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

        public bool IsFree(Vector2Int cell, int ownerId)
        {
            if (!occupiedCells.TryGetValue(cell, out int storedValue))
            {
                return true;
            }
            return storedValue == ownerId;
        }

        public void Occupy(Vector2Int cell, int ownerId)
        {
            if (!IsFree(cell, ownerId))
            {
                Debug.LogError($"Attempted to occupy cell {cell} with ownerId {ownerId}, but it is already occupied by {occupiedCells[cell]}.");
                return;
            }
            occupiedCells[cell] = ownerId;
        }

        public void Release(Vector2Int cell, int ownerId)
        {
            if (!occupiedCells.TryGetValue(cell, out int storedValue) || storedValue != ownerId)
            {
                Debug.LogError($"Attempted to release cell {cell} with ownerId {ownerId},but it is free or occupied by {storedValue}.");
                return;
            }
            occupiedCells.Remove(cell);
        }
    }
}
