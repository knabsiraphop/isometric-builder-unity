using IsoBuilder.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IsoBuilder.Gameplay
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private GridSystemConfig config;
        [SerializeField] private Tilemap zoneTilemap;
        [SerializeField] private Grid zoneGrid;
        [SerializeField] private TilemapRenderer zoneGridRenderer;

        public void Initialize()
        {
            if (config == null)
            {
                return;
            }

            zoneGrid.cellSize = new Vector3(config.CellSize.x, config.CellSize.y, 1);
            for (int i = 0; i < config.Zones.Length; i++)
            {
                ZoneData zone = config.Zones[i];
                for (int x = 0; x < zone.size.x; x++)
                {
                    for (int y = 0; y < zone.size.y; y++)
                    {
                        Vector3Int tilePosition = new Vector3Int(zone.position.x + x, zone.position.y + y, 0);
                        zoneTilemap.SetTile(tilePosition, zone.tile);
                    }
                }
            }
        }

        public Bounds GetMapBounds()
        {
            return zoneGridRenderer.bounds;
        }
    }
}
