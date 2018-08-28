using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedScarf.Framework.UGUI.Example
{
    public class UIExampleModule : UIModule
    {
        public Button button;
        public Button button2;

        protected override void Awake()
        {
            base.Awake();

            if(button!=null) button.onClick.AddListener(OnButtonClick);
            if(button2!=null)button2.onClick.AddListener(OnButton2Click);
        }

        void OnButtonClick()
        {
            UIHistory.StepOne();

            var data = new UIExampleModuleData2();
            UIElement.GetInstanceElementByData(data,UIStage.Instance.transform);
        }

        void OnButton2Click()
        {
            UIHistory.StepOne();
            var data = new UIExampleModuleData3();
            UIElement.GetInstanceElementByData(data, UIStage.Instance.transform);
        }

        protected override void RefreshView()
        {

        }
    }

    [UIBinding(typeof(UIExampleModule), "UIExample/UIExampleModule")]
    public class UIExampleModuleData : UIModuleData
    {

    }
}