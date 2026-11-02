using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIQuizChoice : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI choiceText;

        public void Initialize(UnityAction<int> onChoiceSelected, int index)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onChoiceSelected.Invoke(index));
        }

        public void SetChoiceText(string text)
        {
            choiceText.text = text;
        }
    }
}
