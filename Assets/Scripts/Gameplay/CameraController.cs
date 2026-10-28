using IsoBuilder.Core;
using ISOBuilder.Utils;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        [SerializeField] private Camera cam;
        [SerializeField] private CameraSafeArea cameraSafeArea;

        private void Awake()
        {
            cameraSafeArea.OnSafeAreaChanged += RefitCamera;
        }
        
        private void RefitCamera()
        {
        
        }
    }
}
