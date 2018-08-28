using RedScarf.Framework.UGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedScarf.Framework.UGUI.Example
{
    public class UIExampleElement : UIElement
    {
        public RawImage icon;

        protected override void RefreshView()
        {
            var data = Data as UIExampleElementData;
            icon.color = data.color;
        }
    }

    [UIBinding(typeof(UIExampleElement),"UIExample/UIExampleElement")]
    public class UIExampleElementData : UIElementData
    {
        public Color color;

        public UIExampleElementData(Color color)
        {
            this.color = color;
        }
    }
}