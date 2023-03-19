using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace XIVEditor.Utils
{
    public static class AssetUtils
    {
        /// <summary>Load .asset files via their base class</summary>
        /// <typeparam name="TAsset">Asset Type</typeparam>
        /// <returns>Dictionary that contains the types of assets as key and value as the list of objects</returns>
        public static Dictionary<Type, List<TAsset>> LoadAssets<TAsset>(string folderPath, SearchOption searchOption = SearchOption.AllDirectories) 
            where TAsset : UnityEngine.Object
        {
            Dictionary<Type, List<TAsset>> typeValuePair = new Dictionary<Type, List<TAsset>>();
            string[] assetPaths = Directory.GetFiles(folderPath, "*.asset", searchOption);
                
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
    }
}