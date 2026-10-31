using IsoBuilder.Core;
using System;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacedItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private PlacedItemPointer placedItemPointer;

        public ItemData ItemData { get; private set; }
        public int OwnerId { get; private set; }

        public Vector2Int OriginCell { get; private set; }
        public bool Flipped { get; private set; } = false;
        public Vector3 WorldCellPosition { get; private set; }

        public void Initialize(ItemData itemData, Sprite sprite, int ownerId)
        {
            ItemData = itemData;
            OwnerId = ownerId;

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
            OriginCell = originCell;
            Flipped = flipped;
            WorldCellPosition = worldCellPosition;

            Vector2 offset = flipped ? new Vector2(ItemData.SpriteOffset.x * -1f, ItemData.SpriteOffset.y) : ItemData.SpriteOffset;
            transform.position = worldCellPosition + (Vector3)offset;
            spriteRenderer.flipX = flipped;
            spriteRenderer.sortingOrder = MaxSortingOrder - (OriginCell.x + OriginCell.y);
        }
    }
}
