using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AssetLibrary
{
    class PAsset : MonoBehaviour
    {
        public static void Load(object sender, string assetName, Action<string,bool> action)
        {
            PAsset asset = GetAsset(sender);
            asset.Load(assetName, action);
        }

        public static void UnLoad(object sender, string assetName)
        {

        }

        public static void Clear(object sender)
        {

        }

        static PAsset GetAsset(object sender)
        {
            if (sender.GetType() == typeof(MonoBehaviour))
            {
                MonoBehaviour behaviour = (MonoBehaviour)sender;
                PAssetBehaviour assetBehaviour = behaviour.GetComponent<PAssetBehaviour>();
                if (assetBehaviour == null)
                {
                    assetBehaviour = behaviour.gameObject.AddComponent<PAssetBehaviour>();
                }

                return assetBehaviour.Asset;
            }
            else
            {
            }
            return null;
        }

        public void Load(string assetName, Action<string,bool> action)
        {
            PAssetManager.self.LoadAsset(this,assetName,action);
        }

        public void UnLoad();

        public void Clear();
    }
}
