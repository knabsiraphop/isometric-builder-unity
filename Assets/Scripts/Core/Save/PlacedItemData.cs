using System;
using UnityEngine;

namespace IsoBuilder.Core
{
    [Serializable]
    public class PlacedItemData
    {
        public int InstanceId;
        public int ItemId;
        public Vector2Int CellOrigin;
        public bool Flipped;
    }
}
