using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace UIFrame
{
    /// <summary>
    /// �������� ��Ҫ��ɵ�����ֻ��̳в���˽�й��켴��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class
    {
        //��������
        private static T _singleton;
        //��ȡ����
        public static T Instance
        {
            get
            {
                if (_singleton == null)
                {
                    //����T��Ҫ��public���캯��
                    //_singleton = new T();
                    //ͨ������ʵ�������� �����ĵ������б���Ҫ��˽�е��޲ι���
                    _singleton = (T)Activator.CreateInstance(typeof(T), nonPublic: true);
                }
                return _singleton;
            }
        }
    }

}
