using System.Collections.Generic;

namespace IsoBuilder.Core
{
    public class ItemCatalog
    {
        private Dictionary<int, ItemData> itemDatas;

        public void Initialize(ItemConfig itemConfig)
        {
            itemDatas = new Dictionary<int, ItemData>();

            foreach (ItemData item in itemConfig.ItemDatas)
            {
                itemDatas[item.Id] = item;
            }
        }

        public bool TryGetItemData(int id, out ItemData itemData)
        {
            return itemDatas.TryGetValue(id, out itemData);
        }
    }
}
