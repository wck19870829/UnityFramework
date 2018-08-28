using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.UGUI.Example
{
    public class UIExampleModule3 : UIModule
    {
        protected override void RefreshView()
        {

        }
    }

    [UIBinding(typeof(UIExampleModule3), "UIExample/UIExampleModule3")]
    public class UIExampleModuleData3 : UIModuleData
    {

    }
}