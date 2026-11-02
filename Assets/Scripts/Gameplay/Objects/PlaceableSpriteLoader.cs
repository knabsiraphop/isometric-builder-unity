using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlaceableSpriteLoader : IPlaceableSpriteLoader
    {
        private readonly Dictionary<int, Sprite> spriteCaches;

        public PlaceableSpriteLoader()
        {
            spriteCaches = new Dictionary<int, Sprite>();
        }

        public async UniTask<Sprite> GetSprite(PlaceableData placeableData) 
        {
            if (TryGetCacheSprite(placeableData, out Sprite cachedSprite))
            {
                return cachedSprite;
            }

            string resourcePath = $"Placeable/{placeableData.Type}/{placeableData.Id}";

            Sprite sprite = await ResourceLoader.LoadResourceAsync<Sprite>(resourcePath);
            return sprite;
        }

        public bool TryGetCacheSprite(PlaceableData placeableData, out Sprite sprite)
        {
            if (spriteCaches.TryGetValue(placeableData.Id, out Sprite cachedSprite))
            {
                sprite = cachedSprite;
                return true;
            }

            sprite = null;
            return false;
        }
    }
}
