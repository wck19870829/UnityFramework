using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedScarf.Framework
{
    public static class ObjectExtends
    {
        static void Transform(this Transform transform)
        {
            transform.GetInstanceID();
            Debug.Log("Transform...");
        }
    }
}