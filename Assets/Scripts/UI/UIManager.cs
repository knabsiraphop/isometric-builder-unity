using IsoBuilder.Core;
using UnityEngine;
using UnityEngine.Events;

namespace IsoBuilder.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private UIItemCatalog uiItemCatalog;
        [SerializeField] private UIPlacementPanel uiPlacementPanel;

        public void SetActiveLoadingScreen(bool isActive)
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(isActive);
            }
        }

        public void AddItemsToList(Sprite itemSprite, UnityAction onClickAction)
        {
            uiItemCatalog.AddItem(itemSprite, onClickAction);
        }

        public void ShowPlacementPanel(UnityAction onConfirm, UnityAction onCancel, UnityAction onFlip)
        {
            uiPlacementPanel.Initialize(onConfirm, onCancel, onFlip);
            uiPlacementPanel.gameObject.SetActive(true);
        }

        public void HidePlacementPanel()
        {
            uiPlacementPanel.gameObject.SetActive(false);
        }
    }
}
