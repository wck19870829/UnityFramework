using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RedScarf.Framework.UGUI
{
    /// <summary>
    /// 虚拟鼠标发射器
    /// </summary>
    public class VirstualMouseEmitter : MonoBehaviour
    {
        static float cdSelectTime = 1;
        static Canvas root;

        public Image progressImage;
        public Camera targetCam;
        public Transform target;
        public Camera uiCam;

        protected Button topButton;
        protected PointerEventData cachePeData;
        protected RaycastResult cacheResult;
        protected HashSet<Button> receiverSet;
        protected List<RaycastResult> resultList;
        protected float cd;

        protected virtual void Start()
        {
            resultList = new List<RaycastResult>();
            receiverSet = new HashSet<Button>();
            if (root == null)
            {
                var canvasArr = GameObject.FindObjectsOfType<Canvas>();
                foreach (var canvas in canvasArr)
                {
                    if (canvas.isRootCanvas)
                    {
                        root = canvas;
                        break;
                    }
                }
            }
            var graphicsArr = GetComponentsInChildren<Graphic>(true);
            foreach (var graphics in graphicsArr)
            {
                graphics.raycastTarget = false;
            }
        }

        protected virtual void Update()
        {
            HitCheck();
        }

        protected virtual void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }

        protected void HitCheck()
        {
            if (root == null) return;

            cachePeData = new PointerEventData(EventSystem.current);
            cachePeData.position = RectTransformUtility.WorldToScreenPoint(root.worldCamera, transform.position);
            EventSystem.current.RaycastAll(cachePeData, resultList);
            topButton = null;
            if (resultList.Count > 0)
            {
                cacheResult = resultList[0];
                topButton = cacheResult.gameObject.GetComponent<Button>();
                if (topButton != null)
                {
                    if (receiverSet.Contains(topButton))
                    {
                        //Stay
                    }
                    else
                    {
                        //Enter
                        receiverSet.Add(topButton);
                        topButton.OnPointerEnter(cachePeData);
                    }
                }

                foreach (var receiver in receiverSet)
                {
                    if (topButton != receiver)
                    {
                        //Exit
                        receiver.OnPointerExit(cachePeData);
                    }
                }
                receiverSet.RemoveWhere((x) => {
                    return (topButton == x) ? false : true;
                });
            }

            if (progressImage != null)
            {
                progressImage.fillAmount = cd / cdSelectTime;
            }

            if (topButton!=null)
            {
                cd += Time.deltaTime;
                if (cd> cdSelectTime)
                {
                    cd = 0;
                    if (topButton.onClick!=null)
                    {
                        topButton.onClick.Invoke();
                    }
                }
            }
            else
            {
                cd = 0;
            }

            if (targetCam != null && target != null)
            {
                var screenPos=RectTransformUtility.WorldToScreenPoint(targetCam, target.position);
                var worldPos = Vector3.zero;
                if (uiCam != null)
                {
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, screenPos, uiCam, out worldPos);
                }
                transform.position = worldPos;
            }
        }

        protected virtual void OnDisable()
        {
            foreach (var receiver in receiverSet)
            {
                receiver.OnPointerExit(cachePeData);
            }
            receiverSet.Clear();
        }
    }
}