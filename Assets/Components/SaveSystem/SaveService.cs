using System;
using System.IO;
using UnityEngine;

namespace Components.SaveSystem
{
    public static class SaveService
    {
        private const string SaveDataFileName = "SaveData.dat";
        private static string FilePath => Path.Combine(Application.persistentDataPath, SaveDataFileName);

        /// <summary>
        /// Save the data into the persistentDataPath
        /// </summary>
        /// <param name="saveData"></param>
        public static void Save(SaveData saveData)
        {
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(FilePath, json);
        }

        /// <summary>
        /// Load the data from the persistentDataPath
        /// return a new one if nothing is found
        /// </summary>
        /// <param name="saveData"></param>
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
