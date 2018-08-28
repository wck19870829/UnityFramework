using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RedScarf.Framework.UGUI
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    /// <summary>
    /// UIElementData绑定UIElement
    /// </summary>
    public class UIBindingAttribute : Attribute
    {
        public string prefabPath;
        public Type uiType;

        public UIBindingAttribute(Type uiType,string prefabPath="")
        {
            this.uiType = uiType;
            this.prefabPath = prefabPath;
        }
    }
}