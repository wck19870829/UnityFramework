using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework.Kinect
{
    /// <summary>
    /// 用户监听
    /// </summary>
    public sealed class KinectUserListener : Singleton<KinectUserListener>
    {
        HashSet<long> userSet;
        List<long> removeList;

        public event Action<long> OnUserIn;       //用户进入
        public event Action<long> OnUserOut;      //用户离开

        private void Awake()
        {
            userSet = new HashSet<long>();
            removeList = new List<long>();
        }

        private void Update()
        {
            var allUsers=KinectManager.Instance.GetAllUserIds();
            removeList.Clear();
            foreach (var user in userSet)
            {
                if (allUsers.IndexOf(user)<0)
                {
                    Debug.LogFormat("用户离开{0}", user);

                    removeList.Add(user);
                    if (OnUserOut!=null)
                    {
                        OnUserOut.Invoke(user);
                    }
                }
            }
            foreach (var user in removeList)
            {
                userSet.Remove(user);
            }

            foreach (var user in allUsers)
            {
                if (!userSet.Contains(user))
                {
                    Debug.LogFormat("用户进入{0}", user);

                    userSet.Add(user);
                    if (OnUserIn != null)
                    {
                        OnUserIn.Invoke(user);
                    }
                }
            }
        }
    }
}