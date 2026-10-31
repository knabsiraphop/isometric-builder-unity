using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IsoBuilder.Gameplay
{
    public class PlacedItemPointer : MonoBehaviour, IPointerDownHandler
    {
        private PlacedItem placedItem;
        private Action<PlacedItem> onPointerDown;

        public void Initialize(Action<PlacedItem> onPointerDown, PlacedItem placedItem) 
        {
            this.onPointerDown = onPointerDown;
            this.placedItem = placedItem;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke(placedItem);
        }
    }
}
