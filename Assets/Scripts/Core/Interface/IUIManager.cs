using UnityEngine.Events;

namespace IsoBuilder.Core
{
    public interface IUIManager
    {
        public void ShowItemCatalog();
        public void HideItemCatalog();

        public void ShowPlacementPanel(UnityAction onConfirm, UnityAction onCancel, UnityAction onFlip);
        public void HidePlacementPanel();
        public void ShowStationPanel(StationType stationType);
    }
}
