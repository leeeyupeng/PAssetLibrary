using System;
using System.Collections.Generic;
using System.Text;

namespace AssetLibrary
{
    public class PAssetLoader
    {
        public string m_assetName;
        public List<PAsset> m_asset;

        public PAssetLoader(string assetName)
        {
            m_assetName = assetName;
        }

        public void LoadSuccess(string name)
        {
            foreach (PAsset asset in m_asset)
            {
                asset.LoadCallBack(name,true);
            }
        }

        public void LoadFail(string name)
        {
            foreach (PAsset asset in m_asset)
            {
                asset.LoadCallBack(name, false);
            }
        }
    }
}
