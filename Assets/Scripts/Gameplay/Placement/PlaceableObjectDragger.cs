using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlaceableObjectDragger
    {
        private readonly CameraController cameraController;
        private readonly PointerInput pointerInput;
        private readonly MapController mapController;

        private bool isDrag = false;
        private Vector3 startPointerWorld;
        private Vector3 startObjectWorld;

        public PlaceableObjectDragger(CameraController cameraController, PointerInput pointerInput, MapController mapController)
        {
            this.cameraController = cameraController;
            this.pointerInput = pointerInput;
            this.mapController = mapController;
        }

        public void OnPressedThisFrame(PlacedObject placedObject)
        {
            if (pointerInput.IsPointerOverUI() || placedObject == null)
                return;
            startPointerWorld = cameraController.ScreenToWorldPosition(pointerInput.CurrentPosition());
            startObjectWorld = placedObject.WorldCellPosition;
            isDrag = true;
        }

        public bool TryGetNewCellOrigin(PlacedObject placedObject, out Vector2Int newCellOrigin)
        {
            newCellOrigin = Vector2Int.zero;
            if (!isDrag || placedObject == null)
                return false;

            Vector3 currentPosition = cameraController.ScreenToWorldPosition(pointerInput.CurrentPosition());
            Vector3 diff = currentPosition - startPointerWorld;
            Vector3 newWorldPosition = startObjectWorld + diff;
            newCellOrigin = mapController.WorldToCell(newWorldPosition);

            return newCellOrigin != placedObject.CellOrigin;
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
