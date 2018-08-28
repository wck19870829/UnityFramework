using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedScarf.Framework.UGUI
{
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    /// <summary>
    /// UI面板，带有层级管理属性
    /// 
    /// 【排序规则】
    /// 1.默认使用相同UILayerKind不同优先以UILayerKind排序，UILayerKind相同以RelativeDepth排序，即权重LayerKind>RelativeDepth。
    /// 2.同一UILayerKind新打开面板覆盖于同一UILayerKind旧面板之上，即同一UILayerKind新覆盖旧
    /// </summary>
    public abstract class UIPanel : UIElement
    {
        const int LAYER_KIND_INTERVAL=1000;                             //LayerKind各个值之间的间隔
        static Dictionary<UILayerKind, List<UIPanel>> s_LayerDict;

        [SerializeField]protected UILayerKind m_LayerKind = UILayerKind.General;
        [SerializeField]protected int m_RelativeDepth;
        List<Canvas> canvasList;
        CanvasGroup m_CanvasGroup;
        GraphicRaycaster m_GraphicRaycaster;
        Canvas m_Canvas;
        UIPanelData m_Data;

        static UIPanel()
        {
            s_LayerDict = new Dictionary<UILayerKind, List<UIPanel>>();
        }

        protected override void Awake()
        {
            base.Awake();

            m_CanvasGroup = GetComponent<CanvasGroup>();
            if (m_CanvasGroup == null) m_CanvasGroup = gameObject.AddComponent<CanvasGroup>();
            m_Canvas = GetComponent<Canvas>();
            if (m_Canvas == null) m_Canvas = gameObject.AddComponent<Canvas>();
            m_GraphicRaycaster = GetComponent<GraphicRaycaster>();
            if (m_GraphicRaycaster == null) m_GraphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            LayerKind = m_LayerKind;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            RemoveFromLayerDict();
        }

        protected override bool SetData(UIElementData value)
        {
            if (!base.SetData(value)) return false;
            if (!typeof(UIPanelData).IsAssignableFrom(value.GetType()))
            {
                Debug.LogErrorFormat("Data类型必须继承自UIPanelData {0}",value);
                return false;
            }

            m_Data = value as UIPanelData;

            return true;
        }

        public UILayerKind LayerKind
        {
            get
            {
                return m_LayerKind;
            }
            set
            {
                RemoveFromLayerDict();
                if (!s_LayerDict.ContainsKey(value))
                {
                    s_LayerDict.Add(value, new List<UIPanel>());
                }
                s_LayerDict[value].Add(this);

                m_LayerKind = value;
                UIStage.Instance.SetDirty();
            }
        }

        void RemoveFromLayerDict()
        {
            if (s_LayerDict.ContainsKey(m_LayerKind))
            {
                var idx = s_LayerDict[m_LayerKind].IndexOf(this);
                if (idx >= 0)
                {
                    s_LayerDict[m_LayerKind].RemoveAt(idx);
                }
            }
        }

        /// <summary>
        /// 此LayerKind中的深度
        /// </summary>
        public int RelativeDepth
        {
            get
            {
                return m_RelativeDepth;
            }
            set
            {
                m_RelativeDepth = value;
                UIStage.Instance.SetDirty();
            }
        }

        public override UIElementData StateSnapshot()
        {
            if (m_Data != null)
            {
                var data = m_Data.DeepClone() as UIPanelData;
                data.depth = RelativeDepth;
                data.layerKind = LayerKind;
                return data;
            }

            return null;
        }

        internal void SortingLayer()
        {
            var canvasArray = GetComponentsInChildren<Canvas>(true);
            if (canvasList == null) canvasList = new List<Canvas>();
            else canvasList.Clear();
            foreach (var canvas in canvasArray)
            {
                if (canvas.GetComponentInParent<UILayer>() == this)
                {
                    canvasList.Add(canvas);
                }
            }
            canvasList.Sort((a, b) => {
                if (a.sortingOrder == b.sortingOrder) return 0;
                return a.sortingOrder > b.sortingOrder ? 1 : -1;
            });
            foreach (var canvas in canvasList)
            {
                canvas.sortingOrder = UIStage.globalDepth;
                UIStage.globalDepth++;
            }
        }

        #region 静态方法

        public static Dictionary<UILayerKind, List<UIPanel>> LayerDict
        {
            get
            {
                return s_LayerDict;
            }
        }

        #endregion
    }

    public abstract class UIPanelData : UIElementData
    {
        public UILayerKind layerKind;       //保存层级类型信息
        public int depth;                   //保存depth信息，不要轻易修改
    }
}
