using IsoBuilder.Core;
using IsoBuilder.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIBaseStation : MonoBehaviour, IDisposable
    {
        [SerializeField] private Button closeButton;

        protected GameContext GameContext { get; private set; }

        public virtual void Initialize(GameContext gameContext, UnityAction close)
        {
            GameContext = gameContext;

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

        public virtual void Dispose()
        {
        }
    }
}
