#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    public sealed partial class UIPanelSplitData
    {
        internal bool ShowCreatePanelViewEnum()
        {
            if (ViewTabsStatic == null || ViewTabs == null)
            {
                return false;
            }

            return ViewTabsStatic.Count + ViewTabs.Count >= 1;
        }

        bool ShowCheckBtn()
        {
            if (Panel == null)
            {
                return false;
            }

            return Panel.name.EndsWith(UIConst.Source);
        }

        [GUIColor(0, 1, 1)]
        [Button("检查拆分数据", 30)]
        [ShowIf(nameof(ShowCheckBtn))]
        void AutoCheckBtn()
        {
            AutoCheck();
        }

        internal bool AutoCheck()
        {
            if (!ResetParent()) return false;
            if (!CheckPanelName()) return false;

            CheckViewName(ViewTabsStatic);
            CheckViewName(ViewTabs);
            CheckViewName(ViewPopups);

            CheckViewParent(ViewTabsStatic, ViewTabsParent);
            CheckViewParent(ViewTabs, ViewTabsParent);
            CheckViewParent(ViewPopups, ViewPopupsParent);

            var hashList = new HashSet<RectTransform>();
            CheckRepetition(ref hashList, ViewTabsStatic);
            CheckRepetition(ref hashList, ViewTabs);
            CheckRepetition(ref hashList, ViewPopups);

            return true;
        }

        bool ResetParent()
        {
            if (Panel == null)
            {
                Debug.LogError($"没有找到 Panel");
                return false;
            }

            string viewParentName = UIConst.ViewTabsName;
            if (ViewTabsParent == null || ViewTabsParent.name != viewParentName)
            {
                ViewTabsParent = Panel.transform.FindChildByName(viewParentName).GetComponent<RectTransform>();
            }

            if (ViewTabsParent == null)
            {
                Debug.LogError($"没有找到 {Panel.name} {viewParentName}  这是必须存在的组件 你可以不用 但是不能没有");
                return false;
            }

            var popupViewParentName = UIConst.ViewPopupsName;
            if (ViewPopupsParent == null || ViewPopupsParent.name != popupViewParentName)
            {
                ViewPopupsParent = Panel.transform.FindChildByName(popupViewParentName).GetComponent<RectTransform>();
            }

            if (ViewPopupsParent == null)
            {
                Debug.LogError($"没有找到 {Panel.name} {popupViewParentName}  这是必须存在的组件 你可以不用 但是不能没有");
                return false;
            }

            return true;
        }

        bool CheckPanelName()
        {
            var qualifiedName = NameUtility.ToFirstUpper(Panel.name);
            if (Panel.name != qualifiedName)
            {
                Panel.name = qualifiedName;
            }

            var panelSourceName = UIConst.PanelSourceName;
            if (Panel.name == UIConst.PanelSourceName)
            {
                Debug.LogError($"当前是默认名称 请手动修改名称 Xxx{panelSourceName}");
                return false;
            }

            if (Panel.name.EndsWith($"{panelSourceName}"))
            {
                return true;
            }

            Debug.LogError($"{Panel.name} 命名必须以 {panelSourceName} 结尾 请勿随意修改");
            return false;
        }

        //命名检查
        static void CheckViewName(IList<RectTransform> list)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var current = list[i];
                if (current == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                var qualifiedName = NameUtility.ToFirstUpper(current.name);
                if (current.name != qualifiedName)
                {
                    current.name = qualifiedName;
                }

                var viewParentName = UIConst.ViewParentName;
                if (current.name == UIConst.YIUIViewParentName)
                {
                    Debug.LogError($"当前是默认名称 请手动修改名称 Xxx{viewParentName}");
                    list.RemoveAt(i);
                    continue;
                }

                if (!current.name.EndsWith(viewParentName))
                {
                    Debug.LogError($"{current.name} 命名必须以 {viewParentName} 结尾 请勿随意修改");
                    list.RemoveAt(i);
                    continue;
                }

                var viewName = current.name.Replace(UIConst.ParentName, "");
                var viewTable = current.GetComponentInChildren<UITable>();

                if (viewTable == null)
                {
                    //如果这个子物体被隐藏了
                    if (current.transform.childCount >= 1)
                    {
                        var firstChild = current.transform.GetChild(0);
                        viewTable = firstChild.GetComponent<UITable>();
                    }
                }

                if (viewTable == null)
                {
                    Debug.LogError($" {current.name} 父物体下必须有View  但是未找到View 请使用 右键 YIUI/Create UIView 创建符合要求的结构");
                    list.RemoveAt(i);
                    continue;
                }

                viewTable.gameObject.name = viewName;
            }
        }

        //检查null / 父级
        void CheckViewParent(IList<RectTransform> list, Object parent)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                var current = list[i];

                var parentP = current.parent;
                if (parentP == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                if (parentP != parent)
                {
                    list.RemoveAt(i);

                    //因为只有2个父级 所以如果不是这个就会自动帮你移动到另外一个上面
                    //如果多了还是不要自动了
                    switch (parentP.name)
                    {
                        case UIConst.ViewTabsName:
                            ViewTabs.Add(current);
                            break;
                        case UIConst.ViewPopupsName:
                            ViewPopups.Add(current);
                            break;
                    }
                }
            }
        }

        //检查重复
        void CheckRepetition(ref HashSet<RectTransform> hashList, IList<RectTransform> list)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var current = list[i];
                if (hashList.Contains(current))
                {
                    list.RemoveAt(i);
                    Debug.LogError($"{Panel.name} / {current.name} 重复存在 已移除 请检查");
                }
                else
                {
                    hashList.Add(current);
                }
            }
        }
    }
}
#endif