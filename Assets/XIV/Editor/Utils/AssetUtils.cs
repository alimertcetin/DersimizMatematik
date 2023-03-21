using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XIVEditor.Utils
{
    public static class AssetUtils
    {
        /// <summary>Load .asset files via their base class</summary>
        /// <typeparam name="TAsset">Asset Type</typeparam>
        /// <returns>Dictionary that contains the types of assets as key and value as the list of objects</returns>
        public static Dictionary<Type, List<TAsset>> LoadAssetsOfType<TAsset>(string folderPath, SearchOption searchOption = SearchOption.AllDirectories) 
            where TAsset : Object
        {
            Dictionary<Type, List<TAsset>> typeValuePair = new Dictionary<Type, List<TAsset>>();
            string[] assetPaths = Directory.GetFiles(folderPath, "*", searchOption);
            
            for (int i = 0; i < assetPaths.Length; i++)
            {
                TAsset asset = AssetDatabase.LoadAssetAtPath<TAsset>(assetPaths[i]);
                if (asset == null) continue;
                            
                var assetType = asset.GetType();
                if (typeValuePair.ContainsKey(assetType))
                {
                    typeValuePair[assetType].Add(asset);
                }
                else
                {
                    List<TAsset> list = new List<TAsset>();
                    list.Add(asset);
                    typeValuePair.Add(assetType, list);
                }
            }

            return typeValuePair;
        }
        
        public static void OpenInspectorForAsset(Object asset)
        {
            Type inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
            EditorWindow inspectorWindow = (EditorWindow)ScriptableObject.CreateInstance(inspectorType);
            MethodInfo targetMethod = inspectorType.GetMethod("SetObjectsLocked", BindingFlags.NonPublic | BindingFlags.Instance);
            targetMethod.Invoke(inspectorWindow, new object[] { new List<Object>() { asset } });
            inspectorWindow.Show();
        }

        public static void SelectAsset(Object asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            Selection.activeObject = AssetDatabase.LoadAssetAtPath(path, asset.GetType());
        }

        public static void HighlightAsset(Object asset)
        {
            EditorGUIUtility.PingObject(asset);
        }
        
    }
}