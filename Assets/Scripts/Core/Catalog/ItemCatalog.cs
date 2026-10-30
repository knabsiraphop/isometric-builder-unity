using System.Collections.Generic;
using System.Linq;

namespace IsoBuilder.Core
{
    public class ItemCatalog
    {
        private Dictionary<int, ItemData> itemDatas;

        public void Initialize(ItemConfig itemConfig)
        {
            itemDatas = new Dictionary<int, ItemData>();

            IOrderedEnumerable<ItemData> sortItems = itemConfig.ItemDatas.OrderBy(item => item.Id);

            foreach (ItemData item in sortItems)
            {
                itemDatas[item.Id] = item;
            }
        }

        public bool TryGetItemData(int id, out ItemData itemData)
        {
            return itemDatas.TryGetValue(id, out itemData);
        }

        public IEnumerable<ItemData> GetItems()
        {
            return itemDatas.Values;
        }
    }
}
