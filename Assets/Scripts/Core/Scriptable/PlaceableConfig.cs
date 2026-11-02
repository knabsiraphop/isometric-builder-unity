using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "PlaceableConfig", menuName = "Scriptable Objects/PlaceableConfig")]
    public class PlaceableConfig : ScriptableObject
    {
        [SerializeField] private PlaceableData[] placeableDatas;

        public IReadOnlyList<PlaceableData> PlaceableDatas => placeableDatas;
    }
}
