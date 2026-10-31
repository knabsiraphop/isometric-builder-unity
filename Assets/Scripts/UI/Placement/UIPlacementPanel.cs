using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIPlacementPanel : MonoBehaviour
    {
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button flipButton;

        public void Initialize(UnityAction onConfirm, UnityAction onCancel, UnityAction onFlip)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(onCancel);

            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(onConfirm);

            flipButton.onClick.RemoveAllListeners();
            flipButton.onClick.AddListener(onFlip);
        }
    }
}
