using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIPlaceableButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        public void SetButton(Sprite sprite, UnityAction onClickAction)
        {
            image.sprite = sprite;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClickAction);
        }
    }
}
