using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IsoBuilder.Core
{
    public interface IItemSpriteLoader
    {
        public UniTask<Sprite> GetSprite(ItemData itemData);
    }
}
