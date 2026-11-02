using System;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Core
{
    [Serializable]
    public struct PlaceableData
    {
        public int Id;
        public Vector2Int FootPrint;
        public PlaceableType Type;
        public Vector2 SpriteOffset;

        public IReadOnlyList<Vector2Int> GetCells(Vector2Int originCell, bool flipped)
        {
            int width = flipped ? FootPrint.y : FootPrint.x;
            int height = flipped ? FootPrint.x : FootPrint.y;

            Vector2Int[] result = new Vector2Int[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result[x * height + y] = originCell + new Vector2Int(x, y);
                }
            }
            return result;
        }

        public string GetResourcePath()
        {
            return $"Placeable/{Type}/{Id}";
        }

        public string GetSpriteKey() 
        {
            return $"{Type}_{Id}";
        }
    }
}
