using IsoBuilder.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIBuilderStation : UIBaseStation
    {
        [SerializeField] private Button buildModeButton;
        [SerializeField] private Button destroyModeButton;
        [SerializeField] private TextMeshProUGUI modeText;

        private BuildMode currentMode = BuildMode.Build;

        public override void Initialize(GameContext gameContext, UnityAction close)
        {
            base.Initialize(gameContext, close);
            buildModeButton.onClick.RemoveAllListeners();
            buildModeButton.onClick.AddListener(() => SetupMode(BuildMode.Build));
            destroyModeButton.onClick.RemoveAllListeners();
            destroyModeButton.onClick.AddListener(() => SetupMode(BuildMode.Destroy));
        }

        public override void Show()
        {
            SetupMode(BuildMode.Build);
            base.Show();
        }

        private void SetupMode(BuildMode mode)
        {
            currentMode = mode;
            SetModeText();
        }
        private void SetModeText()
        {
            modeText.text = $"Mode : {currentMode}";
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
