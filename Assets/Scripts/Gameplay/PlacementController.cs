using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacementController : MonoBehaviour
    {
        private IUIManager uiManager;
        private ItemCatalog itemCatalog;
        private ItemSpriteLoader itemSpriteLoader;

        public void Initialize(IUIManager uiManager, ItemCatalog itemCatalog, ItemSpriteLoader itemSpriteLoader)
        {
            this.uiManager = uiManager;
            this.itemCatalog = itemCatalog;
            this.itemSpriteLoader = itemSpriteLoader;
        }

        public async UniTask ShowItemCatalogs()
        {
            if (itemCatalog != null && uiManager != null)
            {
                foreach (ItemData item in itemCatalog.GetItems())
                {
                    Sprite itemSprite = await itemSpriteLoader.GetSprite(item);
                    uiManager.AddItemsToList(itemSprite, () => PlaceItem(item));
                }
            }
        }

        private void PlaceItem(ItemData item)
        {
            // Implement the logic to place the item in the game world
            Debug.Log("Item placed!");
        }
    }
}
