using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IsoBuilder.Gameplay
{
    public class CellHighlighter : MonoBehaviour
    {
        [SerializeField] private Tilemap highlightTileMap;

        [SerializeField] private TileBase canPlaceTile;
        [SerializeField] private TileBase cannotPlaceTile;

        private List<Vector2Int> highlightedCells = new List<Vector2Int>();

        public void HighlightCells(IEnumerable<Vector2Int> cellPositions, bool canPlace)
        {
            foreach (Vector2Int cellPosition in cellPositions)
            {
                HighlightCell(cellPosition, canPlace);
            }
        }

        public void HighlightCell(Vector2Int cellPosition, bool canPlace)
        {
            TileBase tileToUse = canPlace ? canPlaceTile : cannotPlaceTile;
            highlightTileMap.SetTile((Vector3Int)cellPosition, tileToUse);
            if (!highlightedCells.Contains(cellPosition))
            {
                highlightedCells.Add(cellPosition);
            }
        }

        public void ClearHighlight(Vector2Int cellPosition)
        {
            if (!highlightedCells.Contains(cellPosition))
            {
                return;
            }
            highlightTileMap.SetTile((Vector3Int)cellPosition, null);
            highlightedCells.Remove(cellPosition);
        }

        public void ClearAllHighlights()
        {
            foreach (Vector2Int cellPosition in highlightedCells)
            {
                highlightTileMap.SetTile((Vector3Int)cellPosition, null);
            }
            highlightedCells.Clear();
        }
    }
}
