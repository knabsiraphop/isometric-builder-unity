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
        private Action<PlacedItem> tapItem;

        public ItemPressDetector(PointerInput pointerInput, Action<PlacedItem> selectItem, Action<PlacedItem> tabItem)
        {
            this.pointerInput = pointerInput;
            this.selectItem = selectItem;
            this.tapItem = tabItem;
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
                Debug.LogWarning($"Item {placedItem.InstanceId} pressed while already pressed {lastPressedItem.InstanceId}");
            }
            lastPressedItem = placedItem;
        }

        public void OnPressed()
        {
            if (!isPressed || lastPressedItem == null)
                return;
            if (!IsWithinTapDistance())
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
            if (isPressed && lastPressedItem != null && IsWithinTapDistance())
            {
                tapItem?.Invoke(lastPressedItem);
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
            lastPressedItem = null;
            pressedTime = 0;
        }
    }
}
