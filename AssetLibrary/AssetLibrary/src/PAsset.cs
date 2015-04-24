using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace AssetLibrary
{
    public class PAsset : MonoBehaviour
    {
        #region interface 
        public static void Load(object sender, string assetName, Action<string,bool> action)
        {
            PAsset asset = GetAsset(sender);
            asset.Load(assetName, action);
        }

        public static void UnLoad(object sender, string assetName)
        {
            PAsset asset = GetAsset(sender);
            asset.UnLoad(assetName);
        }

        public static void Clear(object sender)
        {
            PAsset asset = GetAsset(sender);
            asset.Clear();
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

        #endregion

        Dictionary<string, Action<string, bool>> m_assetAction = new Dictionary<string,Action<string,bool>>();
        List<string> m_assetUsing = new List<string>();

        public void Load(string assetName, Action<string,bool> action)
        {
            //liyupeng waiting 验证
            if (m_assetAction.ContainsKey(assetName))
            {
                m_assetAction[assetName] += action;
            }
            else
            {
                m_assetAction[assetName] = new Action<string,bool>(action);
            }
            
            PAssetManager.self.LoadAsset(this,assetName);
        }

        public void UnLoad(string assetName)
        {
            if (m_assetUsing.Contains(assetName))
            {
                PAssetManager.self.DecrAssetRef(assetName);
                m_assetUsing.Remove(assetName);
            }
        }

        public void Clear()
        {
            foreach (string assetName in m_assetUsing)
            {
                PAssetManager.self.DecrAssetRef(assetName);
            }

            m_assetUsing.Clear();
        }

        public void LoadCallBack(string assetName, bool success)
        {
            if (success)
            {
                if (!m_assetUsing.Contains(assetName))
                {
                    PAssetManager.self.IncrAssetRef(assetName);
                    m_assetUsing.Add(assetName);
                }
            }
            else
            {
                Debug.Log("load error");
            }

            if (m_assetAction.ContainsKey(assetName))
            {
                m_assetAction[assetName](assetName, success);
            }
        }
    }
}
