using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "StartingData", menuName = "Scriptable Objects/StartingData")]
    public class StartingData : ScriptableObject
    {
        public InventoryItemInstanceData[] InventoryItemInstanceDatas;
        public PlacedObjectData[] PlacedObjectDatas;
    }
}
