using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace XIV.SaveSystem
{
    public static class Saver
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "14.Filo");

        public static void Save()
        {
            var state = LoadFile();
            CaptureState(state);
            SaveFile(state);
        }

        public static void Load()
        {
            var state = LoadFile();
            RestoreState(state);
        }

        private static void SaveFile(object state)
        {
            using (var stream = File.Open(SavePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private static Dictionary<string, object> LoadFile()
        {
            if (File.Exists(SavePath) == false) return new Dictionary<string, object>();

            using (FileStream stream = File.Open(SavePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }


        private static void CaptureState(Dictionary<string, object> state)
        {
            var saveables = Object.FindObjectsOfType<SaveableEntity>();
            foreach (var saveable in saveables)
            {
                var saveableEntityState = saveable.CaptureState();

                if (state.ContainsKey(saveable.Id)) state[saveable.Id] = saveableEntityState;
                else state.Add(saveable.Id, saveableEntityState);
            }
        }

        private static void RestoreState(Dictionary<string, object> state)
        {
            var saveables = Object.FindObjectsOfType<SaveableEntity>();
            foreach (var saveable in saveables)
            {
                if (state.TryGetValue(saveable.Id, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }
    }
}