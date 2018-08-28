using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedScarf.Framework.UGUI
{
    /// <summary>
    /// UI模块
    /// 不支持嵌套,根节点需为UIManager
    /// </summary>
    public abstract class UIModule : UIPanel
    {
        static Dictionary<string, UIModule> s_GuidDict;                            //guid映射

        UIModuleData m_Data;

        static UIModule()
        {
            s_GuidDict = new Dictionary<string, UIModule>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (transform.parent!= UIStage.Instance.transform)
            {
                transform.SetParent(UIStage.Instance.transform);
            }
        }

        protected override void Start()
        {
            base.Start();

            if (transform.parent.GetComponent<UIStage>() == null)
            {
                Debug.LogErrorFormat("UIModule父级需为UIStage {0}", this);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Unregister();
        }

        protected override bool SetData(UIElementData value)
        {
            if (!base.SetData(value)) return false;
            if (!typeof(UIModuleData).IsAssignableFrom(value.GetType()))
            {
                Debug.LogErrorFormat("Data类型必须继承自UIPanelData {0}", value);
                return false;
            }

            m_Data = value as UIModuleData;
            Unregister();
            Register();

            return true;
        }

        /// <summary>
        /// 返回下一步显示
        /// </summary>
        /// <param name="nextStepData"></param>
        public virtual void NextStep(UIModuleData nextStepData)
        {
            Close();
        }

        /// <summary>
        /// 返回上一步显示
        /// </summary>
        /// <param name="prevStepData"></param>
        public virtual void PrevStep(UIModuleData prevStepData)
        {
            Close();
        }

        /// <summary>
        /// 注册
        /// </summary>
        void Register()
        {
            if (m_Data == null || string.IsNullOrEmpty(m_Data.guid)) return;

            if (s_GuidDict.ContainsKey(m_Data.guid))
            {
                s_GuidDict.Remove(m_Data.guid);
            }
            s_GuidDict.Add(m_Data.guid, this);
        }

        /// <summary>
        /// 取消注册
        /// </summary>
        void Unregister()
        {
            if (m_Data == null || string.IsNullOrEmpty(m_Data.guid)) return;

            if (s_GuidDict.ContainsKey(m_Data.guid))
            {
                s_GuidDict.Remove(m_Data.guid);
            }
        }

        #region 静态方法

        /// <summary>
        /// 由guid搜索元素
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static UIModule FindByGUID(string guid)
        {
            if (s_GuidDict.ContainsKey(guid))
            {
                return s_GuidDict[guid];
            }

            return null;
        }

        public static Dictionary<string, UIModule> GuidDict
        {
            get
            {
                return s_GuidDict;
            }
        }

        #endregion
    }

    [UIBinding(typeof(UIModule))]
    public abstract class UIModuleData : UIPanelData
    {
        public string guid;

        public UIModuleData()
        {
            guid = Guid.NewGuid().ToString();
        }

        public UIModuleData(string guid)
        {
            this.guid = guid;
        }
    }
}