#if UNITY_EDITOR
namespace YIUIFramework.Editor
{
    /// <summary>
    /// 事件方法
    /// </summary>
    public static class UICreateMethod
    {
        public static string Get(UITable table)
        {
            var sb = SbPool.Get();
            return SbPool.PutAndToStr(sb);
        }
    }
}
#endif