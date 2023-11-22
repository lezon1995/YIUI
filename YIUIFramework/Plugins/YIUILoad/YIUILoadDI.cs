using System;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace YIUIFramework
{
    /// <summary>
    /// 注入加载方法
    /// </summary>
    public static partial class YIUILoadDI
    {
        public delegate (Object, int) LoadAssetFunc(string pkgName, string resName, Type type);

        public delegate UniTask<(Object, int)> LoadAssetAsyncFunc(string pkgName, string resName, Type type);

        public delegate bool VerifyAssetFunc(string pkgName, string resName);

        public delegate void ReleaseAssetFunc(int identifier);

        public delegate void ReleaseAllAssetFunc();

        //同步加载方法
        public static LoadAssetFunc LoadAsset { internal get; set; }

        //异步加载方法
        public static LoadAssetAsyncFunc LoadAssetAsync { internal get; set; }

        //验证是否有效
        public static VerifyAssetFunc VerifyAsset { internal get; set; }

        //释放方法
        public static ReleaseAssetFunc ReleaseAsset { internal get; set; }

        //释放所有方法
        public static ReleaseAllAssetFunc ReleaseAllAsset { internal get; set; }
    }
}