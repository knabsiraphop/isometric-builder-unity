using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIMixerSlot : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;

        private int index;

        public int Id { get; set; } = -1;

        public void Initialize(int index, UnityAction<int, int> clearSlot)
        {
            this.index = index;
            button.onClick.AddListener(() => clearSlot(Id, this.index));
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
    }
}
