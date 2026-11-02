using IsoBuilder.Core;
using System;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacedObject : MonoBehaviour
    {
        private const int MaxSortingOrder = 1000;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private PlacedObjectPointer placedObjectPointer;

        public PlaceableData PlaceableData { get; private set; }
        public int InstanceId { get; private set; }

        public Vector2Int CellOrigin { get; private set; }
        public bool Flipped { get; private set; } = false;
        public Vector3 WorldCellPosition { get; private set; }
        public Vector2Int CellMin { get; private set; }
        public Vector2Int CellMax { get; private set; }
        public Rect WorldRect { get; private set; }

        public void Initialize(PlaceableData placeableData, Sprite sprite, int instanceId)
        {
            PlaceableData = placeableData;
            InstanceId = instanceId;

            spriteRenderer.sprite = sprite;

            Bounds spriteBound = spriteRenderer.sprite.bounds;
            boxCollider.offset = new Vector2(spriteBound.center.x, spriteBound.center.y);
            boxCollider.size = new Vector2(spriteBound.size.x, spriteBound.size.y);
        }

        public void InitializePointer(Action<PlacedObject> onObjectPressed)
        {
            placedObjectPointer.Initialize(onObjectPressed, this);
        }

        public void SetPlacement(Vector2Int originCell, bool flipped, Vector3 worldCellPosition)
        {
            CellOrigin = originCell;
            Flipped = flipped;
            WorldCellPosition = worldCellPosition;

            Vector2 offset = flipped ? new Vector2(PlaceableData.SpriteOffset.x * -1f, PlaceableData.SpriteOffset.y) : PlaceableData.SpriteOffset;
            transform.position = worldCellPosition + (Vector3)offset;
            spriteRenderer.flipX = flipped;

            CellMin = originCell;
            Vector2Int footprint = flipped ? new Vector2Int(PlaceableData.FootPrint.y - 1, PlaceableData.FootPrint.x - 1) :
                new Vector2Int(PlaceableData.FootPrint.x - 1, PlaceableData.FootPrint.y - 1);
            CellMax = originCell + footprint;

            spriteRenderer.sortingOrder = MaxSortingOrder - (CellMin.x + CellMax.x + CellMin.y + CellMax.y);
        }

        public void SetSortingOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }
    }
}
