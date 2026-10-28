using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IsoBuilder.Core
{
    [Serializable]
    public struct ZoneData
    {
        public ZoneType type;
        public RectInt rect;
        public int tileId;
    }
}
