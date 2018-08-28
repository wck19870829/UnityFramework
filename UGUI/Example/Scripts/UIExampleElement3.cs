using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedScarf.Framework.UGUI.Example
{
    public class UIExampleElement3 : UIElement
    {
        public RawImage icon;

        protected override void RefreshView()
        {
            var data = Data as UIExampleElementData3;
            icon.transform.localEulerAngles = new Vector3(0,0,data.angle);
        }
    }

    [UIBinding(typeof(UIExampleElement3), "UIExample/UIExampleElement3")]
    public class UIExampleElementData3 : UIElementData
    {
        public float angle;

        public UIExampleElementData3(float angle)
        {
            this.angle = angle;
        }
    }
}
