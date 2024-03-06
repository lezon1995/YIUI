using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 部类 界面拆分数据
    /// </summary>
    public abstract partial class UIPanel
    {
        UIPanelSplitData splitData;
        Dictionary<string, UIView> viewTabsStatic = new Dictionary<string, UIView>();
        Dictionary<string, RectTransform> viewParents = new Dictionary<string, RectTransform>();

        /// <summary>
        /// 当前已打开的UI 不包含弹窗
        /// </summary>
        UIView curTabView;

        /// <summary>
        /// 外界可判断的当前打开的view名字
        /// </summary>
        public string CurrentOpenViewName
        {
            get
            {
                if (curTabView != null)
                {
                    if (curTabView.UIResName != null)
                    {
                        return curTabView.UIResName;
                    }

                    return "";
                }

                return "";
            }
        }

        HashSet<string> openingViews = new HashSet<string>();

        void InitPanelViewData()
        {
            viewTabsStatic.Clear();
            viewParents.Clear();
            splitData = Table.PanelSplitData;
            CreateStaticView();
            AddViewParent(splitData.ViewTabsStatic);
            AddViewParent(splitData.ViewTabs);
            AddViewParent(splitData.ViewPopups);
        }

        void AddViewParent(List<RectTransform> list)
        {
            foreach (var transform in list)
            {
                var viewName = transform.name.Replace(UIConst.ParentName, "");
                viewParents.Add(viewName, transform);
            }
        }

        void CreateStaticView()
        {
            foreach (var viewTab in splitData.ViewTabsStatic)
            {
                var viewName = viewTab.name.Replace(UIConst.ParentName, "");

                //通用view的名称是不允许修改的 如果修改了 那么就创建一个新的
                var transform = viewTab.FindChildByName(viewName);
                if (transform == null)
                {
                    Debug.LogError($"{viewName} 当前通用View 不存在于父级下 所以无法自动创建 将会动态创建");
                    continue;
                }

                //查看本地是否已经创建
                var view = Table.FindUIBase<UIBase>(viewName);

                //如果没有则通用重新创建
                if (view == null)
                {
                    view = UIFactory.CreateCommon(UIPkgName, viewName, transform.gameObject);
                }

                switch (view)
                {
                    case null:
                        continue;
                    case UIView uiView:
                        viewTabsStatic.Add(viewName, uiView);
                        break;
                    default:
                        Debug.LogError($"{viewName} 不应该存在的错误 当前创建的View 不是BaseView");
                        break;
                }
            }
        }

        RectTransform GetViewParent(string viewName)
        {
            if (viewParents.TryGetValue(viewName, out var transform))
            {
                return transform;
            }

            return null;
        }

        async UniTask<T> GetView<T>() where T : UIView, new()
        {
            var viewName = typeof(T).Name;
            var parent = GetViewParent(viewName);
            if (parent)
            {
                if (viewTabsStatic.TryGetValue(viewName, out var value))
                {
                    return value as T;
                }

                if (ViewIsOpening(viewName))
                {
                    Debug.LogError($"请检查 {viewName} 正在异步打开中 请勿重复调用 请检查代码是否一瞬间频繁调用");
                    return null;
                }

                AddOpening(viewName);
                var view = await UIFactory.InstantiateAsync<T>(parent);
                RemoveOpening(viewName);

                viewTabsStatic.Add(viewName, view);

                return view;
            }

            Debug.LogError($"不存在这个View  请检查 {viewName}");
            return null;
        }

        /// <summary>
        /// 打开之前
        /// </summary>
        async UniTask OpenViewBefore(UIView view)
        {
            if (!view.WindowFirstOpen)
            {
                await CloseLastView(view);
            }
        }

        /// <summary>
        /// 打开之后
        /// </summary>
        async UniTask OpenViewAfter(UIView view, bool success)
        {
            if (success)
            {
                if (view.WindowFirstOpen)
                {
                    await CloseLastView(view);
                }
            }
            else
            {
                view.Close(false);
            }
        }

        /// <summary>
        /// 关闭上一个
        /// </summary>
        /// <param name="view">当前</param>
        async UniTask CloseLastView(UIView view)
        {
            //其他需要被忽略
            if (view.ViewWindowType != EViewWindowType.View)
            {
                return;
            }

            //View只有切换没有关闭
            var skipTween = view.WindowSkipOtherCloseTween;

            if (curTabView != null && curTabView != view)
            {
                //View 没有自动回退功能  比如AView 关闭 自动吧上一个BView 给打开 没有这种需求 也不能有这个需求
                //只能有 打开一个新View 上一个View的自动处理 99% 都是吧上一个隐藏即可
                //外部就只需要关心 打开 A B C 即可
                //因为这是View  不是 Panel
                switch (curTabView.StackOption)
                {
                    case EViewStackOption.None:
                        break;
                    case EViewStackOption.Visible:
                        curTabView.SetActive(false);
                        break;
                    case EViewStackOption.VisibleTween:
                        await curTabView.CloseAsync(!skipTween);
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {curTabView.StackOption}");
                        curTabView.SetActive(false);
                        break;
                }
            }

            curTabView = view;
        }

        void AddOpening(string name)
        {
            openingViews.Add(name);
        }

        void RemoveOpening(string name)
        {
            openingViews.Remove(name);
        }

        bool ViewIsOpening(string name)
        {
            return openingViews.Contains(name);
        }
    }
}