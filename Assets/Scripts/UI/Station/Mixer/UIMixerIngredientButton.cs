using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIMixerIngredientButton : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image bg;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Button button;

        public void Initialize(Sprite iconSprite, int id, int amount, UnityAction<int> onClick)
        {
            icon.sprite = iconSprite;
            amountText.text = amount.ToString();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick(id));
        }

        public void SetSelect(bool isSelect) 
        {
            if (isSelect)
                bg.color = Color.green;
            else
                bg.color = Color.white;
        }

        public void Clear() 
        {
            icon.sprite = null;
            amountText.text = string.Empty;
            bg.color = Color.white;
        }
    }
}
