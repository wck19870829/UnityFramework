using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework
{
    /// <summary>
    /// MonoBehaviour单例模版
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T>: MonoBehaviour
        where T:MonoBehaviour
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
                        var instances = FindObjectsOfType<T>();
                        if (instances.Length > 0)
                        {
                            s_Instance = instances[0];
                            for (var i = 1; i < instances.Length; i++)
                            {
                                Destroy(instances[i].gameObject);
                            }
                        }
                        if (s_Instance == null)
                        {
                            var go = new GameObject("[" + typeof(T).Name + "]");
                            s_Instance = go.AddComponent<T>();
                            DontDestroyOnLoad(go);
                        }
                    }

                    return s_Instance;
                }
            }
        }
    }
}