using System;
using UnityEngine;

namespace IsoBuilder.Core
{
    [Serializable]
    public struct ItemData
    {
        public int Id;
        public Vector2Int FootPrint;
        public ItemType Type;
    }
}
