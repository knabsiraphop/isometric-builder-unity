using System;
using UnityEngine;

namespace IsoBuilder.Utils
{
    [RequireComponent(typeof(Camera))]
    public class CameraSafeArea : MonoBehaviour
    {
        [SerializeField] private bool xSafeArea = true;
        [SerializeField] private bool ySafeArea = true;

        private Camera cam;
        private Rect safeAreaRect;
        private int lastWidth;
        private int lastHeight;

        public event Action OnSafeAreaChanged;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            safeAreaRect = Screen.safeArea;
            lastWidth = Screen.width;
            lastHeight = Screen.height;
            Apply();
        }

        private void Update()
        {
            if (Screen.safeArea == safeAreaRect && Screen.width == lastWidth && Screen.height == lastHeight)
            {
                return;
            }

            safeAreaRect = Screen.safeArea;
            lastWidth = Screen.width;
            lastHeight = Screen.height;
            Apply();
        }

        private void Apply() 
        {
            Rect safeArea = new Rect(safeAreaRect);

            if (!xSafeArea)
            {
                safeArea.x = 0;
                safeArea.width = Screen.width;
            }

            if (!ySafeArea)
            {
                safeArea.y = 0;
                safeArea.height = Screen.height;
            }

            cam.rect = new Rect(safeArea.x / Screen.width, safeArea.y / Screen.height, safeArea.width / Screen.width, safeArea.height / Screen.height);
            OnSafeAreaChanged?.Invoke();
        }
    }
}
