using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// 倒计时摧毁面板 适用于倒计时缓存界面
    /// </summary>
    public abstract partial class UIPanel
    {
        protected virtual float CachePanelTime => 10;

        CancellationTokenSource cts;

        internal void CacheTimeCountDownDestroyPanel()
        {
            StopCountDownDestroyPanel();
            cts = new CancellationTokenSource();
            DoCountDownDestroyPanel(cts.Token).Forget();
        }

        internal void StopCountDownDestroyPanel()
        {
            if (cts == null)
            {
                return;
            }

            cts.Cancel();
            cts.Dispose();
            cts = null;
        }

        async UniTaskVoid DoCountDownDestroyPanel(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(CachePanelTime), cancellationToken: token);
            }
            catch (OperationCanceledException)
            {
                //取消倒计时 正常操作不需要日志
                return;
            }

            UnityEngine.Object.Destroy(GameObject);
            PanelMgr.Inst.RemoveUIReset(UIResName);
        }
    }
}