using IsoBuilder.Core;
using IsoBuilder.Utils;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    [RequireComponent(typeof(Camera), typeof(CameraSafeArea))]
    public class CameraController : MonoBehaviour, ICameraController
    {
        private Camera cam;
        private CameraSafeArea cameraSafeArea;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            cameraSafeArea = GetComponent<CameraSafeArea>();

            cameraSafeArea.OnSafeAreaChanged += RefitCamera;
        }
        
        private void RefitCamera()
        {
        
        }

        private void OnDestroy()
        {
            cameraSafeArea.OnSafeAreaChanged -= RefitCamera;
        }
    }
}
