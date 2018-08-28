using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedScarf.Framework
{
    public class TestClass : MonoBehaviour
    {
        public Texture2D source;
        public RawImage image;
        public Rect rect;

        private void Update()
        {
            image.texture = Tools.CopyTexture(source, rect);
        }
    }
}