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

        public override void Initialize(SaveDataStore saveDataStore, GameConfig gameConfig, Pooling pooling, ISpriteLoader spriteLoader, UnityAction close)
        {
            base.Initialize(saveDataStore, gameConfig, pooling, spriteLoader, close);
            mixerConfig = gameConfig.MixerConfig;
            mixerStateData = saveDataStore.SaveData.MixerStateData;
            uIMixerIngredient.Initialize(pooling, spriteLoader);
        }

        public override void Show()
        {
            base.Show();
            uIMixerIngredient.ShowIngredients(saveDataStore.SaveData.InventoryItemInstanceDatas, OnSelectIngredient).Forget();
        }

        private void OnSelectIngredient(int id)
        {
            //var data = saveDataStore.SaveData.InventoryItemInstanceDatas.FirstOrDefault(x => x.Id == id);
            //if (data != null)
            {
                Debug.Log($"Selected ingredient: {id}");
            }
        }
    }
}
