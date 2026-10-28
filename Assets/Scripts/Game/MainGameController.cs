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

        private void Start()
        {
            if (mapController != null)
            {
                mapController.Initialize();
            }

            if(cameraController != null)
            {
                cameraController.Initialize();
                cameraController.FitToBounds(mapController.GetMapBounds());
            }

            if (uiManager != null)
            {
                uiManager.SetActiveLoadingScreen(false);
            }
        }
    }
}
