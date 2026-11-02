using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IsoBuilder.Gameplay
{
    public class LoadSavePlacement
    {
        private readonly Dictionary<int, PlacedObjectData> placedObjectDatas;
        private readonly SaveDataStore saveDataStore;

        public LoadSavePlacement(SaveDataStore saveDataStore)
        {
            this.saveDataStore = saveDataStore;
            placedObjectDatas = new Dictionary<int, PlacedObjectData>();
        }

        public async UniTask ApplySave(Func<PlacedObjectData, UniTask> placeObject)
        {
            PlacedObjectData[] savePlacedObjectDatas = saveDataStore.SaveData.PlacedObjectDatas;
            if(savePlacedObjectDatas != null)
            {
                IEnumerable<PlacedObjectData> sortDatas = savePlacedObjectDatas.OrderBy(data => data.InstanceId);
                foreach (PlacedObjectData placedObjectData in sortDatas)
                {
                    placedObjectDatas[placedObjectData.InstanceId] = placedObjectData;
                    await placeObject(placedObjectData);
                }
            }
        }

        public void SaveRemoveState(int instanceId)
        {
            if (placedObjectDatas.ContainsKey(instanceId))
            {
                placedObjectDatas.Remove(instanceId);
            }

            Save();
        }

        public void SaveAddOrUpdateState(PlacedObject placedObject)
        {
            int instanceId = placedObject.InstanceId;
            if (placedObjectDatas.TryGetValue(instanceId, out PlacedObjectData placedObjectData))
            {
                if (IsEqual(placedObjectData, placedObject))
                {
                    return;
                }
                else
                {
                    placedObjectData.Id = placedObject.PlaceableData.Id;
                    placedObjectData.CellOrigin = placedObject.CellOrigin;
                    placedObjectData.Flipped = placedObject.Flipped;
                }
            }
            else
            {
                placedObjectDatas[instanceId] = new PlacedObjectData
                {
                    InstanceId = instanceId,
                    Id = placedObject.PlaceableData.Id,
                    CellOrigin = placedObject.CellOrigin,
                    Flipped = placedObject.Flipped,
                };
            }

            Save();
        }

        private void Save()
        {
            PlacedObjectData[] datas = placedObjectDatas.Values.ToArray();
            saveDataStore.SaveData.PlacedObjectDatas = datas;
            saveDataStore.Save();
        }

        private bool IsEqual(PlacedObjectData data, PlacedObject placedObject)
        {
            if (data.Id != placedObject.PlaceableData.Id)
            {
                return false;
            }
            if (data.CellOrigin != placedObject.CellOrigin)
            {
                return false;
            }
            if (data.Flipped != placedObject.Flipped)
            {
                return false;
            }
            return true;
        }
    }
}
