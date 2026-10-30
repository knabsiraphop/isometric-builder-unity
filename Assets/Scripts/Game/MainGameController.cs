using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using IsoBuilder.Gameplay;
using IsoBuilder.UI;
using UnityEngine;

namespace IsoBuilder
{
    public class MainGameController : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private MapController mapController;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private GameConfig gameConfig;

        private ItemCatalog itemCatalog;

        private async UniTaskVoid Start()
        {
            itemCatalog = new ItemCatalog();
            itemCatalog.Initialize(gameConfig.ItemConfig);


            await mapController.Initialize(gameConfig.GridSystemConfig);

            cameraController.Initialize();
            cameraController.FitToBounds(mapController.GetMapBounds());

            await TestSpawn();

            uiManager.SetActiveLoadingScreen(false);
        }

        private async UniTask TestSpawn()
        {
            ItemSpriteLoader itemSpriteLoader = new ItemSpriteLoader();

            if (!itemCatalog.TryGetItemData(5, out ItemData itemData))
            {
                return;
            }
            Sprite sprite = await itemSpriteLoader.GetSprite(itemData);
            GameObject gameObject = new GameObject($"Item_{itemData.Id}");
            gameObject.AddComponent<SpriteRenderer>().sprite = sprite;
            gameObject.transform.position = mapController.CellToWorld(new Vector2Int(0, 0));
        }
    }
}
