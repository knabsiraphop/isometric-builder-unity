using System.Collections.Generic;
using System.Linq;

namespace IsoBuilder.Core
{
    public class InventoryItemCatalog
    {
        private Dictionary<int, InventoryItemData> inventoryItemDatas;

        public InventoryItemCatalog(InventoryItemMasterData inventoryItemMasterData)
        {
            inventoryItemDatas = new Dictionary<int, InventoryItemData>();

            IOrderedEnumerable<InventoryItemData> sortDatas = inventoryItemMasterData.InventoryItemDatas.OrderBy(data => data.Id);

            foreach (InventoryItemData data in sortDatas)
            {
                inventoryItemDatas[data.Id] = data;
            }
        }

        public bool TryGetInventoryItemData(int id, out InventoryItemData inventoryItemData)
        {
            return inventoryItemDatas.TryGetValue(id, out inventoryItemData);
        }

        public string GetResourcePath(int id)
        {
            if (inventoryItemDatas.TryGetValue(id, out InventoryItemData inventoryItemData))
            {
                return inventoryItemData.GetResourceSpritePath();
            }
            return string.Empty;
        }

        public string GetSpriteKey(int id)
        {
            if (inventoryItemDatas.TryGetValue(id, out InventoryItemData inventoryItemData))
            {
                return inventoryItemData.GetSpriteKey();
            }
            return string.Empty;
        }
    }
}
