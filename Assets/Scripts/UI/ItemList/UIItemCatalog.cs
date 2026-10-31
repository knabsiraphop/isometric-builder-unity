using UnityEngine;
using UnityEngine.Events;

namespace IsoBuilder.UI
{
    public class UIItemCatalog : MonoBehaviour
    {
        [SerializeField] private UIItemButton itemButtonPrefab;
        [SerializeField] private Transform itemContainer;

        public void AddItem(Sprite itemSprite, UnityAction onClickAction)
        {
            UIItemButton newItemButton = Instantiate(itemButtonPrefab, itemContainer);
            newItemButton.SetItem(itemSprite, onClickAction);
        }

        public void ClearItems()
        {
            foreach (Transform child in itemContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
