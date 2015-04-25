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
            string[] src_path_list = new string[] { "Resources"};

            Hashtable dependencyTable = new Hashtable();
            foreach(string src_path in src_path_list)
            {
                _BuildDependencyDirectory(src_path, ref dependencyTable);
            }

            _WriteDependency(dependencyTable);
        }

        [@MenuItem("AssetBundle/All/BuildAllAssetBundle")]
        public static void BuildAllAssetBundle()
        {
            string[] src_path_list = new string[] { "Resources", "Scene" };

            Hashtable dependencyTable = _ReadDependency();
            List<string> dependencyList = new List<string>();
            foreach(DictionaryEntry de in dependencyTable)
            {
                List<string> deValue = (List<string>)de.Value;
                foreach(string d in deValue)
                {
                    if(!dependencyList.Contains(d))
                    {
                        dependencyList.Add(d);
                    }
                }
            }

            BuildPipeline.PushAssetDependencies();

            foreach (string assetPath in dependencyList)
            {
                string srcPath = SrcPath(assetPath);
                string dstPath = DstPath(assetPath) + "_s.unity3d";

                UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(srcPath);
                BuildPipeline.BuildAssetBundle(obj, null, dstPath, GetAssetOptions(),GetAssetTarget());
            }

            List<string> assetList = GetAllFile(src_path_list);
            foreach (string assetPath in assetList)
            {
                BuildPipeline.PushAssetDependencies();

                string srcPath = SrcPath(assetPath);
                string dstPath = DstPath(assetPath) + ".unity3d";

                UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(srcPath);
                BuildPipeline.BuildAssetBundle(obj, null, dstPath, GetAssetOptions(), GetAssetTarget());

                BuildPipeline.PopAssetDependencies();

            }

            BuildPipeline.PopAssetDependencies();
        }

        static void _BuildDependencyDirectory(string src_path, ref Hashtable table)
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
                    _BuildDependencyAsset(assetPath, ref table);
                }
            }

            string[] directoryList = Directory.GetDirectories(src_path);
            foreach (string directory in directoryList)
            {
                _BuildDependencyDirectory(directory,ref table);
            }
        }

        static void _BuildDependencyAsset(string asset_path, ref Hashtable table)
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

        static List<string> GetAllFile(string[] pathList)
        {
            List<string> fileList = new List<string>();
            foreach (string path in pathList)
            {
                GetAllFileContainSub(path, ref fileList);
            }
            return fileList;
        }

        static List<string> GetAllFileContainSub(string path,ref List<string> fileList)
        {
            string[] files = Directory.GetFiles(path);
            foreach(string filePath in files)
            {
                if (!IsIgnoreByFileExtensions(filePath))
                {
                    if(!fileList.Contains(filePath))
                        fileList.Add(filePath);
                }
            }

            string[] directoryList = Directory.GetDirectories(path);
            foreach (string directory in directoryList)
            {
                fileList = GetAllFileContainSub(directory, ref fileList);
            }

            return fileList;
        }

        public static string SrcPath(string path)
        {
            return Application.dataPath + "Assets/" + path;
        }

        public static string DstPath(string path)
        {
            return Application.dataPath + "Assets/StreamingAssets/Assets_Win/" + path;
        }

        public static void _WriteDependency(Hashtable table)
        {
            FileStream stream = new FileStream(DstPath("asset_dependency.txt"), FileMode.Create, FileAccess.Write, FileShare.None);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream,table);
            stream.Close();
        }

        public static Hashtable _ReadDependency()
        {
            FileStream stream = new FileStream(DstPath("asset_dependency.txt"), FileMode.Open, FileAccess.Read, FileShare.None);
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

        public static BuildAssetBundleOptions GetAssetOptions()
        {
            BuildAssetBundleOptions ao = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
            return ao;
        }

        public static BuildTarget GetAssetTarget()
        {
            return BuildTarget.StandaloneWindows;
        }
    }
}
