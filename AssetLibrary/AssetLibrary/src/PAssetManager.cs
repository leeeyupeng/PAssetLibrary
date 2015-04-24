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

        public PiAssetDownLoad m_downLoad;
        //<string,Asset>
        static Hashtable m_tableLoaded = new Hashtable();
        static Hashtable m_tableLoading = new Hashtable();
        //sender type != MonoBehaviour
        Dictionary<object, PAsset> m_dicLoader = new Dictionary<object, PAsset>();

        void Awake()
        {
            self = this;
        }
        
        public AssetBundle GetAsset(string assetName)
        {
            if (m_tableLoaded.ContainsKey(assetName))
            {
                return (AssetBundle)m_tableLoaded[assetName];
            }

            Debug.LogError("AssetLibrary error");
            return null;
        }

        public void LoadAsset(PAsset asset, string assetName)
        {
            if (m_tableLoaded.Contains(assetName))
            {
                LoadFinish(assetName);
            }
            else if (m_tableLoading.Contains(assetName))
            {
                List<PAssetLoader> listLoader = (List<PAssetLoader>)m_tableLoading[assetName];
                listLoader.Add(new PAssetLoader(asset));
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
            List<PAssetLoader> listLoader = (List<PAssetLoader>)m_tableLoading[assetName];
            foreach (PAssetLoader loader in listLoader)
            {
                loader.LoadSuccess(assetName);
            }
            listLoader.Clear();
            m_tableLoading.Remove(assetName);
        }

        public void LoadFailure(string assetName)
        {
            List<PAssetLoader> listLoader = (List<PAssetLoader>)m_tableLoading[assetName];
            foreach (PAssetLoader loader in listLoader)
            {
                loader.LoadFail(assetName);
            }
            listLoader.Clear();
            m_tableLoading.Remove(assetName);
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
