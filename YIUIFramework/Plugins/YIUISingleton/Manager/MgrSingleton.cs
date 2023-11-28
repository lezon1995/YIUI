using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    //与Singleton 相比 mgr的单例可以受MgrCenter管理 可实现IManagerUpdate等操作
    public abstract class MgrSingleton<T> : Singleton<T>, IManagerAsyncInit where T : MgrSingleton<T>, new()
    {
        public bool Enabled { get; private set; }
        public bool InitedSucceed { get; private set; }

        async UniTask<bool> IManagerAsyncInit.InitManagerAsync()
        {
            if (InitedSucceed)
            {
                Debug.LogError($"{typeof(T).Name}已成功初始化过 请勿重复初始化");
                return true;
            }

            var result = await InitMgrAsync();
            if (result)
            {
                //成功初始化才记录
                InitedSucceed = true;
            }
            else
            {
                Debug.LogError($"{typeof(T).Name} 初始化失败");
            }

            return result;
        }

        public void SetEnabled(bool value)
        {
            Enabled = value;
        }

        protected sealed override void OnInitSingleton()
        {
            //密封初始化方法 必须使用异步
        }

        protected virtual async UniTask<bool> InitMgrAsync()
        {
            await UniTask.CompletedTask;
            return true;
        }
    }
}