using System;
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

        const int TEMP_RENDER_TEXTURE_MAX_SIZE = 2048;
        static RenderTexture cacheRT;
        static RenderTexture tempRT;

        /// <summary>
        /// 开始在renderTexture上绘制
        /// </summary>
        static void BeginDrawOnRenderTexture()
        {
            GL.PushMatrix();
            GL.LoadPixelMatrix(0,TEMP_RENDER_TEXTURE_MAX_SIZE, TEMP_RENDER_TEXTURE_MAX_SIZE,0);

            cacheRT = RenderTexture.active;
            tempRT = RenderTexture.GetTemporary(TEMP_RENDER_TEXTURE_MAX_SIZE, TEMP_RENDER_TEXTURE_MAX_SIZE);
            RenderTexture.active = tempRT;
            GL.Clear(true, true, Color.clear);
        }

        /// <summary>
        /// 结束绘制
        /// </summary>
        static void EndDrawOnRenderTexture()
        {
            RenderTexture.ReleaseTemporary(tempRT);
            RenderTexture.active = cacheRT;
            GL.PopMatrix();
        }

        /// <summary>
        /// Texture转Texture2D
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Texture2D Tex2T2d(Texture source)
        {
            var t2d = new Texture2D(source.width,source.height);
            if(!Graphics.ConvertTexture(source, t2d))
            {
                //转换失败尝试其他方法,DX9与Mac+OpenGL不支持
                throw new Exception("转换失败！暂未实现方法");
            }

            return t2d;
        }

        /// <summary>
        /// 一维索引转二维索引
        /// </summary>
        /// <param name="index">一维索引</param>
        /// <param name="rowCount">行元素数量</param>
        /// <returns></returns>
        public static Tuple<int,int> Dimension1To2(int index,int rowCount)
        {
            var rowIdx = index % rowCount;
            var columnIdx = index / rowCount;
            var coord = new Tuple<int, int>(rowIdx, columnIdx);

            return coord;
        }

        /// <summary>
        /// 二维索引转一维索引
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="rowCount">行元素数量</param>
        /// <returns></returns>
        public static int Dimension2To1(int rowIndex,int columnIndex, int rowCount)
        {
            var index = columnIndex*rowCount+rowIndex;

            return index;
        }

        /// <summary>
        /// 通过复制RenderTexture像素拷贝
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Texture2D CopyTextureFromRenderTexture(Texture2D source, Rect rect)
        {
            if (rect.width > TEMP_RENDER_TEXTURE_MAX_SIZE || rect.height > TEMP_RENDER_TEXTURE_MAX_SIZE)
            {
                Debug.LogErrorFormat("源图片尺寸需小于{0}", TEMP_RENDER_TEXTURE_MAX_SIZE);
                return null;
            }

            if (source != null)
            {
                var sourceRect = new Rect(0, 0, source.width, source.height);
                var overlapRect = GetRectOverlap(sourceRect, rect);
                if (overlapRect != Rect.zero)
                {
                    var width = (int)(overlapRect.xMax - overlapRect.xMin);
                    var height = (int)(overlapRect.yMax - overlapRect.yMin);
                    if (width > 0 && height > 0)
                    {
                        BeginDrawOnRenderTexture();

                        //绘制源图片到RenderTexture
                        var screenRect = new Rect(0,0,width,height);
                        var drawSourceRect = new Rect(
                                            overlapRect.x/ sourceRect.width,
                                            1-(overlapRect.y+ overlapRect.height)/sourceRect.height,
                                            overlapRect.width/ sourceRect.width,
                                            overlapRect.height/ sourceRect.height
                                            );
                        Graphics.DrawTexture(screenRect, source, drawSourceRect, 0,0,0,0);

                        //读取像素
                        var tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
                        tex.ReadPixels(screenRect, 0, 0);
                        tex.Apply();

                        EndDrawOnRenderTexture();

                        return tex;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 拷贝图片
        /// 源图片需为IsReadable可读写
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Texture2D CopyTexture(Texture2D source, Rect rect)
        {
            if (source != null)
            {
                var sourceRect = new Rect(0, 0, source.width, source.height);
                rect = new Rect(rect.x,sourceRect.height-rect.y-rect.height,rect.width,rect.height);
                var overlapRect = GetRectOverlap(sourceRect, rect);
                if (overlapRect != Rect.zero)
                {
                    var width = (int)(overlapRect.xMax - overlapRect.xMin);
                    var height = (int)(overlapRect.yMax - overlapRect.yMin);
                    if (width > 0 && height > 0)
                    {
                        var tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
                        var widthOffset = (int)overlapRect.xMin;
                        var heightOffset = (int)overlapRect.yMin;
                        var sourceColors = source.GetPixels32();
                        var sourceWidth = source.width;
                        var texLen = width * height;
                        var texColors = new Color32[texLen];
                        for (var i = 0; i < texLen; i++)
                        {
                            var coord = Dimension1To2(i, width);
                            var sourceIdx = Dimension2To1(coord.Item1 + widthOffset, coord.Item2 + heightOffset, sourceWidth);
                            texColors[i] = sourceColors[sourceIdx];
                        }
                        tex.SetPixels32(texColors);
                        tex.Apply();

                        return tex;
                    }
                }
            }

            return null;
        }

        #endregion

        #region 动画

        /// <summary>
        /// 获取动画时长
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="clip"></param>
        /// <returns></returns>
        public static float GetClipLength(Animator animator,string clip)
        {
            if (animator==null || string.IsNullOrEmpty(clip) || animator.runtimeAnimatorController==null)
                return 0;

            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            AnimationClip[] tAnimationClips = ac.animationClips;
            if (tAnimationClips==null || tAnimationClips.Length <= 0) return 0;

            AnimationClip tAnimationClip;
            for (int tCounter = 0; tCounter < tAnimationClips.Length; tCounter++)
            {
                tAnimationClip = ac.animationClips[tCounter];
                if (tAnimationClip!=null && tAnimationClip.name == clip)
                {
                    return tAnimationClip.length;
                }
            }
            return 0;
        }

        #endregion
    }
}