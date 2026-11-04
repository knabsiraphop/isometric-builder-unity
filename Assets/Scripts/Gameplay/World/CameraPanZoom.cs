using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class CameraPanZoom
    {
        private readonly CameraController cameraController;
        private readonly PointerInput pointerInput;

        private bool isPanning;
        private Vector2 panStartWorld;

        public CameraPanZoom(CameraController cameraController, PointerInput pointerInput)
        {
            this.cameraController = cameraController;
            this.pointerInput = pointerInput;
        }

        public void Update()
        {
#if UNITY_EDITOR
            if (pointerInput.CurrentScroll() != 0f)
            {
                cameraController.Zoom(pointerInput.CurrentScroll());
            }
#endif
        }

        public void OnPressedThisFrame(PlacedObject currentPlacedObject)
        {
            if (!pointerInput.IsPointerOverUI() && currentPlacedObject == null)
            {
                panStartWorld = cameraController.ScreenToWorldPosition(pointerInput.CurrentPosition());
                isPanning = true;
            }
        }

        public void OnPressed()
        {
            if (!isPanning)
                return;
            Vector2 offset = panStartWorld - cameraController.ScreenToWorldPosition(pointerInput.CurrentPosition());
            cameraController.Pan(offset);
        }

        public void OnReleasedThisFrame()
        {
            isPanning = false;
        }

        public void Reset()
        {
            isPanning = false;
            panStartWorld = Vector2.zero;
        }
    }
}
