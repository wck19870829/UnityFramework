using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedScarf.Framework
{
    /// <summary>
    /// 常用工具
    /// </summary>
    public static class Tools
    {
        #region 方法

        /// <summary>
        /// 销毁子元素
        /// </summary>
        /// <param name="go"></param>
        public static void DestroyChildren(GameObject go)
        {
            for (var i = go.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(go.transform.GetChild(i).gameObject);
            }
        }

        #endregion

        #region 空间坐标

        /// <summary>
        /// 获取RectTransform屏幕矩形
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static Rect GetScreenRect(RectTransform trans, Camera cam)
        {
            var corners = new Vector3[4];
            trans.GetWorldCorners(corners);
            for (var i = 0; i < corners.Length; i++)
            {
                corners[i] = cam.WorldToScreenPoint(corners[i]);
            }
            var xMin = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
            var yMin = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
            var xMax = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
            var yMax = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
            var screenRrect = Rect.MinMaxRect(xMin, yMin, xMax, yMax);

            return screenRrect;
        }

        /// <summary>
        /// 获取两个矩形相交区域
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        public static Rect GetRectOverlap(Rect rect,Rect rect2)
        {
            if (rect.Overlaps(rect2, true))
            {
                return Rect.MinMaxRect( Mathf.Max(rect.xMin,rect2.xMin),
                                        Mathf.Max(rect.yMin, rect2.yMin),
                                        Mathf.Min(rect.xMax, rect2.xMax),
                                        Mathf.Min(rect.yMax, rect2.yMax)
                                        );
            }

            return Rect.zero;
        }

        #endregion

        #region 图形图像

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Texture2D CopyTexture(Texture2D source, Rect rect)
        {
            if (source == null) return null;

            var sourceRect = new Rect(0,0,source.width,source.height);
            var overlapRect = GetRectOverlap(sourceRect, rect);
            if (overlapRect!=Rect.zero)
            {
                var width = (int)(overlapRect.xMax - overlapRect.xMin);
                var height = (int)(overlapRect.yMax - overlapRect.yMin);
                var tex = new Texture2D(width, height);
                var widthOffset = (int)overlapRect.xMin;
                var heightOffset = (int)overlapRect.yMin;
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < height; j++)
                    {
                        tex.SetPixel(i, j, source.GetPixel(i+widthOffset, j+heightOffset));
                    }
                }
                tex.Apply();

                return tex;
            }

            return null;
        }

        #endregion
    }
}