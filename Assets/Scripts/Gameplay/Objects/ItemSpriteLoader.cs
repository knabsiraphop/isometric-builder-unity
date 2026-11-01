using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class ItemSpriteLoader : IItemSpriteLoader
    {
        private readonly Dictionary<int, Sprite> spriteCaches;

        public ItemSpriteLoader()
        {
            spriteCaches = new Dictionary<int, Sprite>();
        }

        public async UniTask<Sprite> GetSprite(ItemData itemData) 
        {
            if (TryGetCacheSprite(itemData, out Sprite cachedSprite))
            {
                return cachedSprite;
            }

            string resourcePath = $"Item/{itemData.Type}/{itemData.Id}";

            Sprite sprite = await ResourceLoader.LoadResourceAsync<Sprite>(resourcePath);
            return sprite;
        }

        public bool TryGetCacheSprite(ItemData itemData, out Sprite sprite)
        {
            if (spriteCaches.TryGetValue(itemData.Id, out Sprite cachedSprite))
            {
                sprite = cachedSprite;
                return true;
            }

            sprite = null;
            return false;
        }
    }
}
