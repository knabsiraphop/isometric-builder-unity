using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsoBuilder.Gameplay
{
    public class LoadSavePlacement
    {
        private readonly Dictionary<int, PlacedItemData> placedItemDatas;
        private readonly SaveDataStore saveDataStore;

        public LoadSavePlacement(SaveDataStore saveDataStore)
        {
            this.saveDataStore = saveDataStore;
            placedItemDatas = new Dictionary<int, PlacedItemData>();
        }

        public async UniTask ApplySave(Func<PlacedItemData, UniTask> placeItem)
        {
            IEnumerable<PlacedItemData> sortDatas = saveDataStore.SaveData.PlacedObjectDatas.OrderBy(data => data.InstanceId);
            foreach (PlacedItemData placedObjectData in sortDatas)
            {
                placedItemDatas[placedObjectData.InstanceId] = placedObjectData;
                await placeItem(placedObjectData);
            }
        }

        public void SaveRemoveState(int instanceId)
        {
            if (placedItemDatas.ContainsKey(instanceId))
            {
                placedItemDatas.Remove(instanceId);
            }

            Save();
        }

        public void SaveAddOrUpdateState(PlacedItem placedItem)
        {
            int instanceId = placedItem.InstanceId;
            if (placedItemDatas.TryGetValue(instanceId, out PlacedItemData placedItemData))
            {
                if (IsEqual(placedItemData, placedItem))
                {
                    return;
                }
                else
                {
                    placedItemData.ItemId = placedItem.ItemData.Id;
                    placedItemData.CellOrigin = placedItem.CellOrigin;
                    placedItemData.Flipped = placedItem.Flipped;
                }
            }
            else
            {
                placedItemDatas[instanceId] = new PlacedItemData
                {
                    InstanceId = instanceId,
                    ItemId = placedItem.ItemData.Id,
                    CellOrigin = placedItem.CellOrigin,
                    Flipped = placedItem.Flipped,
                };
            }

            Save();
        }

        private void Save()
        {
            List<PlacedItemData> placedObjectDatas = new List<PlacedItemData>(placedItemDatas.Values);
            saveDataStore.SaveData.PlacedObjectDatas = placedObjectDatas;
            saveDataStore.Save();
        }

        private bool IsEqual(PlacedItemData data, PlacedItem placedItem)
        {
            if (data.ItemId != placedItem.ItemData.Id)
            {
                return false;
            }
            if (data.CellOrigin != placedItem.CellOrigin)
            {
                return false;
            }
            if (data.Flipped != placedItem.Flipped)
            {
                return false;
            }
            return true;
        }
    }
}
