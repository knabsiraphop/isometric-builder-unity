using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace IsoBuilder.UI
{
    public class UIStationController
    {
        private readonly Transform container;
        private StationType currentStationType;

        private Dictionary<StationType, UIBaseStation> stationUIs;

        public UIStationController(Transform container)
        {
            this.container = container;
            stationUIs = new Dictionary<StationType, UIBaseStation>();
        }

        public async UniTask OpenStationUI(StationType stationType)
        {
            if (stationType == StationType.None)
            {
                Debug.LogError("Invalid station type.");
                return;
            }

            if (stationType == currentStationType)
            {
                Debug.LogWarning($"Station UI for this type {stationType} is already open.");
                return;
            }

            if (currentStationType != StationType.None)
            {
                Debug.LogWarning($"A station UI is already open: {currentStationType}. Closing it before opening {stationType}.");
                return;
            }

            currentStationType = stationType;
            UIBaseStation stationUI = await GetStationUI(stationType);
            if (stationUI == null)
            {
                currentStationType = StationType.None;
                Debug.LogError($"Station UI for {stationType} not found.");
                return;
            }
            CloseCurrentStationUI();
            currentStationType = stationType;
            stationUI.gameObject.SetActive(true);
        }

        private async UniTask<UIBaseStation> GetStationUI(StationType stationType)
        {
            if (stationUIs.TryGetValue(stationType, out UIBaseStation stationUI))
            {
                return stationUI;
            }
            UIBaseStation stationUIPrefab = await ResourceLoader.LoadResourceAsync<UIBaseStation>($"Prefabs/UI/{stationType}");
            if (stationUIPrefab == null)
            {
                return null;
            }
            stationUI = GameObject.Instantiate(stationUIPrefab, container);
            stationUI.Initialize(() => CloseCurrentStationUI());
            stationUIs[stationType] = stationUI;
            return stationUI;
        }

        private void CloseCurrentStationUI()
        {
            if(currentStationType == StationType.None)
            {
                return;
            }
            if (stationUIs.TryGetValue(currentStationType, out UIBaseStation stationUI))
            {
                stationUI.gameObject.SetActive(false);
                currentStationType = StationType.None;
            }
        }
    }
}
