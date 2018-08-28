using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.UGUI.Example
{
    public class UIExampleInit : MonoBehaviour
    {
        private void Start()
        {
            var data = new UIExampleModuleData();
            UIElement.GetInstanceElementByData(data);

        }
    }
}