using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework
{
    /// <summary>
    /// 单例模版
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonObject<T>
        where T : class,new()
    {
        static volatile T s_Instance;
        static object syncRoot = new object();

        public static T Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (s_Instance == null)
                    {
                        s_Instance = new T();
                    }

                    return s_Instance;
                }
            }
        }
    }
}
