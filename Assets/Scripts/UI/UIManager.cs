using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
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

        private UIStationController stationController;

        public void Initialize(SaveDataStore saveDataStore, GameConfig gameConfig)
        {
            stationController = new UIStationController(saveDataStore, gameConfig, panelContainer);
        }

        public void SetActiveLoadingScreen(bool isActive)
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(isActive);
            }
        }

        public async UniTask ShowPlaceableCatalogs(PlaceableCatalog placeableCatalog, IPlaceableSpriteLoader placeableSpriteLoader, UnityAction<PlaceableData> onSelect)
        {
            IEnumerable<PlaceableData> datas = placeableCatalog.GetPlaceableDatas();

            foreach (PlaceableData data in datas)
            {
                Sprite sprite = await placeableSpriteLoader.GetSprite(data);
                AddButtonsToList(sprite, () => onSelect(data));
            }
            ShowPlaceableCatalog();
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
