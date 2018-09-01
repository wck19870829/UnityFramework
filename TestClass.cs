using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedScarf.Framework
{
    public class TestClass : MonoBehaviour
    {
        public Texture2D sourceTex;
        public RawImage source;
        public RawImage image;
        public Rect rect;

        private void Update()
        {
            var t2d = new Texture2D(source.texture.width, source.texture.height,TextureFormat.ARGB32,false);
            Graphics.ConvertTexture(source.texture, t2d);
            image.texture = Tools.CopyTextureFromRenderTexture(sourceTex, rect);
        }
    }
}