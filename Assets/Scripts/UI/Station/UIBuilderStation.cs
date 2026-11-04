using IsoBuilder.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IsoBuilder.UI
{
    public class UIBuilderStation : UIBaseStation
    {
        private const BuildMode StartMode = BuildMode.Build;

        [SerializeField] private Button buildModeButton;
        [SerializeField] private Button destroyModeButton;
        [SerializeField] private TextMeshProUGUI modeText;

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
            SetupMode(StartMode);
            base.Show();
        }

        private void SetupMode(BuildMode mode)
        {
            SetModeText(mode);
            GameContext.BuildController.SetBuildMode(mode);
        }
        private void SetModeText(BuildMode mode)
        {
            modeText.text = $"Mode : {mode}";
        }

        public override void Dispose()
        {
            SetupMode(BuildMode.None);
            base.Dispose();
        }
    }
}
