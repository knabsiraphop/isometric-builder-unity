using System;

namespace IsoBuilder.Core
{
    public class Inventory
    {
        private SaveDataStore saveDataStore;

        public Inventory(SaveDataStore saveDataStore)
        {
            this.saveDataStore = saveDataStore;
        }

        public bool TryConsumeItems(int[] ids, int[] amounts)
        {
            if (ids.Length != amounts.Length)
                return false;

            InventoryItemInstanceData[] items = saveDataStore.SaveData.InventoryItemInstanceDatas;
            int[] indexs = new int[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                indexs[i] = Array.FindIndex(items, item => item.Id == ids[i]);
                if (indexs[i] == -1)
                {
                    return false;
                }
                if (items[indexs[i]].Amount < amounts[i])
                {
                    return false;
                }
            }

            for (int i = 0; i < ids.Length; i++)
            {
                items[indexs[i]].Amount -= amounts[i];
            }
            saveDataStore.SaveData.InventoryItemInstanceDatas = items;
            saveDataStore.Save();
            return true;
        }

        public void AddItem(int id, int amount)
        {
            InventoryItemInstanceData[] items = saveDataStore.SaveData.InventoryItemInstanceDatas;

            int index = Array.FindIndex(items, item => item.Id == id);
            if (index != -1)
            {
                items[index].Amount += amount;
            }
            else
            {
                Array.Resize(ref items, items.Length + 1);
                items[items.Length - 1] = new InventoryItemInstanceData()
                {
                    Id = id,
                    Amount = amount
                };
            }

            saveDataStore.SaveData.InventoryItemInstanceDatas = items;
            saveDataStore.Save();
        }
    }
}
