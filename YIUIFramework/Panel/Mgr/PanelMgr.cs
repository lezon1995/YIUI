using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// YIUI 面板管理器
    /// </summary>
    public partial class PanelMgr : MgrSingleton<PanelMgr>
    {
        //低品质 将会影响动画等逻辑 也可以根据这个参数自定义一些区别
        public static bool IsLowQuality = false;

        //绑定初始化情况
        public static bool BindInit { private set; get; }

        protected override async UniTask<bool> InitMgrAsync()
        {
            BindInit = UIBindHelper.InitAllBind();

            if (await InitRoot())
            {
                InitSafeArea();
                return true;
            }

            return false;
        }

        protected override void OnDispose()
        {
            ResetRoot();
            OnBlockDispose();
        }
    }
}