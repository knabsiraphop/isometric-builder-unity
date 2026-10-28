using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "GridSystemConfig", menuName = "Scriptable Objects/GridSystemConfig")]
    public class GridSystemConfig : ScriptableObject
    {
        public Vector2Int GridSize;
        public Vector2 CellSize;
        public ZoneData[] Zones;
        public ZoneTile[] ZoneTiles;
        public int DefaultTileId;
    }
}
