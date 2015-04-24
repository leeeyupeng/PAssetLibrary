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
        
        public void LoadAsset(PAsset asset, string assetName,Action<string,bool> action)
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
                loaderList.Add(new PAssetLoader(asset, action));

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
    }
}
