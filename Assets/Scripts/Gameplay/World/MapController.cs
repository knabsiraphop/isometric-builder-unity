using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IsoBuilder.Gameplay
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private Tilemap zoneTilemap;
        [SerializeField] private Grid zoneGrid;
        [SerializeField] private TilemapRenderer zoneGridRenderer;

        private GridSystemConfig config;
        private Dictionary<Vector2Int, ZoneType> gridDatas;
        private Dictionary<int, TileBase> tileBases;

        public async UniTask Initialize(GridSystemConfig config)
        {
            if (config == null)
            {
                return;
            }

            this.config = config;

            zoneGrid.cellSize = new Vector3(config.CellSize.x, config.CellSize.y, 1);
            gridDatas = new Dictionary<Vector2Int, ZoneType>();
            gridDatas.EnsureCapacity(config.GridSize.x * config.GridSize.y);

            await PreLoadTiles();
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
                        TileId = config.DefaultTileId,
                        Type = ZoneType.None,
                    };

                    for (int i = 0; i < config.Zones.Count; i++)
                    {
                        if (config.Zones[i].Rect.Contains(new Vector2Int(x, y)))
                        {
                            zone = config.Zones[i];
                            break;
                        }
                    }

                    gridDatas[new Vector2Int(x, y)] = zone.Type;

                    if (TryGetTileBase(zone.TileId, out TileBase tile))
                    {
                        zoneTilemap.SetTile(tilePosition, tile);
                    }
                }
            }
        }

        private async UniTask PreLoadTiles()
        {
            tileBases = new Dictionary<int, TileBase>();

            HashSet<int> ids = new HashSet<int>() { config.DefaultTileId };
            for (int i = 0; i < config.Zones.Count; i++)
            {
                ids.Add(config.Zones[i].TileId);
            }

            foreach (int id in ids)
            {
                TileBase result = await ResourceLoader.LoadResourceAsync<TileBase>($"Tile/Tile_{id}");

                if (result == null)
                {
                    return;
                }
                tileBases[id] = result;
            }
        }

        private bool TryGetTileBase(int tileId, out TileBase tileBase)
        {
            if (tileBases.TryGetValue(tileId, out TileBase cachedTile))
            {
                tileBase = cachedTile;
                return true;
            }
            tileBase = null;
            return false;
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

        public Vector3 CellToWorld(Vector2Int cellPosition)
        {
            Vector3 worldPosition = zoneGrid.GetCellCenterWorld(new Vector3Int(cellPosition.x, cellPosition.y, 0));
            return new Vector3(worldPosition.x, worldPosition.y, 0);
        }

        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            Vector3Int cellPosition = zoneGrid.WorldToCell(new Vector3(worldPosition.x, worldPosition.y, 0));
            return new Vector2Int(cellPosition.x, cellPosition.y);
        }
    }
}
