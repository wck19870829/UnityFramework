using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.UGUI.Example
{
    public class UIExamplePanel : UIPanel
    {
        public UIElementContainer container;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                var dataList = new List<UIElementData>()
                {
                    new UIExampleElementData(Color.red),
                    new UIExampleElementData2(Color.yellow,0.5f),
                    new UIExampleElementData3(30)
                };
                var data = new UIExamplePanelData(dataList);
                Data = data;
            }
        }

        protected override void RefreshView()
        {
            var data = Data as UIExamplePanelData;
            container.Create(data.itemDataList);
        }
    }

    [UIBinding(typeof(UIExamplePanel))]
    public class UIExamplePanelData : UIPanelData
    {
        public List<UIElementData> itemDataList;

        public UIExamplePanelData(List<UIElementData> itemDataList)
        {
            this.itemDataList = itemDataList;
        }
    }
}