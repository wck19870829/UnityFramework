using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.UGUI.Example
{
    public class UIExamplePanel2 : UIExamplePanel
    {

    }

    [UIBinding(typeof(UIExamplePanel2))]
    public class UIExamplePanelData2 : UIExamplePanelData
    {
        public UIExamplePanelData2(List<UIElementData> itemDataList)
            :base(itemDataList)
        {

        }
    }
}