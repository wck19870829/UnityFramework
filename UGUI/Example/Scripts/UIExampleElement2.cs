using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.UGUI.Example
{
    public class UIExampleElement2 : UIExampleElement
    {
        public CanvasGroup canvasGroup;

        protected override void RefreshView()
        {
            base.RefreshView();

            var data = Data as UIExampleElementData2;
            canvasGroup.alpha = data.alpha;
        }
    }
    [UIBinding(typeof(UIExampleElement2), "UIExample/UIExampleElement2")]
    public class UIExampleElementData2 : UIExampleElementData
    {
        public float alpha;

        public UIExampleElementData2(Color color,float alpha)
            :base(color)
        {
            this.alpha = alpha;
        }
    }
}