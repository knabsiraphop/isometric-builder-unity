using IsoBuilder.Core;
using System;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacedItem : MonoBehaviour
    {
        private const int MaxSortingOrder = 1000;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private PlacedItemPointer placedItemPointer;

        public ItemData ItemData { get; private set; }
        public int InstanceId { get; private set; }

        public Vector2Int CellOrigin { get; private set; }
        public bool Flipped { get; private set; } = false;
        public Vector3 WorldCellPosition { get; private set; }
        public Vector2Int CellMin { get; private set; }
        public Vector2Int CellMax { get; private set; }
        public Rect WorldRect { get; private set; }

        public void Initialize(ItemData itemData, Sprite sprite, int instanceId)
        {
            ItemData = itemData;
            InstanceId = instanceId;

            spriteRenderer.sprite = sprite;

            Bounds spriteBound = spriteRenderer.sprite.bounds;
            boxCollider.offset = new Vector2(spriteBound.center.x, spriteBound.center.y);
            boxCollider.size = new Vector2(spriteBound.size.x, spriteBound.size.y);
        }

        public void InitializePointer(Action<PlacedItem> onItemPressed)
        {
            placedItemPointer.Initialize(onItemPressed, this);
        }

        public void SetPlacement(Vector2Int originCell, bool flipped, Vector3 worldCellPosition)
        {
            CellOrigin = originCell;
            Flipped = flipped;
            WorldCellPosition = worldCellPosition;

            Vector2 offset = flipped ? new Vector2(ItemData.SpriteOffset.x * -1f, ItemData.SpriteOffset.y) : ItemData.SpriteOffset;
            transform.position = worldCellPosition + (Vector3)offset;
            spriteRenderer.flipX = flipped;

            CellMin = originCell;
            Vector2Int footprint = flipped ? new Vector2Int(ItemData.FootPrint.y - 1, ItemData.FootPrint.x - 1) :
                new Vector2Int(ItemData.FootPrint.x - 1, ItemData.FootPrint.y - 1);
            CellMax = originCell + footprint;

            spriteRenderer.sortingOrder = MaxSortingOrder - (CellMin.x + CellMax.x + CellMin.y + CellMax.y);
        }

        public void SetSortingOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }
    }
}
