using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace IsoBuilder.UI
{
    public class UIMixerSlotController : MonoBehaviour, IDisposable
    {
        [SerializeField] private UIMixerSlot[] slots;
        [SerializeField] private UIMixerSlot resultSlot;

        private int[] selectedIds;
        private int resultId = -1;
        private MixerConfig config;
        private InventoryItemCatalog inventoryItemCatalog;
        private ISpriteLoader spriteLoader;
        private UnityAction<int> onSlotCleared;
        UnityAction<MixerRecipeData> onRecipeMatch;

        public void Initialize(GameContext gameContext, UnityAction<int> onSlotCleared, UnityAction<MixerRecipeData> onRecipeMatch)
        {
            this.config = gameContext.Config.MixerConfig;
            this.inventoryItemCatalog = gameContext.InventoryItemCatalog;
            this.spriteLoader = gameContext.SpriteLoader;
            this.onSlotCleared = onSlotCleared;
            this.onRecipeMatch = onRecipeMatch;

            selectedIds = new int[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                UIMixerSlot slot = slots[i];
                slot.Initialize(i, ClearSlotOnClick);
                selectedIds[i] = -1;
            }
        }

        private void ClearSlotOnClick(int id, int index)
        {
            ClearSlotIcon(id, index);
            ClearResultSlot();
            onSlotCleared?.Invoke(id);
        }

        public bool TryAddIngredient(int id)
        {
            if (selectedIds.Contains(id) || !selectedIds.Contains(-1))
                return false;

            for (int i = 0; i < selectedIds.Length; i++)
            {
                int slotId = selectedIds[i];
                if (slotId == -1)
                {
                    selectedIds[i] = id;
                    TrySetResultId();
                    return true;
                }
            }
            return false;
        }

        private void TrySetResultId()
        {
            if (config.TryFindRecipe(selectedIds[0], selectedIds[1], out MixerRecipeData recipe))
            {
                resultId = recipe.ResultId;
                onRecipeMatch?.Invoke(recipe);
            }
        }

        public async UniTask RefreshIcons()
        {
            for (int i = 0; i < selectedIds.Length; i++)
            {
                await SetSlotSprite(slots[i], selectedIds[i]);
            }
            await SetSlotSprite(resultSlot, resultId);
        }

        private async UniTask SetSlotSprite(UIMixerSlot mixerSlot, int id)
        {
            if (mixerSlot.Id == id || id == -1)
            {
                return;
            }
            Sprite sprite = await GetItemSprite(id);
            mixerSlot.SetIcon(sprite, id);
        }

        private void ClearResultSlot()
        {
            resultId = -1;
            resultSlot.ClearIcon();
        }

        private async UniTask<Sprite> GetItemSprite(int id)
        {
            return await spriteLoader.GetInventoryItemSprite(inventoryItemCatalog, id);
        }

        public void ClearSlotIcon(int id, int index)
        {
            if (selectedIds[index] == id)
            {
                slots[index].ClearIcon();
                selectedIds[index] = -1;
            }
        }

        public void Dispose()
        {
            resultId = -1;
            resultSlot.ClearIcon();
            for (int i = 0; i < selectedIds.Length; i++)
            {
                selectedIds[i] = -1;
                slots[i].ClearIcon();
            }
        }
    }
}
