using IsoBuilder.Core;
using IsoBuilder.Utils;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        [SerializeField] private Camera cam;
        [SerializeField] private CameraSafeArea cameraSafeArea;

        private float fitSize;

        private Bounds mapBounds;

        public void Initialize()
        {
            cameraSafeArea.OnSafeAreaChanged += RefitCamera;
        }

        public void ResetCamera()
        {
            MoveCamera(mapBounds.center);
            RefitCamera();
        }

        public void FitToBounds(Bounds bounds)
        {
            mapBounds = bounds;
            MoveCamera(bounds.center);
            RefitCamera();
        }

        private void RefitCamera()
        {
            const float boundsPadding = 1.1f;

            if (mapBounds.Equals(default))
                return;

            float halfHeight = mapBounds.size.y / 2f;
            float halfWidthAsSize = mapBounds.size.x / (2f * cam.aspect);

            fitSize = Mathf.Max(halfHeight, halfWidthAsSize) * boundsPadding;
            SetCameraSize(fitSize);
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

        public void Pan(Vector2 offset)
        {
            MoveCamera(cam.transform.position + new Vector3(offset.x, offset.y, 0));
        }

        public void MoveCamera(Vector3 position)
        {
            var x = Mathf.Clamp(position.x, mapBounds.min.x, mapBounds.max.x);
            var y = Mathf.Clamp(position.y, mapBounds.min.y, mapBounds.max.y);
            cam.transform.position = new Vector3(x, y, cam.transform.position.z);
        }

        public void Zoom(float increment)
        {
            float size = Mathf.Clamp(cam.orthographicSize - increment, fitSize / 2f, fitSize);
            SetCameraSize(size);
        }

        private void OnDestroy()
        {
            cameraSafeArea.OnSafeAreaChanged -= RefitCamera;
        }
    }
}
