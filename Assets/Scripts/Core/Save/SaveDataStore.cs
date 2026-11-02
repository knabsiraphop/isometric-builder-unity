namespace IsoBuilder.Core
{
    public class SaveDataStore
    {
        private readonly LocalSave<SaveData> localSave;

        public SaveData SaveData { get; private set; }

        public SaveDataStore()
        {
            localSave = new LocalSave<SaveData>();
        }

        public void LoadData()
        {
            SaveData = localSave.Load();
            if (SaveData == null)
            {
                SaveData = new SaveData();
            }
        }

        public void Save()
        {
            localSave.Save(SaveData);
        }
    }
}
