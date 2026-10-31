using System;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class ItemPressDetector
    {
        private const float HoldTime = 0.5f;
        private const float HoldCancelDistancePx = 10f;

        private PointerInput pointerInput;

        private bool isPressed;
        private Vector2 pressedPoint;
        private PlacedItem lastPressedItem;
        private float pressedTime;
        private Action<PlacedItem> selectItem;

        public ItemPressDetector(PointerInput pointerInput, Action<PlacedItem> selectItem)
        {
            this.pointerInput = pointerInput;
            this.selectItem = selectItem;
        }

        public void OnPressedThisFrame()
        {
            if (pointerInput.IsPointerOverUI())
                return;
            pressedTime = 0;
            pressedPoint = pointerInput.CurrentPosition();
            isPressed = true;
        }

        public void OnItemPressed(PlacedItem placedItem)
        {
            if (lastPressedItem != null)
            {
                Debug.LogWarning($"Item {placedItem.OwnerId} pressed while already pressed {lastPressedItem.OwnerId}");
            }
            lastPressedItem = placedItem;
        }

        public void OnPressed()
        {
            if (!isPressed || lastPressedItem == null)
                return;
            Vector2 currentPoint = pointerInput.CurrentPosition();
            Vector2 diff = pressedPoint - currentPoint;
            if (Mathf.Abs(diff.x) >= HoldCancelDistancePx || Mathf.Abs(diff.y) >= HoldCancelDistancePx)
            {
                Reset();
                return;
            }
            pressedTime += Time.deltaTime;
            if (pressedTime >= HoldTime)
            {
                selectItem?.Invoke(lastPressedItem);
                Reset();
            }
        }

        public void OnReleasedThisFrame()
        {
            Reset();
        }

        public void Reset()
        {
            isPressed = false;
            lastPressedItem = null;
            pressedTime = 0;
        }
    }
}
