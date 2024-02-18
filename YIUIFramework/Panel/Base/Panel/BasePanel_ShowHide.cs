using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public partial class BasePanel
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