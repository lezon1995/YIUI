using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    //打开泛型 异步
    public abstract partial class BasePanel
    {
        #region 异步通过泛型打开

        protected async UniTask<T> OpenPanelAsync<T>() where T : BasePanel, new()
        {
            return await m_PanelMgr.OpenPanelAsync<T>();
        }

        protected async UniTask<T> OpenPanelAsync<T, P1>(P1 p1) where T : BasePanel, IYIUIOpen<P1>, new()
        {
            return await m_PanelMgr.OpenPanelAsync<T, P1>(p1);
        }

        protected async UniTask<T> OpenPanelAsync<T, P1, P2>(P1 p1, P2 p2) where T : BasePanel, IYIUIOpen<P1, P2>, new()
        {
            return await m_PanelMgr.OpenPanelAsync<T, P1, P2>(p1, p2);
        }

        protected async UniTask<T> OpenPanelAsync<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : BasePanel, IYIUIOpen<P1, P2, P3>, new()
        {
            return await m_PanelMgr.OpenPanelAsync<T, P1, P2, P3>(p1, p2, p3);
        }

        protected async UniTask<T> OpenPanelAsync<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) where T : BasePanel, IYIUIOpen<P1, P2, P3, P4>, new()
        {
            return await m_PanelMgr.OpenPanelAsync<T, P1, P2, P3, P4>(p1, p2, p3, p4);
        }

        protected async UniTask<T> OpenPanelAsync<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) where T : BasePanel, IYIUIOpen<P1, P2, P3, P4, P5>, new()
        {
            return await m_PanelMgr.OpenPanelAsync<T, P1, P2, P3, P4, P5>(p1, p2, p3, p4, p5);
        }

        #endregion

        #region 异步通过Name打开（object类型参数）

        /// <summary>
        /// 用字符串开启 必须保证类名与资源名一致否则无法找到
        /// 首选使用T泛型方法打开UI 字符串只适合于特定场合使用
        /// </summary>
        protected async UniTask<BasePanel> OpenPanelAsync(string panelName, object param = null)
        {
            return await m_PanelMgr.OpenPanelAsync(panelName, param);
        }

        protected async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2)
        {
            return await m_PanelMgr.OpenPanelAsync(panelName, param1, param2);
        }

        protected async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2, object param3)
        {
            return await m_PanelMgr.OpenPanelAsync(panelName, param1, param2, param3);
        }

        protected async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2, object param3, object param4)
        {
            return await m_PanelMgr.OpenPanelAsync(panelName, param1, param2, param3, param4);
        }

        protected async UniTask<BasePanel> OpenPanelAsync(string panelName, object param1, object param2, object param3, object param4, params object[] paramMore)
        {
            return await m_PanelMgr.OpenPanelAsync(panelName, param1, param2, param3, param4, paramMore);
        }

        #endregion

        #region 同步通过泛型打开（泛型类型参数）

        protected void OpenPanel<T>() where T : BasePanel, new()
        {
            m_PanelMgr.OpenPanel<T>();
        }

        protected void OpenPanel<T, P1>(P1 p1) where T : BasePanel, IYIUIOpen<P1>, new()
        {
            m_PanelMgr.OpenPanel<T, P1>(p1);
        }

        protected void OpenPanel<T, P1, P2>(P1 p1, P2 p2) where T : BasePanel, IYIUIOpen<P1, P2>, new()
        {
            m_PanelMgr.OpenPanel<T, P1, P2>(p1, p2);
        }

        protected void OpenPanel<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : BasePanel, IYIUIOpen<P1, P2, P3>, new()
        {
            m_PanelMgr.OpenPanel<T, P1, P2, P3>(p1, p2, p3);
        }

        protected void OpenPanel<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) where T : BasePanel, IYIUIOpen<P1, P2, P3, P4>, new()
        {
            m_PanelMgr.OpenPanel<T, P1, P2, P3, P4>(p1, p2, p3, p4);
        }

        protected void OpenPanel<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) where T : BasePanel, IYIUIOpen<P1, P2, P3, P4, P5>, new()
        {
            m_PanelMgr.OpenPanel<T, P1, P2, P3, P4, P5>(p1, p2, p3, p4, p5);
        }

        #endregion

        #region 同步通过Name打开（object类型参数）

        //非特殊需求 应该尽量使用异步操作
        //同步 无法获得返回值
        protected void OpenPanel(string panelName, object param = null)
        {
            m_PanelMgr.OpenPanel(panelName, param);
        }

        protected void OpenPanel(string panelName, object param1, object param2)
        {
            m_PanelMgr.OpenPanel(panelName, param1, param2);
        }

        protected void OpenPanel(string panelName, object param1, object param2, object param3)
        {
            m_PanelMgr.OpenPanel(panelName, param1, param2, param3);
        }

        protected void OpenPanel(string panelName, object param1, object param2, object param3, object param4)
        {
            m_PanelMgr.OpenPanel(panelName, param1, param2, param3, param4);
        }

        protected void OpenPanel(string panelName, object param1, object param2, object param3, object param4, params object[] paramMore)
        {
            m_PanelMgr.OpenPanel(panelName, param1, param2, param3, param4, paramMore);
        }

        #endregion
    }
}