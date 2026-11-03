using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIMixerSlot : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;

        public int Id { get; private set; }

        public void Initialize(UnityAction onClick)
        {
            button.onClick.AddListener(() =>
            {
                onClick();
                ClearIcon();
            });
        }

        public void SetIcon(Sprite sprite, int id)
        {
            iconImage.enabled = true;
            iconImage.sprite = sprite;
            this.Id = id;
        }

        public void ClearIcon()
        {
            iconImage.enabled = false;
            iconImage.sprite = null;
            Id = -1;
        }

        public bool IsEmpty()
        {
            return Id == -1;
        }
    }
}
