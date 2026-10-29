using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IsoBuilder.Core
{
    [Serializable]
    public struct ZoneData
    {
        public ZoneType Type;
        public RectInt Rect;
        public int TileId;
    }
}
