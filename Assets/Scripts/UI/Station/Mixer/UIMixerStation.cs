using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IsoBuilder.UI
{
    public class UIMixerStation : UIBaseStation
    {
        [SerializeField] private UIMixerIngredient uIMixerIngredient;

        private MixerConfig mixerConfig;
        private MixerStateData mixerStateData;

        public override void Initialize(GameContext gameContext, UnityAction close)
        {
            base.Initialize(gameContext, close);
            mixerConfig = gameContext.Config.MixerConfig;
            mixerStateData = gameContext.SaveDataStore.SaveData.MixerStateData;
            uIMixerIngredient.Initialize(gameContext.Pooling, gameContext.SpriteLoader, gameContext.InventoryItemCatalog);
        }

        public override void Show()
        {
            base.Show();
            uIMixerIngredient.ShowIngredients(GameContext.SaveDataStore.SaveData.InventoryItemInstanceDatas, OnSelectIngredient).Forget();
        }

        private void OnSelectIngredient(int id)
        {
            //var data = gameContext.SaveDataStore.SaveData.InventoryItemInstanceDatas.FirstOrDefault(x => x.Id == id);
            //if (data != null)
            {
                Debug.Log($"Selected ingredient: {id}");
            }
        }

        public override void Dispose()
        {
            uIMixerIngredient.Dispose();
            base.Dispose();
        }
    }
}
