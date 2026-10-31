using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIItemButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image itemImage;

        public void SetItem(Sprite itemSprite, UnityAction onClickAction)
        {
            itemImage.sprite = itemSprite;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClickAction);
        }
    }
}
