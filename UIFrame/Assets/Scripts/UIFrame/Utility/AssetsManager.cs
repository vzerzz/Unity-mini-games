using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrame
{
    public class AssetsManager : Singleton<AssetsManager>
    {
        //˽�й���
        private AssetsManager()
        {
            assetsCache = new Dictionary<string, Object>();
        }
        /// <summary>
        /// ��Դ����
        /// </summary>
        private Dictionary<string, Object> assetsCache;
        /// <summary>
        /// ��ȡ��Դ
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Object GetAsset(string path)
        {
            Object assetObj = null;
            //���Դ��ֵ����ȡ·����Ӧ����Դ,���򷵻�true
            if(!assetsCache.TryGetValue(path, out assetObj))
            {
                assetObj = Resources.Load(path);
                assetsCache.Add(path, assetObj);
            }

            return assetObj;
        }
    }

}
