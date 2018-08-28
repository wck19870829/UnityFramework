using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.UGUI
{
    /// <summary>
    /// 加载器
    /// </summary>
    public class UILoader : MonoBehaviour
    {
        public UIElement Load(UIElementData data)
        {
            return UIElement.GetInstanceElementByData(data);
        }

        public void LoadAsync(UIElementData data,Action<UIElement>element)
        {

        }
    }
}