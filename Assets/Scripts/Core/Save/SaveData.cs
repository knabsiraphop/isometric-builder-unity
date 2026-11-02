using System;
using System.Collections.Generic;

namespace IsoBuilder.Core
{
    [Serializable]
    public class SaveData
    {
        public List<PlacedObjectData> PlacedObjectDatas;
        public int QuizProgress;
        public InventoryItemData[] InventoryItemDatas;

        public SaveData()
        {
            PlacedObjectDatas = new List<PlacedObjectData>();
            QuizProgress = 0;
            InventoryItemDatas = new InventoryItemData[0];
        }
    }
}
