using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IsoBuilder.Core
{
    public interface ISpriteLoader
    {
        public UniTask<Sprite> GetSprite(string path, string spriteKey);
        public void ClearAllCache();
    }

    public static class SpriteLoaderExtension
    {
        public static async UniTask<Sprite> GetInventoryItemSprite(this ISpriteLoader spriteLoader, InventoryItemCatalog catalog, int id)
        {
            return await spriteLoader.GetSprite(catalog.GetResourcePath(id), catalog.GetSpriteKey(id));
        }
    }
}
