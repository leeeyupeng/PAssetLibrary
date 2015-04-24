using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AssetLibrary
{
    class PAssetBehaviour : MonoBehaviour
    {
        PAsset m_asset;
        public PAsset Asset { get { return m_asset; } }

        void Awake()
        {
            m_asset = new PAsset();
        }

        void OnDestroy()
        {
            m_asset.Clear();
        }
    }
}
