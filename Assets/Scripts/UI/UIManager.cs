using IsoBuilder.Core;
using UnityEngine;

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
