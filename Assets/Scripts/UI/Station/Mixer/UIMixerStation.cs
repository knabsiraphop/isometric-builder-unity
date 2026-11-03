using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIMixerStation : UIBaseStation
    {
        [SerializeField] private GameObject idlePanel;
        [SerializeField] private UIMixerIngredient ingredientList;
        [SerializeField] private UIMixerSlotController slotController;
        [SerializeField] private UIMixerProcess processPanel;
        [SerializeField] private Button mixButton;

        private MixerRecipeData currentMixerRecipeData;

        public override void Initialize(GameContext gameContext, UnityAction close)
        {
            base.Initialize(gameContext, close);
            ingredientList.Initialize(gameContext.Pooling, gameContext.SpriteLoader, gameContext.InventoryItemCatalog);
            slotController.Initialize(gameContext, OnSlotRemoveSelect, OnMatchRecipe);
            processPanel.Initialize(gameContext, Refresh);
            mixButton.onClick.RemoveAllListeners();
            mixButton.onClick.AddListener(ConfirmMix);
        }

        public override void Show()
        {
            base.Show();
            Refresh();
        }

        private void Refresh()
        {
            ingredientList.Dispose();
            slotController.Dispose();
            processPanel.Dispose();

            if (processPanel.gameObject.activeSelf)
                processPanel.gameObject.SetActive(false);
            if (idlePanel.gameObject.activeSelf)
                idlePanel.gameObject.SetActive(false);

            mixButton.interactable = false;
            currentMixerRecipeData = default;

            if (HasMixerState())
            {
                if (TrySetCurrentRecipeData())
                {
                    StartProcess();
                    return;
                }
            }

            ingredientList.ShowIngredients(GameContext.SaveDataStore.SaveData.InventoryItemInstanceDatas,
            (id) => OnSelectIngredient(id)).Forget();
            idlePanel.gameObject.SetActive(true);
        }

        private bool TrySetCurrentRecipeData()
        {
            MixerStateData stateData = GameContext.SaveDataStore.SaveData.MixerStateData;
            if (!GameContext.Config.MixerConfig.TryGetRecipeData(stateData.MixerRecipeId, out currentMixerRecipeData))
            {
                stateData.Clear();
                GameContext.SaveDataStore.Save();
                Debug.LogError("Recipe Not Found");
                return false;
            }
            return true;
        }

        private bool HasMixerState()
        {
            MixerStateData stateData = GameContext.SaveDataStore.SaveData.MixerStateData;
            return stateData.IsActive();
        }

        private void OnSelectIngredient(int id)
        {
            bool isSet = slotController.TryAddIngredient(id);
            if (isSet)
                ingredientList.SetSelectButton(id, true);
            slotController.RefreshIcons().Forget();
        }

        private void OnSlotRemoveSelect(int id)
        {
            ingredientList.SetSelectButton(id, false);
            currentMixerRecipeData = default;
            mixButton.interactable = false;
        }

        private void OnMatchRecipe(MixerRecipeData data)
        {
            currentMixerRecipeData = data;
            mixButton.interactable = true;
        }

        private void ConfirmMix()
        {
            if (currentMixerRecipeData.Equals(default(MixerRecipeData)))
            {
                Debug.LogError("Current recipe data is Default");
                return;
            }
            if (!GameContext.Inventory.TryConsumeItems(new int[] { currentMixerRecipeData.Ingredient1Id, currentMixerRecipeData.Ingredient2Id },
                new int[] { 1, 1 }))
            {
                Debug.LogError("Not enough item");
                return;
            }

            GameContext.SaveDataStore.SaveData.MixerStateData.SetStateData(currentMixerRecipeData.MixerRecipeId, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            GameContext.SaveDataStore.Save();

            idlePanel.SetActive(false);

            StartProcess();
        }

        private void StartProcess()
        {
            processPanel.StartProcess(currentMixerRecipeData).Forget();
            processPanel.gameObject.SetActive(true);
        }

        public override void Dispose()
        {
            mixButton.interactable = false;

            ingredientList.Dispose();
            slotController.Dispose();
            processPanel.Dispose();

            base.Dispose();
        }
    }
}
