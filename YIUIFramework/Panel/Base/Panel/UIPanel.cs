namespace YIUIFramework
{
    /// <summary>
    /// 面板基类, 编辑器创建面板代码时自动带入
    /// </summary>
    public abstract partial class UIPanel : UIWindow, IPanel
    {
        public virtual EPanelLayer Layer => EPanelLayer.Panel;
        public virtual EPanelOption PanelOption => EPanelOption.None;
        public virtual EPanelStackOption StackOption => EPanelStackOption.Visible;

        //容器类界面 //比如伤害飘字 此类界面如果做到panel层会被特殊处理 建议还是不要放到panel层
        public virtual bool PanelContainer => PanelOption.Has(EPanelOption.Container);

        //永久缓存界面 //永远不会被摧毁 与禁止关闭不同这个可以关闭 只是不销毁 也可相当于无限长的倒计时
        public virtual bool PanelForeverCache => PanelOption.Has(EPanelOption.ForeverCache);

        //倒计时缓存界面 //被关闭后X秒之后在摧毁 否则理解摧毁
        public virtual bool PanelTimeCache => PanelOption.Has(EPanelOption.TimeCache);

        //禁止关闭的界面 //是需要一直存在的你可以隐藏 但是你不能摧毁
        public virtual bool PanelDisClose => PanelOption.Has(EPanelOption.DisClose);

        //忽略返回 返回操作会跳过这个界面 //他的打开与关闭不会触发返回功能 堆栈功能
        public virtual bool PanelIgnoreBack => PanelOption.Has(EPanelOption.IgnoreBack);
        
        /// <summary>
        /// 优先级，用于同层级排序,
        /// 大的在前 小的在后
        /// 相同时 后添加的在前
        /// </summary>
        public virtual int Priority => 0;

        #region 密封生命周期

        protected sealed override void SealedInitialize()
        {
            InitPanelViewData();
        }

        protected sealed override void SealedStart()
        {
        }

        protected sealed override void SealedOnDestroy()
        {
        }

        #endregion
    }
}