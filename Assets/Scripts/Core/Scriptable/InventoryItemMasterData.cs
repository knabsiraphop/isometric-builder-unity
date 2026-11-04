using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "InventoryItemMasterData", menuName = "Scriptable Objects/InventoryItemMasterData")]
    public class InventoryItemMasterData : ScriptableObject
    {
        public InventoryItemData[] InventoryItemDatas;
    }
}
