using IsoBuilder.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIPopup : MonoBehaviour, IUIPopup
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button confirmButton;

        public void ShowPopup(string message, UnityAction onConfirm = null)
        {
            messageText.text = message;
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() =>
            {
                SetActive(false);
                onConfirm?.Invoke();
            });
            SetActive(true);
        }

        private void SetActive(bool isActive)
        {
            if (gameObject.activeSelf != isActive)
                gameObject.SetActive(isActive);
        }
    }
}
