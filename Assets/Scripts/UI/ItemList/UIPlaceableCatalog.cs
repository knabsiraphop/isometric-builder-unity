using UnityEngine;
using UnityEngine.Events;

namespace IsoBuilder.UI
{
    public class UIPlaceableCatalog : MonoBehaviour
    {
        [SerializeField] private UIPlaceableButton buttonPrefab;
        [SerializeField] private Transform container;

        public void AddButton(Sprite sprite, UnityAction onClickAction)
        {
            UIPlaceableButton button = Instantiate(buttonPrefab, container);
            button.SetButton(sprite, onClickAction);
        }

        public void ClearAllButton()
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
