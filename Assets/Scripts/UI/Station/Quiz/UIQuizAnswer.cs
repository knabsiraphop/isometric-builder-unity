using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIQuizAnswer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Button button;

        public void Initialize(UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        public void SetAnswerText(string messageText, string buttonText)
        {
            this.messageText.text = messageText;
            this.buttonText.text = buttonText;
        }
    }
}
