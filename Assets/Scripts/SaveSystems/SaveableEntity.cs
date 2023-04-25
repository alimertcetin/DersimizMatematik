using System;
using System.Collections.Generic;
using UnityEngine;
using XIV.Core;

namespace XIV.SaveSystems
{
    [DisallowMultipleComponent]
    public class SaveableEntity : MonoBehaviour
    {
        [DisplayWithoutEdit]
        [SerializeField] string id = string.Empty;
        public string Id => id;

        [ContextMenu("Generate Id")]
        private void GenerateId() => id = Guid.NewGuid().ToString();

        public object CaptureState()
        {
            var state = new Dictionary<string, object>();
            var saveables = GetComponents<ISaveable>();

            for (int i = 0; i < saveables.Length; i++)
            {
                ISaveable saveable = saveables[i];
                state.Add(saveable.GetType().ToString(), saveable.CaptureState());
            }

            return state;
        }

        public void RestoreState(object state)
        {
            var stateDictionary = (Dictionary<string, object>)state;
            var saveables = GetComponents<ISaveable>();

            for (int i = 0; i < saveables.Length; i++)
            {
                ISaveable saveable = saveables[i];
                string typeName = saveable.GetType().ToString();
                if (stateDictionary.TryGetValue(typeName, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }

    }
}