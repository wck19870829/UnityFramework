using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RedScarf.Framework.UGUI
{
    /// <summary>
    /// 虚拟鼠标接收按钮
    /// </summary>
    public class VirstualMouseReceiverButton : Button
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            DoStateTransition(SelectionState.Highlighted, true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            DoStateTransition(SelectionState.Normal,true);
        }
    }
}