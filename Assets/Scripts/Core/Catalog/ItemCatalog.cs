using System.Collections.Generic;
using System.Linq;

namespace IsoBuilder.Core
{
    public class ItemCatalog
    {
        private Dictionary<int, ItemData> itemDatas;
        private Dictionary<int, StationType> stationTypes;

        public void Initialize(ItemConfig itemConfig, StationConfig stationConfig)
        {
            itemDatas = new Dictionary<int, ItemData>();
            stationTypes = new Dictionary<int, StationType>();

            IOrderedEnumerable<ItemData> sortItems = itemConfig.ItemDatas.OrderBy(item => item.Id);

            foreach (ItemData item in sortItems)
            {
                itemDatas[item.Id] = item;
            }

            foreach (StationData station in stationConfig.StationDatas)
            {
                stationTypes[station.ItemId] = station.Type;
            }
        }

        public bool TryGetItemData(int id, out ItemData itemData)
        {
            return itemDatas.TryGetValue(id, out itemData);
        }

        public bool TryGetStationType(int itemId, out StationType stationType)
        {
            return stationTypes.TryGetValue(itemId, out stationType);
        }

        public IEnumerable<ItemData> GetItems()
        {
            return itemDatas.Values;
        }
    }
}
