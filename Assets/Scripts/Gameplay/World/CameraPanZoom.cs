using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class CameraPanZoom
    {
        private readonly CameraController cameraController;
        private readonly PointerInput pointerInput;

        private bool isPanning;
        private Vector2 panStartWorld;

        private bool isZoom;
        private float previousDistance;

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
            if (pointerInput.IsTwoTouch())
            {
                float dist = pointerInput.TouchDistance();
                if (!isZoom)
                {
                    previousDistance = dist;
                    isZoom = true;
                }
                else 
                {
                    cameraController.PinchZoom(previousDistance/ dist);
                    previousDistance = dist;
                }
            }
            else
            {
                isZoom = false;
            }
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
            Vector2 offset = panStartWorld - CurrentPos;
            cameraController.Pan(offset);
            panStartWorld = CurrentPos;
        }

        private Vector2 CurrentPos => cameraController.ScreenToWorldPosition(pointerInput.CurrentPosition());

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
