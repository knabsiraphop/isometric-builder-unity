using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class ItemSpriteLoader
    {
        private Dictionary<int, Sprite> spriteCaches;

        public ItemSpriteLoader()
        {
            spriteCaches = new Dictionary<int, Sprite>();
        }

        public async UniTask<Sprite> GetSprite(ItemData itemData) 
        {
            if (spriteCaches.TryGetValue(itemData.Id, out Sprite cachedSprite))
            {
                return cachedSprite;
            }

            string resourcePath = $"Item/{itemData.Type}/{itemData.Id}";
            ResourceRequest resourceRequest = Resources.LoadAsync<Sprite>(resourcePath);
            await resourceRequest.ToUniTask();
            if(resourceRequest.asset == null)
            {
                Debug.LogError($"Sprite not found at path: {resourcePath}");
                return null;
            }
            Sprite sprite = (Sprite)resourceRequest.asset;
            spriteCaches[itemData.Id] = sprite;
            return sprite;
        }
    }
}
