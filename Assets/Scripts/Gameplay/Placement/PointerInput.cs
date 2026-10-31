using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace IsoBuilder.Gameplay
{
    public class PointerInput
    {
        private PointerEventData pointerEventData;
        private List<RaycastResult> raycastResults;

        private bool IsTouch => Touchscreen.current != null &&
            (Touchscreen.current.primaryTouch.press.isPressed || Touchscreen.current.primaryTouch.press.wasPressedThisFrame
            || Touchscreen.current.primaryTouch.press.wasReleasedThisFrame);
        private bool IsMouse => Mouse.current != null;

        public PointerInput()
        {
            pointerEventData = new(EventSystem.current);
            raycastResults = new();
        }

        public Vector2 CurrentPosition()
        {
            if (IsTouch)
                return Touchscreen.current.primaryTouch.position.ReadValue();
            else if (IsMouse)
                return Mouse.current.position.ReadValue();
            return Vector2.zero;
        }

        public bool IsPressed()
        {
            if (IsTouch)
                return Touchscreen.current.primaryTouch.press.isPressed;
            else if (IsMouse)
                return Mouse.current.leftButton.isPressed;
            return false;
        }

        public bool WasPressedThisFrame()
        {
            if (IsTouch)
                return Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
            else if (IsMouse)
                return Mouse.current.leftButton.wasPressedThisFrame;
            return false;
        }

        public bool WasReleasedThisFrame()
        {
            if (IsTouch)
                return Touchscreen.current.primaryTouch.press.wasReleasedThisFrame;
            else if (IsMouse)
                return Mouse.current.leftButton.wasReleasedThisFrame;
            return false;
        }

        public bool IsPointerOverUI()
        {
            pointerEventData.position = CurrentPosition();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            foreach (RaycastResult result in raycastResults)
            {
                if (result.module is GraphicRaycaster)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
