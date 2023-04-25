using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace XIV.SaveSystems
{
    public static class SaveSystem
    {
        static string CurrentScene => SceneManager.GetActiveScene().name;
        static string saveDirectory => Path.Combine(Application.persistentDataPath, CurrentScene);
        static string saveFile => CurrentScene + ".sav";
        static string SavePath => Path.Combine(saveDirectory, saveFile);

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

        public static IEnumerator SaveAsync()
        {
            var state = LoadFile();
            yield return CaptureStateAsync(state);
            SaveFile(state);
        }

        public static IEnumerator LoadAsync()
        {
            var state = LoadFile();
            yield return RestoreStateAsync(state);
        }

        static void SaveFile(object state)
        {
            if (Directory.Exists(saveDirectory) == false) Directory.CreateDirectory(saveDirectory);
            using (var stream = File.Open(SavePath, FileMode.Create, FileAccess.Write))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        static Dictionary<string, object> LoadFile()
        {
            if (File.Exists(SavePath) == false) return new Dictionary<string, object>();

            using (FileStream stream = File.Open(SavePath, FileMode.Open, FileAccess.Read))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        static void CaptureState(Dictionary<string, object> state)
        {
            var saveables = Object.FindObjectsOfType<SaveableEntity>();
            foreach (var saveable in saveables)
            {
                var saveableEntityState = saveable.CaptureState();

                if (state.ContainsKey(saveable.Id)) state[saveable.Id] = saveableEntityState;
                else state.Add(saveable.Id, saveableEntityState);
            }
        }


        static IEnumerator CaptureStateAsync(Dictionary<string, object> state)
        {
            var saveables = Object.FindObjectsOfType<SaveableEntity>();
            foreach (var saveable in saveables)
            {
                yield return null;
                var saveableEntityState = saveable.CaptureState();

                if (state.ContainsKey(saveable.Id)) state[saveable.Id] = saveableEntityState;
                else state.Add(saveable.Id, saveableEntityState);
            }
        }

        static void RestoreState(Dictionary<string, object> state)
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

        static IEnumerator RestoreStateAsync(Dictionary<string, object> state)
        {
            var saveables = Object.FindObjectsOfType<SaveableEntity>();
            foreach (var saveable in saveables)
            {
                yield return null;
                if (state.TryGetValue(saveable.Id, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }

        public static bool IsSaveExists() => File.Exists(SavePath);
    }
}