using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 异步打开
    /// </summary>
    public partial class PanelMgr
    {
        #region 异步通过泛型打开

        async UniTask<PanelInfo> OpenPanelStartAsync(string panelName)
        {
#if YIUIMACRO_PANEL_OPENCLOSE
            Debug.Log($"<color=yellow> 打开UI: {panelName} </color>");
#endif

            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogError($"<color=red> 无法打开 这是一个空名称 </color>");
                return null;
            }

            if (!panelInfos.TryGetValue(panelName, out var info))
            {
                Debug.LogError($"请检查 {panelName} 没有获取到PanelInfo  1. 必须继承IPanel 的才可行  2. 检查是否没有注册上");
                return null;
            }

            if (info.Panel == null)
            {
                if (PanelIsOpening(panelName))
                {
                    Debug.LogError($"请检查 {panelName} 正在异步打开中 请勿重复调用 请检查代码是否一瞬间频繁调用");
                    return null;
                }

                AddOpening(panelName);
                var panel = await YIUIFactory.CreatePanelAsync(info);
                RemoveOpening(panelName);
                if (panel == null)
                {
                    Debug.LogError($"面板[{panelName}]没有创建成功，packName={info.PkgName}, resName={info.ResName}");
                    return null;
                }

                panel.SetActive(false);
                info.Panel = panel;
            }

            AddUI(info);
            return info;
        }

        public async UniTask<T> OpenPanelAsync<T>() where T : BasePanel, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.Panel.Open();
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        public async UniTask<T> OpenPanelAsync<T, P1>(P1 p1) where T : BasePanel, IYIUIOpen<P1>, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.Panel.Open(p1);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        public async UniTask<T> OpenPanelAsync<T, P1, P2>(P1 p1, P2 p2) where T : BasePanel, IYIUIOpen<P1, P2>, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.Panel.Open(p1, p2);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        public async UniTask<T> OpenPanelAsync<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : BasePanel, IYIUIOpen<P1, P2, P3>, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.Panel.Open(p1, p2, p3);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        public async UniTask<T> OpenPanelAsync<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) where T : BasePanel, IYIUIOpen<P1, P2, P3, P4>, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.Panel.Open(p1, p2, p3, p4);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        public async UniTask<T> OpenPanelAsync<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) where T : BasePanel, IYIUIOpen<P1, P2, P3, P4, P5>, new()
        {
            var info = await OpenPanelStartAsync(GetPanelName<T>());
            if (info == null) return default;

            var success = false;

            await OpenPanelBefore(info);

            try
            {
                success = await info.Panel.Open(p1, p2, p3, p4, p5);
            }
            catch (Exception e)
            {
                Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
            }

            return (T)await OpenPanelAfter(info, success);
        }

        #endregion

        #region 异步通过Name打开（object类型参数）

        /// <summary>
        /// 获取PanelInfo
        /// 没有则创建  相当于一个打开过了 UI基础配置档
        /// 这个根据BindVo创建  为什么没有直接用VO  因为里面有Panel 实例对象
        /// 这个k 根据resName
        /// </summary>
        bool TryGetPanelInfo(string panelName, out PanelInfo panelInfo)
        {
            if (UIBindHelper.TryGetBindVoByPanelName(panelName, out var vo))
            {
                if (panelInfos.TryGetValue(panelName, out panelInfo))
                {
                    return true;
                }

                panelInfo = new PanelInfo(panelName, vo.PkgName, vo.ResName);
                panelInfos.Add(panelName, panelInfo);
                return true;
            }

            panelInfo = null;
            return false;
        }

        /// <summary>
        /// 用字符串开启 必须保证类名与资源名一致否则无法找到
        /// 首选使用T泛型方法打开UI 字符串只适合于特定场合使用
        /// </summary>
        public async UniTask<BasePanel> OpenPanelAsync(string panelName, object param = null)
        {
            if (TryGetPanelInfo(panelName, out var info))
            {
                var panel = await OpenPanelStartAsync(panelName);
                if (panel == null)
                {
                    return default;
                }

                var success = false;

                await OpenPanelBefore(info);

                try
                {
                    var p = ParamVo.Get(param);
                    success = await info.Panel.Open(p);
                    ParamVo.Put(p);
                }
                catch (Exception e)
                {
                    Debug.LogError($"panel={info.ResName}, err={e.Message}{e.StackTrace}");
                }

                return await OpenPanelAfter(info, success);
            }

            return default;
        }

        public async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            var panel = await OpenPanelAsync(panelName, paramList);
            ListPool<object>.Put(paramList);
            return panel;
        }

        public async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2, object param3)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            var panel = await OpenPanelAsync(panelName, paramList);
            ListPool<object>.Put(paramList);
            return panel;
        }

        public async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2, object param3, object param4)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            paramList.Add(param4);
            var panel = await OpenPanelAsync(panelName, paramList);
            ListPool<object>.Put(paramList);
            return panel;
        }

        public async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2, object param3, object param4, params object[] paramMore)
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

            var panel = await OpenPanelAsync(panelName, paramList);
            ListPool<object>.Put(paramList);
            return panel;
        }

        #endregion

        #region 同步通过泛型打开（泛型类型参数）

        public void OpenPanel<T>() where T : BasePanel, new()
        {
            OpenPanelAsync<T>().Forget();
        }

        public void OpenPanel<T, P1>(P1 p1) where T : BasePanel, IYIUIOpen<P1>, new()
        {
            OpenPanelAsync<T, P1>(p1).Forget();
        }

        public void OpenPanel<T, P1, P2>(P1 p1, P2 p2) where T : BasePanel, IYIUIOpen<P1, P2>, new()
        {
            OpenPanelAsync<T, P1, P2>(p1, p2).Forget();
        }

        public void OpenPanel<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : BasePanel, IYIUIOpen<P1, P2, P3>, new()
        {
            OpenPanelAsync<T, P1, P2, P3>(p1, p2, p3).Forget();
        }

        public void OpenPanel<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) where T : BasePanel, IYIUIOpen<P1, P2, P3, P4>, new()
        {
            OpenPanelAsync<T, P1, P2, P3, P4>(p1, p2, p3, p4).Forget();
        }

        public void OpenPanel<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) where T : BasePanel, IYIUIOpen<P1, P2, P3, P4, P5>, new()
        {
            OpenPanelAsync<T, P1, P2, P3, P4, P5>(p1, p2, p3, p4, p5).Forget();
        }

        #endregion

        #region 同步通过Name打开（object类型参数）

        //非特殊需求 应该尽量使用异步操作
        //同步 无法获得返回值
        public void OpenPanel(string panelName, object param = null)
        {
            OpenPanelAsync(panelName, param).Forget();
        }

        public void OpenPanel(string panelName, object param1, object param2)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            OpenPanel(panelName, paramList);
            ListPool<object>.Put(paramList);
        }

        public void OpenPanel(string panelName, object param1, object param2, object param3)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            OpenPanel(panelName, paramList);
            ListPool<object>.Put(paramList);
        }

        public void OpenPanel(string panelName, object param1, object param2, object param3, object param4)
        {
            var paramList = ListPool<object>.Get();
            paramList.Add(param1);
            paramList.Add(param2);
            paramList.Add(param3);
            paramList.Add(param4);
            OpenPanel(panelName, paramList);
            ListPool<object>.Put(paramList);
        }

        public void OpenPanel(string panelName, object param1, object param2, object param3, object param4, params object[] paramMore)
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

            OpenPanel(panelName, paramList);
            ListPool<object>.Put(paramList);
        }

        #endregion
    }
}