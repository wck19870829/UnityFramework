using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.UGUI
{
    /// <summary>
    /// UI历史记录
    /// </summary>
    public class UIHistory
    {
        static int s_StepIndex;
        static List<List<UIModuleData>> stepDataList;

        static UIHistory()
        {
            stepDataList = new List<List<UIModuleData>>();
        }

        /// <summary>
        /// 保存一步快照
        /// 只保存激活的UIModule
        /// </summary>
        public static void StepOne()
        {
            if (UIModule.GuidDict.Values.Count == 0) return;

            var activeList = new List<UIModuleData>(UIModule.GuidDict.Values.Count);
            foreach (var value in UIModule.GuidDict.Values)
            {
                if (value.isOpen)
                {
                    var dataSnapshot = value.StateSnapshot() as UIModuleData;
                    if (dataSnapshot != null)
                    {
                        activeList.Add(dataSnapshot);
                    }
                }
            }

            stepDataList.RemoveRange(s_StepIndex, stepDataList.Count - s_StepIndex);
            stepDataList.Add(activeList);
        }

        /// <summary>
        /// 下一步数据
        /// </summary>
        /// <returns></returns>
        public static void NextStep()
        {
            StepIndex++;
        }

        /// <summary>
        /// 上一步数据
        /// </summary>
        /// <returns></returns>
        public static void PrevStep()
        {
            StepIndex--;
        }

        /// <summary>
        /// 解析
        /// </summary>
        static void Resolve()
        {
            if (stepDataList.Count == 0) return;

            var currentStep=stepDataList[s_StepIndex];
            foreach (var data in currentStep)
            {
                var module = UIModule.FindByGUID(data.guid);
                if (module==null)
                {
                    //已经销毁，创建新的
                    module = (UIModule)UIModule.GetInstanceElementByData(data,UIStage.Instance.transform);
                    module.Data = data;
                }
                module.LayerKind = data.layerKind;
                module.cacheDepth = data.cacheDepth;
            }
            UIStage.Instance.SetDirty();
        }

        /// <summary>
        /// 当前历史记录索引
        /// </summary>
        public static int StepIndex
        {
            get
            {
                return s_StepIndex;
            }
            set
            {
                s_StepIndex = value;
                s_StepIndex = Mathf.Clamp(s_StepIndex, 0, stepDataList.Count - 1);
                Resolve();
            }
        }
    }
}
