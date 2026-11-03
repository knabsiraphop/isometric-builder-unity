using System;

namespace IsoBuilder.Core
{
    [Serializable]
    public struct InventoryItemData
    {
        public int Id;

        public string GetResourceSpritePath()
        {
            return $"InventoryItems/InventoryItem_{Id}";
        }

        public string GetSpriteKey()
        {
            return $"InventoryItem_{Id}";
        }
    }
}
