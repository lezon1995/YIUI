using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// 关闭
    /// </summary>
    public abstract partial class UIPanel
    {
        public void Close(bool tween = true, bool ignoreElse = false)
        {
            CloseAsync(tween, ignoreElse).Forget();
        }

        public async UniTask CloseAsync(bool tween = true, bool ignoreElse = false)
        {
            await manager.ClosePanelAsync(UIResName, tween, ignoreElse);
        }

        protected void Home<T>(bool tween = true) where T : UIPanel
        {
            manager.HomePanel<T>(tween).Forget();
        }

        protected void Home(string homeName, bool tween = true)
        {
            manager.HomePanel(homeName, tween).Forget();
        }
    }
}