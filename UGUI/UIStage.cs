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
        internal static int globalDepth;
        bool isDirty;
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
            if (isDirty)
            {
                foreach (var item in UIPanel.LayerDict)
                {
                    globalDepth = (int)item.Key;

                    item.Value.Sort((a, b) => {
                        if (a.cacheDepth == b.cacheDepth) return 0;

                        return a.cacheDepth > b.cacheDepth ? 1 : -1;
                    });
                    foreach (var layer in item.Value)
                    {
                        layer.SortingLayer();
                    }
                }

                isDirty = false;

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

        public void SetDirty()
        {
            isDirty = true;
        }
    }
}