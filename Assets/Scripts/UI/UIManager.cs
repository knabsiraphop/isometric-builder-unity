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
        [SerializeField] private UIItemCatalog uiItemCatalog;
        [SerializeField] private UIPlacementPanel uiPlacementPanel;
        [SerializeField] private Transform panelContainer;

        private UIStationController stationController;

        private void Awake()
        {
            stationController = new UIStationController(panelContainer);
        }

        public void SetActiveLoadingScreen(bool isActive)
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(isActive);
            }
        }

        public async UniTask ShowItemCatalogs(ItemCatalog itemCatalog, IItemSpriteLoader itemSpriteLoader, UnityAction<ItemData> onItemSelect)
        {
            IEnumerable<ItemData> items = itemCatalog.GetItems();

            foreach (ItemData item in items)
            {
                Sprite itemSprite = await itemSpriteLoader.GetSprite(item);
                AddItemsToList(itemSprite, () => onItemSelect(item));
            }
            ShowItemCatalog();
        }

        public void AddItemsToList(Sprite itemSprite, UnityAction onClickAction)
        {
            uiItemCatalog.AddItem(itemSprite, onClickAction);
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

        public void ShowItemCatalog()
        {
            uiItemCatalog.gameObject.SetActive(true);
        }

        public void HideItemCatalog()
        {
            uiItemCatalog.gameObject.SetActive(false);
        }
    }
}
