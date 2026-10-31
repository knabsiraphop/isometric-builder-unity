using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IsoBuilder.Gameplay
{
    public class ItemDragger
    {
        private readonly CameraController cameraController;
        private readonly PointerInput pointerInput;
        private readonly MapController mapController;

        private bool isDrag = false;
        private Vector3 startPointerWorld;
        private Vector3 startItemWorld;

        public ItemDragger(CameraController cameraController, PointerInput pointerInput, MapController mapController)
        {
            this.cameraController = cameraController;
            this.pointerInput = pointerInput;
            this.mapController = mapController;
        }

        public void OnPressedThisFrame(PlacedItem placedItem)
        {
            if (pointerInput.IsPointerOverUI() || placedItem == null)
                return;
            startPointerWorld = cameraController.ScreenToWorldPosition(pointerInput.CurrentPosition());
            startItemWorld = placedItem.WorldCellPosition;
            isDrag = true;
        }

        public bool TryGetNewCellOrigin(PlacedItem placedItem, out Vector2Int newCellOrigin)
        {
            newCellOrigin = Vector2Int.zero;
            if (!isDrag || placedItem == null)
                return false;

            Vector3 currentPosition = cameraController.ScreenToWorldPosition(pointerInput.CurrentPosition());
            Vector3 diff = currentPosition - startPointerWorld;
            Vector3 newWorldPosition = startItemWorld + diff;
            newCellOrigin = mapController.WorldToCell(newWorldPosition);

            return newCellOrigin != placedItem.OriginCell;
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
