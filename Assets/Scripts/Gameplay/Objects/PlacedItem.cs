using IsoBuilder.Core;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacedItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public ItemData ItemData { get; private set; }
        public int OwnerId { get; private set; }

        public Vector2Int Origin { get; private set; }
        public bool Flipped { get; private set; }

        public void Initialize(ItemData itemData, Sprite sprite, int ownerId)
        {
            ItemData = itemData;
            OwnerId = ownerId;

            spriteRenderer.sprite = sprite;
        }

        public void SetPlacement(Vector2Int origin, bool flipped, Vector3 originWorldPosition)
        {
            const int MaxSortingOrder = 100;

            Origin = origin;
            Flipped = flipped;

            Vector2 offset = flipped ? new Vector2(ItemData.SpriteOffset.x * -1f, ItemData.SpriteOffset.y) : ItemData.SpriteOffset;
            transform.position = originWorldPosition + (Vector3)offset;
            spriteRenderer.flipX = flipped;
            spriteRenderer.sortingOrder = MaxSortingOrder - (Origin.x + Origin.y);
        }
    }
}
