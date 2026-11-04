using System;
using UnityEngine;

namespace IsoBuilder.Core
{
    [Serializable]
    public class PlacedObjectData
    {
        public int InstanceId;
        public int Id;
        public Vector2Int CellOrigin;
        public bool Flipped;
    }
}
