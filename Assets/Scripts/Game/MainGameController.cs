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

        private ItemCatalog itemCatalog;

        private async UniTaskVoid Start()
        {
            itemCatalog = new ItemCatalog();
            itemCatalog.Initialize(gameConfig.ItemConfig);


            await mapController.Initialize(gameConfig.GridSystemConfig);

            cameraController.Initialize();
            cameraController.FitToBounds(mapController.GetMapBounds());

            uiManager.SetActiveLoadingScreen(false);
        }
    }
}
