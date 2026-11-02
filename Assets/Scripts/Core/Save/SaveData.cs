using System;

namespace IsoBuilder.Core
{
    [Serializable]
    public class SaveData
    {
        public PlacedObjectData[] PlacedObjectDatas;
        public int QuizProgress;
        public InventoryItemInstanceData[] InventoryItemInstanceDatas;
        public MixerStateData[] MixerStateDatas;

        public void ApplySaveData(GameConfig gameConfig)
        {
            PlacedObjectDatas = null;
            QuizProgress = 0;
            InventoryItemInstanceDatas = gameConfig.InventoryStartData.InventoryItemInstanceDatas;

            MixerStateDatas = null;
        }
    }
}
