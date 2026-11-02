using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Gameplay;
using IsoBuilder.UI;
using IsoBuilder.Utils;
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
        [SerializeField] private Pooling pooling;

        private PlaceableCatalog placeableCatalog;
        private SpriteLoader spriteLoader;

        private LoadSavePlacement loadSavePlacement;
        private SaveDataStore saveDataStore;

        private async UniTaskVoid Start()
        {
            placeableCatalog = new PlaceableCatalog();
            placeableCatalog.Initialize(gameConfig.PlaceableConfig, gameConfig.StationConfig);
            spriteLoader = new SpriteLoader();

            saveDataStore = new SaveDataStore(gameConfig);
            saveDataStore.LoadData();
            loadSavePlacement = new LoadSavePlacement(saveDataStore);

            uiManager.Initialize(saveDataStore, gameConfig);

            await mapController.Initialize(gameConfig.GridSystemConfig);

            cameraController.Initialize();
            cameraController.FitToBounds(mapController.GetMapBounds());

            objectManager.Initialize(mapController, loadSavePlacement, pooling);

            placementController.Initialize(uiManager, placeableCatalog, spriteLoader, mapController, objectManager, cameraController);
            await loadSavePlacement.ApplySave(placementController.PlaceObjectFromData);

            await uiManager.ShowPlaceableCatalogs(placeableCatalog, spriteLoader, (data) => placementController.PlaceObject(data).Forget());

            uiManager.SetActiveLoadingScreen(false);
        }
    }
}
