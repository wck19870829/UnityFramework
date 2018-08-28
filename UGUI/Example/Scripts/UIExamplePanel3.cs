using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.UGUI.Example
{
    public class UIExamplePanel3 : UIPanel
    {
        protected override void RefreshView()
        {

        }
    }

    [UIBinding(typeof(UIExamplePanel3))]
    public class UIExamplePanelData3 : UIPanelData
    {

    }
}