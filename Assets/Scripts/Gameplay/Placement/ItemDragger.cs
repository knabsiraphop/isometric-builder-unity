using UnityEngine;
using UnityEngine.EventSystems;

namespace IsoBuilder.Gameplay
{
    public class ItemDragger
    {
        private readonly CameraController cameraController;
        private readonly PointerInput pointerInput;
        private readonly MapController mapController;

        private bool isDrag = false;
        private Vector3 startPosition;
        private Vector3 startPlacedPosition;

        public ItemDragger(CameraController cameraController, PointerInput pointerInput, MapController mapController)
        {
            this.cameraController = cameraController;
            this.pointerInput = pointerInput;
            this.mapController = mapController;
        }

        public void OnPressedThisFrame(Vector3 worldOriginCellPosition)
        {
            if (EventSystem.current.IsPointerOverGameObject(pointerInput.PointerId()))
                return;
            startPosition = cameraController.ScreenToWorldPosition(pointerInput.CurrentPosition());
            startPlacedPosition = worldOriginCellPosition;
            isDrag = true;
        }

        public bool TryGetNewCellOrigin(Vector2Int originCell, out Vector2Int newCellOrigin)
        {
            newCellOrigin = Vector2Int.zero;
            if (!isDrag)
                return false;

            Vector3 currentPosition = cameraController.ScreenToWorldPosition(pointerInput.CurrentPosition());
            Vector3 diff = currentPosition - startPosition;
            Vector3 newWorldPosition = startPlacedPosition + diff;
            newCellOrigin = mapController.WorldToCell(newWorldPosition);

            return newCellOrigin != originCell;
        }

        public void OnReleasedThisFrame()
        {
            Reset();
        }

        public void Reset()
        {
            isDrag = false;
        }
    }
}
