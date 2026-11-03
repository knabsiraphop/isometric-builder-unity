using System;

namespace IsoBuilder.Core
{
    [Serializable]
    public class SaveData
    {
        public PlacedObjectData[] PlacedObjectDatas;
        public int QuizProgress;
        public InventoryItemInstanceData[] InventoryItemInstanceDatas;
        public MixerStateData MixerStateData;

        public void ApplySaveData(GameConfig gameConfig)
        {
            PlacedObjectDatas = null;
            QuizProgress = 0;

            InventoryItemInstanceData[] inventoryItemInstanceDatas = gameConfig.InventoryStartData.InventoryItemInstanceDatas;
            InventoryItemInstanceDatas = new InventoryItemInstanceData[inventoryItemInstanceDatas.Length];
            Array.Copy(inventoryItemInstanceDatas, InventoryItemInstanceDatas, inventoryItemInstanceDatas.Length);

            MixerStateData = null;
        }
    }
}
