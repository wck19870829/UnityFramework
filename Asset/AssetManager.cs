using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.Asset
{
    /// <summary>
    /// 资源管理器
    /// 包含加载、卸载资源等方法
    /// </summary>
    public class AssetManager : MonoBehaviour
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T Load<T>(string path)where T:Object
        {
            var asset = default(T);

            return asset;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        public void LoadAsync<T>(string path)where T : Object
        {

        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="obj"></param>
        public void Unload(Object obj)
        {

        }
    }
}
