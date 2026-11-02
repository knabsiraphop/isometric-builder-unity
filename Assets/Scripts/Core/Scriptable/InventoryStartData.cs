using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "InventoryStartData", menuName = "Scriptable Objects/InventoryStartData")]
    public class InventoryStartData : ScriptableObject
    {
        public InventoryItemInstanceData[] InventoryItemInstanceDatas;
    }
}
