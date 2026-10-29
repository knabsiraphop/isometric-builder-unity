using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Core
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Scriptable Objects/ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField] private ItemData[] itemDatas;

        public IReadOnlyList<ItemData> ItemDatas => itemDatas;
    }
}
