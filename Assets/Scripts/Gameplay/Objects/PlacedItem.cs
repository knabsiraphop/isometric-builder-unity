using IsoBuilder.Core;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacedItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public ItemData ItemData { get; private set; }
        public int OwnerId { get; private set; }

        public Vector2Int OriginCell { get; private set; }
        public bool Flipped { get; private set; }
        public Vector3 WorldCellPosition { get; private set; }

        public void Initialize(ItemData itemData, Sprite sprite, int ownerId)
        {
            ItemData = itemData;
            OwnerId = ownerId;

            spriteRenderer.sprite = sprite;
        }

        public void SetPlacement(Vector2Int originCell, bool flipped, Vector3 worldCellPosition)
        {
            const int MaxSortingOrder = 100;

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
