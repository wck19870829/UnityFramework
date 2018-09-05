using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System;
using System.Threading;
using System.IO;

namespace RedScarf.Framework.Net.HttpServer.Example
{
    public class SimpleHttpServerExample : MonoBehaviour
    {
        private void OnGUI()
        {
            if (GUILayout.Button("开始监听"))
            {
                SimpleHttpServer.Instance.Init();
                SimpleHttpServer.Instance.StartListener();
            }
        }
    }
}