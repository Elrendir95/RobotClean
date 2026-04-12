using System;
using System.IO;
using UnityEngine;

namespace Components.SaveSystem
{
    public static class SaveService
    {
        private const string SaveDataFileName = "SaveData.dat";
        private static string FilePath => Path.Combine(Application.persistentDataPath, SaveDataFileName);

        public static void Save(SaveData saveData)
        {
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(FilePath, json);
        }

        public static SaveData Load()
        {
            if (!File.Exists(FilePath)) return new SaveData();

            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonUtility.FromJson<SaveData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load " + SaveDataFileName + ": " + e.Message);
            }
            return new SaveData();
        }
    }
}
