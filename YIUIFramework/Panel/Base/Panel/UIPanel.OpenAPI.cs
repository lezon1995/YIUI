// using Cysharp.Threading.Tasks;
//
// namespace YIUIFramework
// {
//     //打开泛型 异步
//     public abstract partial class UIPanel
//     {
//         #region 异步通过泛型打开
//
//         protected async UniTask<T> OpenPanelAsync<T>() where T : UIPanel, new()
//         {
//             return await manager.OpenPanelAsync<T>();
//         }
//
//         protected async UniTask<T> OpenPanelAsync<T, P1>(P1 p1) where T : UIPanel, IOpen<P1>, new()
//         {
//             return await manager.OpenPanelAsync<T, P1>(p1);
//         }
//
//         protected async UniTask<T> OpenPanelAsync<T, P1, P2>(P1 p1, P2 p2) where T : UIPanel, IOpen<P1, P2>, new()
//         {
//             return await manager.OpenPanelAsync<T, P1, P2>(p1, p2);
//         }
//
//         protected async UniTask<T> OpenPanelAsync<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : UIPanel, IOpen<P1, P2, P3>, new()
//         {
//             return await manager.OpenPanelAsync<T, P1, P2, P3>(p1, p2, p3);
//         }
//
//         protected async UniTask<T> OpenPanelAsync<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) where T : UIPanel, IOpen<P1, P2, P3, P4>, new()
//         {
//             return await manager.OpenPanelAsync<T, P1, P2, P3, P4>(p1, p2, p3, p4);
//         }
//
//         protected async UniTask<T> OpenPanelAsync<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) where T : UIPanel, IOpen<P1, P2, P3, P4, P5>, new()
//         {
//             return await manager.OpenPanelAsync<T, P1, P2, P3, P4, P5>(p1, p2, p3, p4, p5);
//         }
//
//         #endregion
//
//         #region 异步通过Name打开（object类型参数）
//
//         /// <summary>
//         /// 用字符串开启 必须保证类名与资源名一致否则无法找到
//         /// 首选使用T泛型方法打开UI 字符串只适合于特定场合使用
//         /// </summary>
//         protected async UniTask<UIPanel> OpenPanelAsync(string panelName, object param = null)
//         {
//             return await manager.OpenPanelAsync(panelName, param);
//         }
//
//         protected async UniTask<UIPanel> OpenPanelAsync(string panelName, object param1, object param2)
//         {
//             return await manager.OpenPanelAsync(panelName, param1, param2);
//         }
//
//         protected async UniTask<UIPanel> OpenPanelAsync(string panelName, object param1, object param2, object param3)
//         {
//             return await manager.OpenPanelAsync(panelName, param1, param2, param3);
//         }
//
//         protected async UniTask<UIPanel> OpenPanelAsync(string panelName, object param1, object param2, object param3, object param4)
//         {
//             return await manager.OpenPanelAsync(panelName, param1, param2, param3, param4);
//         }
//
//         protected async UniTask<UIPanel> OpenPanelAsync(string panelName, object param1, object param2, object param3, object param4, params object[] paramMore)
//         {
//             return await manager.OpenPanelAsync(panelName, param1, param2, param3, param4, paramMore);
//         }
//
//         #endregion
//
//         #region 同步通过泛型打开（泛型类型参数）
//
//         protected void OpenPanel<T>() where T : UIPanel, new()
//         {
//             manager.OpenPanel<T>();
//         }
//
//         protected void OpenPanel<T, P1>(P1 p1) where T : UIPanel, IOpen<P1>, new()
//         {
//             manager.OpenPanel<T, P1>(p1);
//         }
//
//         protected void OpenPanel<T, P1, P2>(P1 p1, P2 p2) where T : UIPanel, IOpen<P1, P2>, new()
//         {
//             manager.OpenPanel<T, P1, P2>(p1, p2);
//         }
//
//         protected void OpenPanel<T, P1, P2, P3>(P1 p1, P2 p2, P3 p3) where T : UIPanel, IOpen<P1, P2, P3>, new()
//         {
//             manager.OpenPanel<T, P1, P2, P3>(p1, p2, p3);
//         }
//
//         protected void OpenPanel<T, P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4) where T : UIPanel, IOpen<P1, P2, P3, P4>, new()
//         {
//             manager.OpenPanel<T, P1, P2, P3, P4>(p1, p2, p3, p4);
//         }
//
//         protected void OpenPanel<T, P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) where T : UIPanel, IOpen<P1, P2, P3, P4, P5>, new()
//         {
//             manager.OpenPanel<T, P1, P2, P3, P4, P5>(p1, p2, p3, p4, p5);
//         }
//
//         #endregion
//
//         #region 同步通过Name打开（object类型参数）
//
//         //非特殊需求 应该尽量使用异步操作
//         //同步 无法获得返回值
//         protected void OpenPanel(string panelName, object param = null)
//         {
//             manager.OpenPanel(panelName, param);
//         }
//
//         protected void OpenPanel(string panelName, object param1, object param2)
//         {
//             manager.OpenPanel(panelName, param1, param2);
//         }
//
//         protected void OpenPanel(string panelName, object param1, object param2, object param3)
//         {
//             manager.OpenPanel(panelName, param1, param2, param3);
//         }
//
//         protected void OpenPanel(string panelName, object param1, object param2, object param3, object param4)
//         {
//             manager.OpenPanel(panelName, param1, param2, param3, param4);
//         }
//
//         protected void OpenPanel(string panelName, object param1, object param2, object param3, object param4, params object[] paramMore)
//         {
//             manager.OpenPanel(panelName, param1, param2, param3, param4, paramMore);
//         }
//
//         #endregion
//     }
// }