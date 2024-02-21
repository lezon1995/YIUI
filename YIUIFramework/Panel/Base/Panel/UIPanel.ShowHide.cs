using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public partial class UIPanel
    {
        public async UniTask Show()
        {
            SetActive(true);

            await InternalOnWindowOpenTween();
        }

        public async UniTask Hide()
        {
            await InternalOnWindowCloseTween();
            
            SetActive(false);
        }
    }
}