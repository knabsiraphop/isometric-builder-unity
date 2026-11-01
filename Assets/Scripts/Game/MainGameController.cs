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

        private ItemCatalog itemCatalog;
        private ItemSpriteLoader itemSpriteLoader;

        private LoadSavePlacement loadSavePlacement;

        private async UniTaskVoid Start()
        {
            itemCatalog = new ItemCatalog();
            itemCatalog.Initialize(gameConfig.ItemConfig, gameConfig.StationConfig);
            itemSpriteLoader = new ItemSpriteLoader();

            loadSavePlacement = new LoadSavePlacement();

            await mapController.Initialize(gameConfig.GridSystemConfig);

            cameraController.Initialize();
            cameraController.FitToBounds(mapController.GetMapBounds());

            objectManager.Initialize(mapController, loadSavePlacement);

            placementController.Initialize(uiManager, itemCatalog, itemSpriteLoader, mapController, objectManager, cameraController, loadSavePlacement);
            await placementController.ApplySave();

            await uiManager.ShowItemCatalogs(itemCatalog, itemSpriteLoader, (data) => placementController.PlaceItem(data).Forget());

            uiManager.SetActiveLoadingScreen(false);
        }
    }
}
