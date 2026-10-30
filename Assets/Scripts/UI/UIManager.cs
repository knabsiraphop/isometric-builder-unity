using IsoBuilder.Core;
using UnityEngine;
using UnityEngine.Events;

namespace IsoBuilder.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private UIItemCatalog uiItemCatalog;

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
            throw new System.NotImplementedException();
        }

        public void HidePlacementPanel()
        {
            throw new System.NotImplementedException();
        }
    }
}
