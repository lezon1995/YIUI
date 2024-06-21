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

                //通用创建 这个时候通用UI一定是没有创建的 否则就有问题
                var view = UIFactory.CreateCommon(UIPkgName, viewName, viewTab.gameObject);
                if (view == null) continue;
                viewTab.gameObject.SetActive(false);

                switch (view)
                {
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

        async UniTask<T> GetView<T>() where T : UIView
        {
            var viewName = typeof(T).Name;
            var parent = GetViewParent(viewName);
            if (parent)
            {
                if (viewTabsStatic.TryGetValue(viewName, out var value))
                {
                    return value as T;
                }

                using var asyncLock = await AsyncLockMgr.Inst.Wait(viewName.GetHashCode());

                var view = await UIFactory.InstantiateAsync<T>(parent);

                viewTabsStatic.Add(viewName, view);

                return view;
            }

            Debug.LogError($"不存在这个View  请检查 {viewName}");
            return null;
        }

        public (bool, UIView) ExistView<T>() where T : UIView
        {
            if (!UIBindHelper.TryGetBindVo<T>(out var vo))
                return (false, null);

            var viewName = vo.ResName;
            var viewParent = GetViewParent(viewName);
            if (viewParent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {viewName}");
                return (false, null);
            }

            if (viewTabsStatic.TryGetValue(viewName, out var baseView))
            {
                return (true, baseView);
            }

            return (false, null);
        }

        public (bool, UIView) ExistView(string viewName)
        {
            var viewParent = GetViewParent(viewName);
            if (viewParent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {viewName}");
                return (false, null);
            }

            if (viewTabsStatic.TryGetValue(viewName, out var baseView))
            {
                return (true, baseView);
            }

            return (false, null);
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

        public void CloseView<T>(bool tween = true) where T : UIView
        {
            CloseViewAsync<T>(tween).Forget();
        }

        public void CloseView(string resName, bool tween = true)
        {
            CloseViewAsync(resName, tween).Forget();
        }

        public async UniTask<bool> CloseViewAsync<TView>(bool tween = true) where TView : UIView
        {
            var (exist, entity) = ExistView<TView>();
            if (!exist) return false;
            return await CloseViewAsync(entity, tween);
        }

        public async UniTask<bool> CloseViewAsync(string resName, bool tween = true)
        {
            var (exist, entity) = ExistView(resName);
            if (!exist) return false;
            return await CloseViewAsync(entity, tween);
        }

        private async UniTask<bool> CloseViewAsync(UIView view, bool tween)
        {
            if (view == null) return false;
            await view.CloseAsync(tween);
            return true;
        }
    }
}