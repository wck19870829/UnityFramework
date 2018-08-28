using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.UGUI
{
    [RequireComponent(typeof(Canvas))]
    /// <summary>
    /// UI舞台根目录
    /// UIStage
    ///     |_UIMudule
    ///         |_UIPanel
    ///             |_UIElement
    /// </summary>
    public sealed class UIStage :Singleton<UIStage>
    {
        Canvas m_MainCanvas;

        public static event Action OnSortLayerFinished;             //层级排序完成回调

        private void Awake()
        {
            if (m_MainCanvas==null)
            {
                m_MainCanvas = gameObject.GetComponent<Canvas>();
                if (m_MainCanvas==null)
                {
                    m_MainCanvas = gameObject.AddComponent<Canvas>();
                    m_MainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }
            }
        }

        private void Update()
        {
            if (UIPanel.isDirty)
            {
                foreach (var item in UIPanel.LayerDict)
                {
                    UIPanel.globalDepth = (int)item.Key;

                    item.Value.Sort((a, b) => {
                        if (a.RelativeDepth == b.RelativeDepth) return 0;
                        return a.RelativeDepth > b.RelativeDepth ? 1 : -1;
                    });
                    foreach (var layer in item.Value)
                    {
                        layer.SortingLayer();
                    }
                }

                UIPanel.isDirty = false;

                if (OnSortLayerFinished != null)
                {
                    OnSortLayerFinished.Invoke();
                }
            }
        }

        /// <summary>
        /// 主面板
        /// </summary>
        public Canvas MainCanvas
        {
            get
            {
                if (m_MainCanvas == null)
                {
                    m_MainCanvas = GetComponent<Canvas>();
                }
                if (m_MainCanvas == null)
                {
                    m_MainCanvas = gameObject.AddComponent<Canvas>();
                }

                return m_MainCanvas;
            }
        }
    }
}