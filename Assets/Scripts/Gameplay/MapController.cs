using IsoBuilder.Core;
using System.Collections.Generic;
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

        private Dictionary<Vector2Int, ZoneType> gridDatas;
        private Dictionary<int, TileBase> tileBases;

        public void Initialize()
        {
            if (config == null)
            {
                return;
            }

            MapTileBases();
            zoneGrid.cellSize = new Vector3(config.CellSize.x, config.CellSize.y, 1);
            gridDatas = new Dictionary<Vector2Int, ZoneType>();
            gridDatas.EnsureCapacity(config.GridSize.x * config.GridSize.y);
            SetupMapTiles();
        }

        private void SetupMapTiles()
        {
            for (int x = 0; x < config.GridSize.x; x++)
            {
                for (int y = 0; y < config.GridSize.y; y++)
                {
                    Vector3Int tilePosition = new Vector3Int(x, y, 0);
                    ZoneData zone = new ZoneData()
                    {
                        tileId = config.DefaultTileId,
                        type = ZoneType.None,
                    };

                    for (int i = 0; i < config.Zones.Length; i++)
                    {
                        if (config.Zones[i].rect.Contains(new Vector2Int(x, y)))
                        {
                            zone = config.Zones[i];
                            break;
                        }
                    }

                    gridDatas[new Vector2Int(x, y)] = zone.type;
                    if (tileBases.TryGetValue(zone.tileId, out TileBase tile))
                    {
                        zoneTilemap.SetTile(tilePosition, tile);
                    }
                }
            }
        }

        private void MapTileBases()
        {
            tileBases = new Dictionary<int, TileBase>();
            for (int i = 0; i < config.ZoneTiles.Length; i++)
            {
                ZoneTile zoneTile = config.ZoneTiles[i];
                tileBases[zoneTile.tileId] = zoneTile.tile;
            }
        }

        public Bounds GetMapBounds()
        {
            return zoneGridRenderer.bounds;
        }

        public bool TryGetZoneType(Vector2Int gridPosition, out ZoneType zoneType)
        {
            if (gridDatas == null)
            {
                zoneType = ZoneType.None;
                return false;
            }
            return gridDatas.TryGetValue(gridPosition, out zoneType);
        }
    }
}
