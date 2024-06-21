using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    //基类使用odin 方便做一些显示
    //没有的不需要的可以改为UnityEngine.MonoBehaviour
    public abstract class DisposerMonoSingleton : SerializedMonoBehaviour, IManagerAsyncInit
    {
        public bool Disposed { get; private set; }

        //释放方法1: 对象释放
        public bool Dispose()
        {
            if (Disposed)
            {
                return false;
            }

            Disposed = true;
            OnDispose();
            gameObject.SafeDestroySelf();
            return true;
        }

        //如非必要 任何子类都不要重写unity的OnDestroy
        //重写请base
        //推荐使用OnDispose
        protected virtual void OnDestroy()
        {
            if (!Disposed)
            {
                if (GetDontDestroyOnLoad())
                {
                    if (UIOperationHelper.IsPlaying())
                    {
                        //进入到这里说明不是被dispose 调用后摧毁的
                        //而是直接被摧毁的 这种行为是不允许的
                        Debug.LogError($"{name} 请调用 Dispose/DisposeInst 来移除Mono单例 而非直接删除GameObject对象");
                    }
                }
            }
        }

        /// <summary>
        /// 是否切换场景时不销毁销毁
        /// </summary>
        protected virtual bool GetDontDestroyOnLoad()
        {
            return true;
        }

        /// <summary>
        /// 实例对象隐藏
        /// </summary>
        protected virtual bool GetHideAndDontSave()
        {
            return false;
        }

        /// <summary>
        /// 处理释放相关事情
        /// </summary>
        protected virtual void OnDispose()
        {
        }

        //初始化回调
        protected virtual void OnInitSingleton()
        {
        }

        //每次使用前回调
        protected virtual void OnUseSingleton()
        {
        }


        //在mono中无效
        public bool Enabled => true;

        public bool InitedSucceed { get; private set; }

        async UniTask<bool> IManagerAsyncInit.InitManagerAsync()
        {
            if (InitedSucceed)
            {
                Debug.LogError($"{gameObject.name}已成功初始化过 请勿重复初始化");
                return true;
            }

            var result = await MgrAsyncInit();
            if (result)
            {
                //成功初始化才记录
                InitedSucceed = true;
            }
            else
            {
                Debug.LogError($"{gameObject.name} 初始化失败");
            }

            return result;
        }

        protected virtual async UniTask<bool> MgrAsyncInit()
        {
            await UniTask.CompletedTask;
            return true;
        }
    }
}