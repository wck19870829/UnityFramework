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

        string m_GUID;

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
            if (string.IsNullOrEmpty(GUID)) return;

            if (s_GuidDict.ContainsKey(GUID))
            {
                s_GuidDict.Remove(GUID);
            }
            s_GuidDict.Add(GUID, this);
        }

        /// <summary>
        /// 取消注册
        /// </summary>
        void Unregister()
        {
            if (string.IsNullOrEmpty(GUID)) return;

            if (s_GuidDict.ContainsKey(GUID))
            {
                s_GuidDict.Remove(GUID);
            }
        }

        /// <summary>
        /// GUID
        /// </summary>
        public string GUID
        {
            get
            {
                if (string.IsNullOrEmpty(m_GUID))
                {
                    m_GUID = System.Guid.NewGuid().ToString();
                }
                return m_GUID;
            }
            internal set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Debug.LogErrorFormat("GUID不能为null!  {0}",value);
                    return;
                }

                m_GUID = value;
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