using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "GridSystemConfig", menuName = "Scriptable Objects/GridSystemConfig")]
    public class GridSystemConfig : ScriptableObject
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2 cellSize;
        [SerializeField] private ZoneData[] zones;
        [SerializeField] private int defaultTileId;

        public Vector2Int GridSize => gridSize;
        public Vector2 CellSize => cellSize;
        public IReadOnlyList<ZoneData> Zones => zones;
        public int DefaultTileId => defaultTileId;
    }
}
