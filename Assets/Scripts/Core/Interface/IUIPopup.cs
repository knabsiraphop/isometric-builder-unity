using UnityEngine.Events;

namespace IsoBuilder.Core
{
    public interface IUIPopup
    {
        public void ShowPopup(string message, UnityAction onConfirm = null);
    }
}
