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
            PlacedObjectData[] startingPlaceObjects = gameConfig.StartingData.PlacedObjectDatas;
            PlacedObjectDatas = new PlacedObjectData[startingPlaceObjects.Length];
            for (int i = 0; i < startingPlaceObjects.Length; i++) 
            {
                PlacedObjectDatas[i] = startingPlaceObjects[i].Clone();
            }

            QuizProgress = 0;

            InventoryItemInstanceData[] inventoryItemInstanceDatas = gameConfig.StartingData.InventoryItemInstanceDatas;
            InventoryItemInstanceDatas = new InventoryItemInstanceData[inventoryItemInstanceDatas.Length];
            Array.Copy(inventoryItemInstanceDatas, InventoryItemInstanceDatas, inventoryItemInstanceDatas.Length);

            MixerStateData = new();
        }
    }
}
