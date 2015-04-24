using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AssetLibrary
{
    public class PAssetManager : MonoBehaviour
    {
        public static PAssetManager self;

        PAssetDownLoad m_downLoad = new PAssetDownLoad();
        //<string,Asset>
        static Hashtable m_tableLoaded = new Hashtable();
        static Hashtable m_tableLoading = new Hashtable();
        //sender type != MonoBehaviour
        Dictionary<object, PAsset> m_dicLoader = new Dictionary<object, PAsset>();

        void Awake()
        {
            self = this;
        }
        
        public void LoadAsset(PAsset asset, string assetName)
        {
            if (m_tableLoaded.Contains(assetName))
            {
            }
            else if (m_tableLoading.Contains(assetName))
            {
            }
            else
            {
                List<PAssetLoader> loaderList = new List<PAssetLoader>();
                loaderList.Add(new PAssetLoader(asset));

                m_tableLoading[assetName] = loaderList;

                m_downLoad.DownLoad(assetName,LoadCallBack);
            }
        }

        public void LoadCallBack(string assetName, bool success, AssetBundle bundle)
        {
            if (success)
            {
                m_tableLoaded[assetName] = bundle;

                LoadFinish(assetName);
            }
            else
            {
                LoadFailure(assetName);
            }
        }

        public void LoadFinish(string assetName)
        {
        }

        public void LoadFailure(string assetName)
        {
        }

        Dictionary<string, int> m_assetRefCount = new Dictionary<string, int>();
        Dictionary<string, uint> m_assetCache = new Dictionary<string, uint>();

        public void IncrAssetRef(string assetName)
        {
            if (m_assetRefCount.ContainsKey(assetName))
            {
                m_assetRefCount[assetName]++;
            }
            else
            {
                m_assetRefCount[assetName] = 1;
                if (m_assetCache.ContainsKey(assetName))
                {
                    m_assetCache.Remove(assetName);
                }
            }
        }

        public void DecrAssetRef(string assetName)
        {
            if (m_assetRefCount.ContainsKey(assetName))
            {
                m_assetRefCount[assetName]--;
                if (m_assetRefCount[assetName] == 0)
                {
                    m_assetRefCount.Remove(assetName);

                    m_assetCache[assetName] = TimeUtil.Now;
                }
            }
        }
    }
}
