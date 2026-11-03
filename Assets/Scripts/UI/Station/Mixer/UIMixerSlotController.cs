using IsoBuilder.Core;
using UnityEngine;
using UnityEngine.Events;

namespace IsoBuilder.UI
{
    public class UIMixerSlotController : MonoBehaviour
    {
        [SerializeField] private UIMixerSlot[] slots;
        [SerializeField] private UIMixerSlot resultSlot;

        private MixerConfig config;

        public void Initialize(MixerConfig config, UnityAction removeSeleted)
        {
            this.config = config;
            foreach (UIMixerSlot slot in slots)
            {
                slot.Initialize(removeSeleted);
            }
        }

        public void SetSlotIcon(int id, Sprite sprite)
        {
            foreach (UIMixerSlot slot in slots)
            {
                if (slot.IsEmpty())
                {
                    slot.SetIcon(sprite, id);
                    break;
                }
            }
        }

        public void ClearSlotIcon(int id)
        {
            foreach (UIMixerSlot slot in slots)
            {
                if (!slot.IsEmpty() && slot.Id == id)
                {
                    slot.ClearIcon();
                    break;
                }
            }
        }
    }
}
