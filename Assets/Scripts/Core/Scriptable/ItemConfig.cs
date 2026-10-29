using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Scriptable Objects/ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] private ItemData[] itemDatas;
        [SerializeField] private ItemSprite[] itemSprites;

        public IReadOnlyList<ItemData> ItemDatas => itemDatas;
        public IReadOnlyList<ItemSprite> ItemSprites => itemSprites;
    }
}
