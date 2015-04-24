using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetLibrary
{
    public class PAssetLoader
    {
        public PAsset m_asset;
        public Action<string,bool> m_action;

        public PAssetLoader(PAsset asset, Action<string,bool> action)
        {
            m_asset = asset;
            m_action = action;
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
