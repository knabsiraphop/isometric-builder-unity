using System;

namespace IsoBuilder.Core
{
    [Serializable]
    public struct InventoryItemInstanceData
    {
        public int Id;
        public int Amount;

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
