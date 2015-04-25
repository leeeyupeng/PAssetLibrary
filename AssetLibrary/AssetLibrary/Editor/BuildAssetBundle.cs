using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEditor;
using UnityEngine;


namespace AssetLibrary.Editor
{
    class BuildAssetBundle
    {
        [@MenuItem("AssetBundle/All/BuildDependencyDirectoryList")]
        public static void BuildDependencyDirectoryList()
        {
            string[] src_path_list = new string[] { "Resources","Scene"};

            Hashtable dependencyTable = new Hashtable();
            foreach(string src_path in src_path_list)
            {
                _BuildDependencyDirectory(src_path, dependencyTable);
            }

            _WriteDependency(dependencyTable);
        }

        static void _BuildDependencyDirectory(string src_path, Hashtable table)
        {
            if (!Directory.Exists(src_path))
            {
                Debug.LogError("error");
            }
            string[] assetList = Directory.GetFiles(src_path);
            foreach(string assetPath in assetList)
            {
                if (!IsIgnoreByFileExtensions(assetPath))
                {
                    _BuildDependencyAsset(assetPath, table);
                }
            }

            string[] directoryList = Directory.GetDirectories(src_path);
            foreach (string directory in directoryList)
            {
                _BuildDependencyDirectory(directory,table);
            }
        }

        static void _BuildDependencyAsset(string asset_path, Hashtable table)
        {
            string[] dependList = AssetDatabase.GetDependencies(new string[] {asset_path});
            List<string> dependRealList = new List<string>();
            foreach (string depend in dependList)
            {
                if (IsShareAsset(depend))
                {
                    dependRealList.Add(depend);
                }
                else
                {
                    //ignore
                }
            }

            if (dependRealList.Count > 0)
            {
                table[asset_path] = dependRealList;
            }
        }

        static bool IsIgnoreByFileExtensions(string asset_path)
        {
            string[] extensionList = new string[] { ".meta"};
            foreach (string extension in extensionList)
            {
                if (asset_path.EndsWith(extension))
                {
                    return true;
                }
            }
            return false;
        }

        static bool IsShareAsset(string assetPath)
        {
            string[] extensionList = new string[] { ".fbx",".mat"};
            foreach (string extension in extensionList)
            {
                if (assetPath.EndsWith(extension))
                {
                    return true;
                }
            }
            return false;
        }

        static string FullPath(string path)
        {
            return Application.dataPath + "Assets/" + path;
        }

        static void _WriteDependency(Hashtable table)
        {
            FileStream stream = new FileStream("Assets/StreamingAssets/asset_dependency.txt", FileMode.Create, FileAccess.Write, FileShare.None);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream,table);
            stream.Close();
        }

        static Hashtable _ReadDependency()
        {
            FileStream stream = new FileStream("Assets/StreamingAssets/asset_dependency.txt", FileMode.Open, FileAccess.Read, FileShare.None);
            if (stream != null)
            {
                Hashtable table;
                BinaryFormatter bf = new BinaryFormatter();
                table = (Hashtable)bf.Deserialize(stream);
                stream.Close();
                return table;
            }
            return null;
        }
    }
}
