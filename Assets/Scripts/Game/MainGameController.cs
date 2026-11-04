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

        private LoadSavePlacement loadSavePlacement;
        private GameContext gameContext;

        private async UniTaskVoid Start()
        {
            Application.targetFrameRate = 60;

            gameContext = new GameContext(gameConfig, new SpriteLoader(), pooling, placementController, uiManager.Popup);

            loadSavePlacement = new LoadSavePlacement(gameContext.SaveDataStore);

            uiManager.Initialize(gameContext);

            await mapController.Initialize(gameConfig.GridSystemConfig);

            cameraController.Initialize();
            cameraController.FitToBounds(mapController.GetMapBounds());

            objectManager.Initialize(mapController, loadSavePlacement, pooling);

            placementController.Initialize(gameContext, uiManager, mapController, objectManager, cameraController);
            await loadSavePlacement.ApplySave(placementController.PlaceObjectFromData);

            await uiManager.InitializePlaceableCatalog(gameContext.PlaceableCatalog, gameContext.SpriteLoader, (data) => placementController.PlaceObject(data).Forget());

            uiManager.SetActiveLoadingScreen(false);
        }
    }
}
