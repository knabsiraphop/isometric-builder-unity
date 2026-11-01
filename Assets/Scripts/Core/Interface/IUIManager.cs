using UnityEngine.Events;

namespace IsoBuilder.Core
{
    public interface IUIManager
    {
        public void ShowPlacementPanel(UnityAction onConfirm, UnityAction onCancel, UnityAction onFlip);
        public void HidePlacementPanel();
    }
}
