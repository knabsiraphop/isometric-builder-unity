using IsoBuilder.Utils;

namespace IsoBuilder.Core
{
    public class GameContext
    {
        public GameConfig Config { get; private set; }
        public SaveDataStore SaveDataStore { get; private set; }
        public PlaceableCatalog PlaceableCatalog { get; private set; }
        public InventoryItemCatalog InventoryItemCatalog { get; private set; }
        public ISpriteLoader SpriteLoader { get; private set; }
        public Pooling Pooling { get; private set; }

        public GameContext(GameConfig config, ISpriteLoader spriteLoader, Pooling pooling)
        {
            Config = config;

            SaveDataStore = new SaveDataStore(config);
            SaveDataStore.LoadData();

            PlaceableCatalog = new PlaceableCatalog(config.PlaceableConfig, config.StationConfig);

            InventoryItemCatalog = new InventoryItemCatalog(config.InventoryItemMasterData);

            SpriteLoader = spriteLoader;
            Pooling = pooling;
        }
    }
}
