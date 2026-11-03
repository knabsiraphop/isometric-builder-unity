using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using System;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIMixerProcess : MonoBehaviour, IDisposable
    {
        [SerializeField] private Button collectButton;
        [SerializeField] private TextMeshProUGUI timeRemainingText;
        [SerializeField] private UIMixerSlot resultMixerSlot;

        private SaveDataStore saveDataStore;
        private ISpriteLoader spriteLoader;
        private InventoryItemCatalog inventoryItemCatalog;
        private CancellationTokenSource cancellationTokenSource;
        private MixerRecipeData mixerRecipeData;
        private Inventory inventoryItem;
        private Action refreshAndshowMixerIdle;

        public void Initialize(GameContext gameContext, Action refreshAndshowMixerIdle)
        {
            this.saveDataStore = gameContext.SaveDataStore;
            this.spriteLoader = gameContext.SpriteLoader;
            this.inventoryItemCatalog = gameContext.InventoryItemCatalog;
            this.refreshAndshowMixerIdle = refreshAndshowMixerIdle;
            this.inventoryItem = gameContext.Inventory;

            collectButton.onClick.RemoveAllListeners();
            collectButton.onClick.AddListener(CollectItem);
        }

        private void CollectItem()
        {
            collectButton.interactable = false;
            saveDataStore.SaveData.MixerStateData.Clear();
            inventoryItem.AddItem(mixerRecipeData.ResultId, 1);
            refreshAndshowMixerIdle?.Invoke();
        }

        public async UniTask StartProcess(MixerRecipeData data)
        {
            mixerRecipeData = data;
            StartProcessLoop().Forget();
            await SetResultSlotSprite(data.ResultId);
        }

        private async UniTaskVoid StartProcessLoop()
        {
            cancellationTokenSource = new();
            long collectTime = saveDataStore.SaveData.MixerStateData.StartTimeStamp + mixerRecipeData.duration;

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (CanCollect(now, collectTime))
                {
                    ReadyToCollect();
                    break;
                }
                else
                {
                    long diff = now - collectTime;
                    timeRemainingText.text = TimeSpan.FromSeconds(diff).ToString(@"hh\:mm\:ss");
                    await UniTask.WaitForSeconds(1, cancellationToken: cancellationTokenSource.Token);
                }
            }
        }

        private async UniTask SetResultSlotSprite(int resultId)
        {
            Sprite sprite = await spriteLoader.GetInventoryItemSprite(inventoryItemCatalog, resultId);
            resultMixerSlot.SetIcon(sprite, resultId);
        }

        private bool CanCollect(long now, long collectTime)
        {
            return now >= collectTime;
        }

        private void ReadyToCollect()
        {
            StopProcessLoop();
            collectButton.interactable = true;
        }

        public void Dispose()
        {
            collectButton.interactable = false;

            StopProcessLoop();

            resultMixerSlot.ClearIcon();
        }

        private void StopProcessLoop() 
        {
            timeRemainingText.text = "00:00:00";
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }
    }
}
