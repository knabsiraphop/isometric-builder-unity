using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IsoBuilder.Gameplay
{
    public class PlacedObjectPointer : MonoBehaviour, IPointerDownHandler
    {
        private PlacedObject placedObject;
        private Action<PlacedObject> onPointerDown;

        public void Initialize(Action<PlacedObject> onPointerDown, PlacedObject placedObject) 
        {
            this.onPointerDown = onPointerDown;
            this.placedObject = placedObject;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke(placedObject);
        }
    }
}
