using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrame
{
    public class AssetsManager : Singleton<AssetsManager>
    {
        //私有构造
        private AssetsManager()
        {
            assetsCache = new Dictionary<string, Object>();
        }
        /// <summary>
        /// 资源缓存
        /// </summary>
        private Dictionary<string, Object> assetsCache;
        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Object GetAsset(string path)
        {
            Object assetObj = null;
            //尝试从字典里获取路径对应的资源,有则返回true
            if(!assetsCache.TryGetValue(path, out assetObj))
            {
                assetObj = Resources.Load(path);
                assetsCache.Add(path, assetObj);
            }

            return assetObj;
        }
    }

}
