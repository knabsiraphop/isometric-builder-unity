namespace IsoBuilder.Core
{
    public class SaveDataStore
    {
        private readonly LocalSave<SaveData> localSave;

        private GameConfig gameConfig;

        public SaveData SaveData { get; private set; }

        public SaveDataStore(GameConfig gameConfig)
        {
            this.gameConfig = gameConfig;
            localSave = new LocalSave<SaveData>();
        }

        public void LoadData()
        {
            if (!localSave.TryLoad(out SaveData saveData))
            {
                saveData.ApplySaveData(gameConfig);
                SaveData = saveData;
            }
        }

        public void Save()
        {
            localSave.Save(SaveData);
        }
    }
}
