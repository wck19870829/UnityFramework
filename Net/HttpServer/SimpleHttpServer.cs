using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RedScarf.Framework.Net.HttpServer
{
    /// <summary>
    /// Http服务器简单实现
    /// </summary>
    public sealed class SimpleHttpServer : Singleton<SimpleHttpServer>
    {
        static readonly int defaultPort = 8848;
        static readonly string defaultFileRootPath = "D:/SimpleHttpServer";

        [SerializeField] string m_FileRootPath;                               //文件根路径
        HttpListener m_HttpListener;
        string m_LocalIP;
        List<Func<HttpListenerContext, bool>> m_GetMessageChecker;

        public SimpleHttpServer()
        {
            m_HttpListener = new HttpListener();
        }

        private void OnDestroy()
        {
            Stop();
            Close();
        }

        /// <summary>
        /// 使用默认值初始化
        /// </summary>
        public void Init()
        {
            var checker = new List<Func<HttpListenerContext, bool>>()
            {
                CheckerFile
            };
            var prefixes = new string[]
            {
                "http://+:"+defaultPort+"/"
            };
            Init(checker, defaultFileRootPath, prefixes);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="getMessageChecker">获取信息检测序列,用于按顺序检测</param>
        /// <param name="prefixes">监听ip地址列表</param>
        /// <param name="rootPath">根文件夹</param>
        public void Init(List<Func<HttpListenerContext, bool>> getMessageChecker, string fileRootPath, string[] prefixes)
        {
            if (getMessageChecker == null || getMessageChecker.Count == 0)
            {
                Debug.LogErrorFormat("获取信息方法检测不为空且数量大于0");
                return;
            }

            m_GetMessageChecker = getMessageChecker;
            m_HttpListener.Prefixes.Clear();
            foreach (var p in prefixes)
            {
                m_HttpListener.Prefixes.Add(p);
            }
            m_LocalIP = NetTools.GetLocalIP();

            m_FileRootPath = fileRootPath;
            if (!Directory.Exists(fileRootPath))
            {
                try
                {
                    Directory.CreateDirectory(fileRootPath);
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("创建文件夹错误:{0}", e);
                }
            }

            Debug.LogFormat("HttpServer初始化.");
        }

        /// <summary>
        /// 根文件路径
        /// </summary>
        public string FileRootPath { get { return m_FileRootPath; }}

        /// <summary>
        /// 本机地址
        /// </summary>
        public string LocalIP { get { return m_LocalIP; } }

        /// <summary>
        /// 开始服务器
        /// </summary>
        public void StartListener()
        {
            try
            {
                m_HttpListener.Start();
                Debug.LogFormat("HttpServer开始监听.");
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("HttpServer开始监听错误:{0}", e);
                return;
            }

            Listenering();
        }

        /// <summary>
        /// 监听链接传入
        /// </summary>
        async void Listenering()
        {
            if (!Application.isPlaying) return;

            HttpListenerContext context = null;

            try
            {
                context = await m_HttpListener.GetContextAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Listenering();

                try
                {
                    if (m_GetMessageChecker != null)
                    {
                        for (var i = 0; i < m_GetMessageChecker.Count; i++)
                        {
                            if (m_GetMessageChecker[i].Invoke(context)==true)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    //Debug.LogErrorFormat("检测错误：{0}", e);
                }
            }
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bytes"></param>
        public async void SendAsync(HttpListenerContext context, byte[] bytes)
        {
            await Task.Run(() =>
            {
                try
                {
                    context.Response.Close(bytes, true);
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("HttpServer发送错误:{0}", e);
                }
            });
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            try
            {
                m_HttpListener.Stop();
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("HttpServer服务停止错误:{0}", e);
            }
            finally
            {
                Debug.LogFormat("HttpServer服务停止");
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            try
            {
                m_HttpListener.Close();
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("HttpServer服务关闭错误:{0}", e);
            }
            finally
            {
                Debug.LogFormat("HttpServer服务关闭");
            }
        }

        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool CheckerFile(HttpListenerContext context)
        {
            Debug.Log("xxx");

            if (!string.IsNullOrEmpty(context.Request.RawUrl))
            {
                var filePath = SimpleHttpServer.Instance.FileRootPath + context.Request.RawUrl;
                if (File.Exists(filePath))
                {
                    var bytes = File.ReadAllBytes(filePath);
                    SimpleHttpServer.Instance.SendAsync(context, bytes);

                    return true;
                }
            }

            return false;
        }
    }
}