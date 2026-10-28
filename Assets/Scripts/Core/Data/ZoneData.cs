using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IsoBuilder.Core
{
    [Serializable]
    public struct ZoneData
    {
        public ZoneType type;
        public Vector2Int position;
        public Vector2Int size;
        public TileBase tile;
    }
}
