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
        public Inventory Inventory { get; private set; }
        public IBuildController BuildController { get; private set; }

        public GameContext(GameConfig config, ISpriteLoader spriteLoader, Pooling pooling, IBuildController buildController)
        {
            Config = config;

            SaveDataStore = new SaveDataStore(config);
            SaveDataStore.LoadData();

            PlaceableCatalog = new PlaceableCatalog(config.PlaceableConfig, config.StationConfig);

            InventoryItemCatalog = new InventoryItemCatalog(config.InventoryItemMasterData);
            Inventory = new(SaveDataStore);

            SpriteLoader = spriteLoader;
            Pooling = pooling;

            BuildController = buildController;
        }
    }
}
