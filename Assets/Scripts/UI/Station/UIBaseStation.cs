using IsoBuilder.Core;
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

        public virtual void Initialize(SaveDataStore saveDataStore, GameConfig gameConfig, UnityAction close)
        {
            this.saveDataStore = saveDataStore;
            this.gameConfig = gameConfig;

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
