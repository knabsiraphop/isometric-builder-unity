using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIBaseStation : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        public void Initialize(UnityAction close)
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(close);
            }
        }
    }
}
