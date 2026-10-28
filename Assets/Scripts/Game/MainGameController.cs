using UnityEngine;

namespace IsoBuilder
{
    public class MainGameController : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;

        private void Start()
        {
            if (uiManager != null)
            {
                uiManager.SetActiveLoadingScreen(true);
            }
        }
    }
}
