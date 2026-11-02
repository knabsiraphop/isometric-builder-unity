using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class SpriteLoader : ISpriteLoader
    {
        private readonly Dictionary<string, Sprite> spriteCaches;

        public SpriteLoader()
        {
            spriteCaches = new Dictionary<string, Sprite>();
        }

        public async UniTask<Sprite> GetSprite(string path, string spriteKey)
        {
            if (TryGetCacheSprite(spriteKey, out Sprite cachedSprite))
            {
                return cachedSprite;
            }
            Sprite sprite = await ResourceLoader.LoadResourceAsync<Sprite>(path);
            return sprite;
        }

        private bool TryGetCacheSprite(string spriteKey, out Sprite sprite)
        {
            if (spriteCaches.TryGetValue(spriteKey, out Sprite cachedSprite))
            {
                sprite = cachedSprite;
                return true;
            }

            sprite = null;
            return false;
        }

        public void ClearAllCache()
        {
            throw new System.NotImplementedException();
        }
    }
}
