using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    //打开泛型 异步
    public abstract partial class BasePanel
    {
        #region 异步通过泛型打开（泛型类型参数）

        protected async UniTask<T> OpenViewAsync<T>() where T : BaseView, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open();
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<T> OpenViewAsync<T, P1>(P1 p1) where T : BaseView, IYIUIOpen<P1>, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<T> OpenViewAsync<T, P1, P2>(P1 p1, P2 p2) where T : BaseView, IYIUIOpen<P1, P2>, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<T> OpenViewAsync<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : BaseView, IYIUIOpen<P1, P2, P3>, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<T> OpenViewAsync<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) where T : BaseView, IYIUIOpen<P1, P2, P3, P4>, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3, p4);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<T> OpenViewAsync<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) where T : BaseView, IYIUIOpen<P1, P2, P3, P4, P5>, new()
        {
            var view = await GetView<T>();
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3, p4, p5);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        #endregion

        #region 异步通过Name打开（object类型参数）

        async UniTask<BaseView> GetView(string viewName)
        {
            BaseView view;
            var parent = GetViewParent(viewName);
            if (parent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {viewName}");
                return null;
            }

            if (m_ExistView.TryGetValue(viewName, out view))
            {
                return view;
            }

            var success = UIBindHelper.TryGetBindVoByPath(UIPkgName, viewName, out var vo);
            if (success == false)
            {
                return null;
            }

            if (ViewIsOpening(viewName))
            {
                Debug.LogError($"请检查 {viewName} 正在异步打开中 请勿重复调用 请检查代码是否一瞬间频繁调用");
                return null;
            }

            AddOpening(viewName);
            view = (BaseView)await YIUIFactory.InstantiateAsync(vo, parent);
            RemoveOpening(viewName);

            m_ExistView.Add(viewName, view);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync(string viewName, object param)
        {
            var view = await GetView(viewName);
            if (view == null) return null;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                var p = ParamVo.Get(param);
                success = await view.Open(p);
                ParamVo.Put(p);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync(string viewName, object param1, object param2)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            var view = await OpenViewAsync(viewName, paramList);
            ListPool<object>.Put(paramList);
            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync(string viewName, object param1, object param2, object param3)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            var view = await OpenViewAsync(viewName, paramList);
            ListPool<object>.Put(paramList);
            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync(string viewName, object param1, object param2, object param3, object param4)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            paramList.Add(param4);
            var view = await OpenViewAsync(viewName, paramList);
            ListPool<object>.Put(paramList);
            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync(string viewName, object param1, object param2, object param3, object param4, params object[] paramMore)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            paramList.Add(param4);
            if (paramMore.Length > 0)
            {
                paramList.AddRange(paramMore);
            }

            var view = await OpenViewAsync(viewName, paramList);
            ListPool<object>.Put(paramList);
            return view;
        }

        #endregion

        #region 异步通过Name打开（泛型类型参数）

        protected async UniTask<BaseView> OpenViewAsync(string viewName)
        {
            var view = await GetView(viewName);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open();
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync<P1>(string viewName, P1 p1)
        {
            var view = await GetView(viewName);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync<P1, P2>(string viewName, P1 p1, P2 p2)
        {
            var view = await GetView(viewName);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3>(string viewName, P1 p1, P2 p2, P3 p3)
        {
            var view = await GetView(viewName);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3, P4>(string viewName, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            var view = await GetView(viewName);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3, p4);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3, P4, P5>(string viewName, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            var view = await GetView(viewName);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3, p4, p5);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        #endregion

        #region 异步通过Type打开（泛型类型参数）

        async UniTask<BaseView> GetView(Type viewType)
        {
            var viewName = viewType.Name;
            return await GetView(viewName);
        }

        protected async UniTask<BaseView> OpenViewAsync(Type viewType)
        {
            var view = await GetView(viewType);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open();
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync<P1>(Type viewType, P1 p1)
        {
            var view = await GetView(viewType);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync<P1, P2>(Type viewType, P1 p1, P2 p2)
        {
            var view = await GetView(viewType);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3>(Type viewType, P1 p1, P2 p2, P3 p3)
        {
            var view = await GetView(viewType);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3, P4>(Type viewType, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            var view = await GetView(viewType);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3, p4);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        protected async UniTask<BaseView> OpenViewAsync<P1, P2, P3, P4, P5>(Type viewType, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            var view = await GetView(viewType);
            if (view == null) return default;

            var success = false;

            await OpenViewBefore(view);

            try
            {
                success = await view.Open(p1, p2, p3, p4, p5);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={view.UIResName}, err={e.Message}{e.StackTrace}");
            }

            await OpenViewAfter(view, success);

            return view;
        }

        #endregion

        #region 同步通过泛型打开（泛型类型参数）

        protected void OpenView<T>() where T : BaseView, new()
        {
            OpenViewAsync<T>().Forget();
        }

        protected void OpenView<T, P1>(P1 p1) where T : BaseView, IYIUIOpen<P1>, new()
        {
            OpenViewAsync<T, P1>(p1).Forget();
        }

        protected void OpenView<T, P1, P2>(P1 p1, P2 p2) where T : BaseView, IYIUIOpen<P1, P2>, new()
        {
            OpenViewAsync<T, P1, P2>(p1, p2).Forget();
        }

        protected void OpenView<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : BaseView, IYIUIOpen<P1, P2, P3>, new()
        {
            OpenViewAsync<T, P1, P2, P3>(p1, p2, p3).Forget();
        }

        protected void OpenView<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) where T : BaseView, IYIUIOpen<P1, P2, P3, P4>, new()
        {
            OpenViewAsync<T, P1, P2, P3, P4>(p1, p2, p3, p4).Forget();
        }

        protected void OpenView<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) where T : BaseView, IYIUIOpen<P1, P2, P3, P4, P5>, new()
        {
            OpenViewAsync<T, P1, P2, P3, P4, P5>(p1, p2, p3, p4, p5).Forget();
        }

        #endregion
    }
}