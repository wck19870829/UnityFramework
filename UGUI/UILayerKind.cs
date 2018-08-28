using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.UGUI
{
    /// <summary>
    /// UI层类型
    /// 可以再此增加，不要轻易删除
    /// </summary>
    public enum UILayerKind
    {
        General      = 1100000,             //一般UI
        Level2       = 1200000,             //2-N级界面
        Level3       = 1300000,
        Level4       = 1400000,
        Level5       = 1500000,
        Overlay      = 6000000,             //顶层
        System       = 9000000              //系统
    }
}