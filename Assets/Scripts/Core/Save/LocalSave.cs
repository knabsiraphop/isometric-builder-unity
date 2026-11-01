using System;
using System.IO;
using UnityEngine;

namespace IsoBuilder.Core
{
    public class LocalSave<T> where T : new()
    {
        private readonly string saveFilePath;
        private readonly string tempFilePath;

        public LocalSave()
        {
            saveFilePath = Path.Combine(Application.persistentDataPath, $"{typeof(T).Name}.json");
            tempFilePath = $"{saveFilePath}.tmp";

            Debug.Log($"saveFilePath: {saveFilePath}");
            Debug.Log($"tempFilePath: {tempFilePath}");
        }

        public void Save(T dataToSave)
        {
            try
            {
                string json = JsonUtility.ToJson(dataToSave, true);

                File.WriteAllText(tempFilePath, json);

                if (File.Exists(saveFilePath))
                {
                    File.Replace(tempFilePath, saveFilePath, null);
                }
                else
                {
                    File.Move(tempFilePath, saveFilePath);
                }

                Debug.Log("Game data saved successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving data: {e.Message}");
            }
        }

        public T Load()
        {
            if (!File.Exists(saveFilePath))
            {
                Debug.Log("No save file found. Creating new data.");
                return new T();
            }

            try
            {
                string json = File.ReadAllText(saveFilePath);
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading data: {e.Message}");
                return new T();
            }
        }
    }
}
