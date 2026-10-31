using UnityEngine;
using UnityEngine.InputSystem;

namespace IsoBuilder.Gameplay
{
    public class PointerInput
    {
        private bool IsTouch => Touchscreen.current != null &&
            (Touchscreen.current.primaryTouch.press.isPressed || Touchscreen.current.primaryTouch.press.wasPressedThisFrame
            || Touchscreen.current.primaryTouch.press.wasReleasedThisFrame);
        private bool IsMouse => Mouse.current != null;

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

        public int PointerId()
        {
            if (IsTouch)
                return Touchscreen.current.primaryTouch.touchId.ReadValue();
            return -1;
        }
    }
}
