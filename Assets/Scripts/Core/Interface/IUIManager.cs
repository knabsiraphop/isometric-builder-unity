using UnityEngine;
using UnityEngine.Events;

namespace IsoBuilder.Core
{
    public interface IUIManager
    {
        public void SetActiveLoadingScreen(bool isActive);
        public void AddItemsToList(Sprite itemSprite, UnityAction onClickAction);
        public void ShowPlacementPanel(UnityAction onConfirm, UnityAction onCancel, UnityAction onFlip);
        public void HidePlacementPanel();
    }
}
