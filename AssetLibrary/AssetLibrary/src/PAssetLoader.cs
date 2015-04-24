using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetLibrary
{
    public class PAssetLoader
    {
        public PAsset m_asset;
        public Action<string,bool> m_action;

        public PAssetLoader(PAsset asset)
        {
            m_asset = asset;
            m_action = asset.LoadCallBack;
        }

        public void LoadSuccess(string name)
        {
            if (m_action != null)
            {
                m_action(name,true);
            }
        }

        public void LoadFail(string name)
        {
            if (m_action != null)
            {
                m_action(name, false);
            }
        }
    }
}
