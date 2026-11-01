using System;
using System.Collections.Generic;

namespace IsoBuilder.Core
{
    [Serializable]
    public class SaveData
    {
        public List<PlacedItemData> PlacedObjectDatas;

        public SaveData()
        {
            PlacedObjectDatas = new List<PlacedItemData>();
        }
    }
}
