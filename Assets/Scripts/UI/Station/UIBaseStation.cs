using IsoBuilder.Core;
using IsoBuilder.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIBaseStation : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        protected SaveDataStore saveDataStore;
        protected GameConfig gameConfig;
        protected Pooling pooling;
        protected ISpriteLoader spriteLoader;

        public virtual void Initialize(SaveDataStore saveDataStore, GameConfig gameConfig, Pooling pooling, ISpriteLoader spriteLoader, UnityAction close)
        {
            this.saveDataStore = saveDataStore;
            this.gameConfig = gameConfig;
            this.pooling = pooling;
            this.spriteLoader = spriteLoader;

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(close);
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
