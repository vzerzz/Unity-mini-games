using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace UIFrame
{
    /// <summary>
    /// 单例基类 需要变成单例的只需继承并且私有构造即可
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class
    {
        //单例对象
        private static T _singleton;
        //获取单例
        public static T Instance
        {
            get
            {
                if (_singleton == null)
                {
                    //这样T必要有public构造函数
                    //_singleton = new T();
                    //通过反射实例化对象 派生的单例类中必须要有私有的无参构造
                    _singleton = (T)Activator.CreateInstance(typeof(T), nonPublic: true);
                }
                return _singleton;
            }
        }
    }

}
