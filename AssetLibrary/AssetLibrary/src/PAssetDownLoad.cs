using System;
using System.Collections;
using System.Linq;
using System.Text;

using UnityEngine;

namespace AssetLibrary
{
    public interface PiAssetDownLoad
    {
        void DownLoad(string assetName, Action<string, bool, AssetBundle> action);
    }

    public class PAssetDownLoad : MonoBehaviour,PiAssetDownLoad
    {
        void Awake()
        {
            PAssetManager.self.m_downLoad = this;
        }

        public virtual void DownLoad(string assetName, Action<string,bool, AssetBundle> action)
        {
        }

        public void DownLoadFromServer(string assetName, Action<string, bool, AssetBundle> action)
        {
        }

        public void DownLoadFromLocal(string assetName, Action<string, bool, AssetBundle> action)
        {
            string url = assetName;
            StartCoroutine(DownLoadFromUrl(assetName,url,action));
        }

        public void DownLoadFromCache(string assetName, Action<string, bool, AssetBundle> action)
        {
        }

        IEnumerator DownLoadFromUrl(string assetName,string url, Action<string, bool, AssetBundle> action)
        {
            WWW assetW = new WWW(url);
            yield return assetW;

            if (string.IsNullOrEmpty(assetW.error))
            {
                action(assetName, false, null);
            }
            else if (assetW.isDone)
            {
                action(assetName,true,assetW.assetBundle);
            }
        }
    }
}
