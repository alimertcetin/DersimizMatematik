using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XIV.XIVEditor.Utils;
using XIV.InventorySystem.ScriptableObjects.NonSerializedData;

namespace XIV.InventorySystem.XIVEditor
{
    [CustomEditor(typeof(NonSerializedItemDataContainerSO))]
    public class NonSerializedItemDataContainerSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            NonSerializedItemDataContainerSO container = (NonSerializedItemDataContainerSO)target;

            if (GUILayout.Button("Load Data Containers"))
            {
                Dictionary<Type, List<NonSerializedItemDataSO>> dataContainers = AssetUtils.LoadAssetsOfType<NonSerializedItemDataSO>("Assets/ScriptableObjects");
                List<NonSerializedItemDataSO> itemDatas = new List<NonSerializedItemDataSO>(dataContainers.Count);
                foreach (KeyValuePair<Type, List<NonSerializedItemDataSO>> dataContainer in dataContainers)
                {
                    itemDatas.AddRange(dataContainer.Value);
                }
                
                Undo.RecordObject(container, "Load Data Containers");
                container.itemDataPairs = new NonSerializedItemDataSO[itemDatas.Count];
                for (int i = 0; i < itemDatas.Count; i++)
                {
                    container.itemDataPairs[i] = itemDatas[i];
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}