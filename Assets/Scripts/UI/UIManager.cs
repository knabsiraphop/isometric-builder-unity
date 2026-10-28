using IsoBuilder.Core;
using UnityEngine;

namespace IsoBuilder.UI
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject loadingScreen;

        public void SetActiveLoadingScreen(bool isActive)
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(isActive);
            }
        }
    }
}
