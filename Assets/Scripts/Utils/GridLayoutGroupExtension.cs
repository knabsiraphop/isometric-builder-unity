using UnityEngine;
using UnityEngine.UI;

namespace IsoBuilder.Utils
{
    public static class GridLayoutGroupExtension
    {
        public static void SetCellSize(this GridLayoutGroup gridLayoutGroup, RectTransform rectTransform, int columnCount,int rowCount, int padding, int space)
        {
            float cellWidth = (rectTransform.rect.width - (2 * padding) - ((columnCount - 1) * space)) / columnCount;
            float cellHeight = (rectTransform.rect.height - (2 * padding) - ((rowCount - 1) * space)) / rowCount;
            gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);
            gridLayoutGroup.padding = new RectOffset(padding, padding, padding, padding);
        }
    }
}
