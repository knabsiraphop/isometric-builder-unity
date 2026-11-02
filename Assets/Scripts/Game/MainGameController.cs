using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Gameplay;
using IsoBuilder.UI;
using UnityEngine;

namespace IsoBuilder
{
    public class MainGameController : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private MapController mapController;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private PlacementController placementController;
        [SerializeField] private ObjectManager objectManager;

        private PlaceableCatalog placeableCatalog;
        private PlaceableSpriteLoader placeableSpriteLoader;

        private LoadSavePlacement loadSavePlacement;
        private SaveDataStore saveDataStore;

        private async UniTaskVoid Start()
        {
            placeableCatalog = new PlaceableCatalog();
            placeableCatalog.Initialize(gameConfig.PlaceableConfig, gameConfig.StationConfig);
            placeableSpriteLoader = new PlaceableSpriteLoader();

            saveDataStore = new SaveDataStore();
            saveDataStore.LoadData();
            loadSavePlacement = new LoadSavePlacement(saveDataStore);

            uiManager.Initialize(saveDataStore, gameConfig);

            await mapController.Initialize(gameConfig.GridSystemConfig);

            cameraController.Initialize();
            cameraController.FitToBounds(mapController.GetMapBounds());

            objectManager.Initialize(mapController, loadSavePlacement);

            placementController.Initialize(uiManager, placeableCatalog, placeableSpriteLoader, mapController, objectManager, cameraController);
            await loadSavePlacement.ApplySave(placementController.PlaceObjectFromData);

            await uiManager.ShowPlaceableCatalogs(placeableCatalog, placeableSpriteLoader, (data) => placementController.PlaceObject(data).Forget());

            uiManager.SetActiveLoadingScreen(false);
        }
    }
}
