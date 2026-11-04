using System;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlaceableObjectPressDetector
    {
        private const float HoldTime = 0.5f;
        private const float HoldCancelDistancePx = 10f;

        private PointerInput pointerInput;

        private bool isPressed;
        private Vector2 pressedPoint;
        private PlacedObject lastPressedObject;
        private float pressedTime;
        private Action<PlacedObject> selectObject;
        private Action<PlacedObject> tapObject;

        public PlaceableObjectPressDetector(PointerInput pointerInput, Action<PlacedObject> selectObject, Action<PlacedObject> tapObject)
        {
            this.pointerInput = pointerInput;
            this.selectObject = selectObject;
            this.tapObject = tapObject;
        }

        public void OnPressedThisFrame()
        {
            if (pointerInput.IsPointerOverUI())
                return;
            pressedTime = 0;
            pressedPoint = pointerInput.CurrentPosition();
            isPressed = true;
        }

        public void OnObjectPressed(PlacedObject placedObject)
        {
            if (lastPressedObject != null)
            {
                Debug.LogWarning($"Object {placedObject.InstanceId} pressed while already pressed {lastPressedObject.InstanceId}");
            }
            lastPressedObject = placedObject;
        }

        public void OnPressed()
        {
            if (!isPressed || lastPressedObject == null)
                return;
            if (!IsWithinTapDistance())
            {
                Reset();
                return;
            }
            pressedTime += Time.deltaTime;
            if (pressedTime >= HoldTime)
            {
                selectObject?.Invoke(lastPressedObject);
                Reset();
            }
        }

        public void OnReleasedThisFrame()
        {
            if (isPressed && lastPressedObject != null && IsWithinTapDistance())
            {
                tapObject?.Invoke(lastPressedObject);
            }
            Reset();
        }

        private bool IsWithinTapDistance()
        {
            Vector2 currentPoint = pointerInput.CurrentPosition();
            Vector2 diff = pressedPoint - currentPoint;
            return Mathf.Abs(diff.x) < HoldCancelDistancePx && Mathf.Abs(diff.y) < HoldCancelDistancePx;
        }

        public void Reset()
        {
            isPressed = false;
            lastPressedObject = null;
            pressedTime = 0;
        }
    }
}
