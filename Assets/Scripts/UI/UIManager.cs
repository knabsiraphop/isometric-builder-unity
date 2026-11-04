using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IsoBuilder.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private UIPlaceableCatalog uiPlaceableCatalog;
        [SerializeField] private UIPlacementPanel uiPlacementPanel;
        [SerializeField] private Transform panelContainer;
        [SerializeField] private UIPopup popup;

        private UIStationController stationController;

        public IUIPopup Popup => popup;

        public void Initialize(GameContext gameContext)
        {
            stationController = new UIStationController(gameContext, panelContainer);
        }

        public void SetActiveLoadingScreen(bool isActive)
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(isActive);
            }
        }

        public async UniTask InitializePlaceableCatalog(PlaceableCatalog placeableCatalog, ISpriteLoader placeableSpriteLoader, UnityAction<PlaceableData> onSelect)
        {
            IEnumerable<PlaceableData> datas = placeableCatalog.GetPlaceableDatas();

            foreach (PlaceableData data in datas)
            {
                Sprite sprite = await placeableSpriteLoader.GetSprite(data.GetResourcePath(), data.GetSpriteKey());
                AddButtonsToList(sprite, () => onSelect(data));
            }
        }

        public void AddButtonsToList(Sprite sprite, UnityAction onClickAction)
        {
            uiPlaceableCatalog.AddButton(sprite, onClickAction);
        }

        public void ShowPlacementPanel(UnityAction onConfirm, UnityAction onCancel, UnityAction onFlip)
        {
            uiPlacementPanel.Initialize(onConfirm, onCancel, onFlip);
            uiPlacementPanel.gameObject.SetActive(true);
        }

        public void HidePlacementPanel()
        {
            uiPlacementPanel.gameObject.SetActive(false);
        }

        public void ShowStationPanel(StationType stationType)
        {
            stationController.OpenStationUI(stationType).Forget();
        }

        public void ShowPlaceableCatalog()
        {
            uiPlaceableCatalog.gameObject.SetActive(true);
        }

        public void HidePlaceableCatalog()
        {
            uiPlaceableCatalog.gameObject.SetActive(false);
        }
    }
}
