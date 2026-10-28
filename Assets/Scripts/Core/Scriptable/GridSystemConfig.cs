using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "GridSystemConfig", menuName = "Scriptable Objects/GridSystemConfig")]
    public class GridSystemConfig : ScriptableObject
    {
        public Vector2 GridSize;
        public Vector2 CellSize;
        public ZoneData[] Zones;
    }
}
