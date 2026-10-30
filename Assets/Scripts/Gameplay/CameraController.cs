using IsoBuilder.Core;
using IsoBuilder.Utils;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        [SerializeField] private Camera cam;
        [SerializeField] private CameraSafeArea cameraSafeArea;

        private Bounds mapBounds;

        public void Initialize()
        {
            cameraSafeArea.OnSafeAreaChanged += RefitCamera;
        }

        public void FitToBounds(Bounds bounds)
        {
            mapBounds = bounds;
            cam.transform.position = new Vector3(bounds.center.x, bounds.center.y, cam.transform.position.z);
            RefitCamera();
        }

        private void RefitCamera()
        {
            const float boundsPadding = 1.1f;

            if (mapBounds.Equals(default))
                return;

            float halfHeight = mapBounds.size.y / 2f;
            float halfWidthAsSize = mapBounds.size.x / (2f * cam.aspect);

            float size = Mathf.Max(halfHeight, halfWidthAsSize) * boundsPadding;
            SetCameraSize(size);
        }

        public void SetCameraSize(float size)
        {
            cam.orthographicSize = size;
        }

        public Vector2 GetScreenCenter() 
        {
            return cam.pixelRect.center;
        }
       
        public Vector2 ScreenToWorldPosition(Vector2 screenPosition) 
        {
            return cam.ScreenToWorldPoint(screenPosition);
        }

        private void OnDestroy()
        {
            cameraSafeArea.OnSafeAreaChanged -= RefitCamera;
        }
    }
}
