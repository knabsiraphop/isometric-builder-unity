using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIMixerIngredient : MonoBehaviour, IDisposable
    {
        private const int PreloadCount = 5;
        private const int PaddingAndSpace = 10;
        private const int FixedColumnCount = 2;
        private const int FixedRowCount = 4;

        [SerializeField] private Transform container;
        [SerializeField] private UIMixerIngredientButton buttonPrefab;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;

        private Pooling pooling;
        private ISpriteLoader spriteLoader;
        private InventoryItemCatalog inventoryItemCatalog;
        private Dictionary<int, UIMixerIngredientButton> ingredientButtons;

        public void Initialize(Pooling pooling, ISpriteLoader spriteLoader, InventoryItemCatalog inventoryItemCatalog)
        {
            ingredientButtons = new();
            this.pooling = pooling;
            this.spriteLoader = spriteLoader;
            this.inventoryItemCatalog = inventoryItemCatalog;
            pooling.PreLoad(buttonPrefab.gameObject, PreloadCount);

            gridLayoutGroup.SetCellSize(rectTransform, FixedColumnCount, FixedRowCount, PaddingAndSpace, PaddingAndSpace);
        }

        public async UniTask ShowIngredients(InventoryItemInstanceData[] instanceDatas, UnityAction<int> selectIngredient)
        {
            foreach (InventoryItemInstanceData data in instanceDatas)
            {
                UIMixerIngredientButton button = pooling.GetObjectUI(buttonPrefab, container);
                Sprite sprite = await spriteLoader.GetInventoryItemSprite(inventoryItemCatalog, data.Id);
                button.Initialize(sprite, data.Id, data.Amount, selectIngredient);
                ingredientButtons[data.Id] = button;
            }
        }

        public void SetSelectButton(int id, bool isSelect)
        {
            if (ingredientButtons.TryGetValue(id, out UIMixerIngredientButton button))
            {
                button.SetSelect(isSelect);
            }
        }

        public void Dispose()
        {
            foreach (UIMixerIngredientButton button in ingredientButtons.Values)
            {
                button.Clear();
                pooling.ReturnObject(buttonPrefab.name, button.gameObject);
            }
            ingredientButtons.Clear();
        }
    }
}
