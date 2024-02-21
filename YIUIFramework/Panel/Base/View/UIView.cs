namespace YIUIFramework
{
    public partial class UIView : UIWindow, IView
    {
        public virtual EViewWindowType ViewWindowType => EViewWindowType.View;

        public virtual EViewStackOption StackOption => EViewStackOption.Visible;

        #region 密封生命周期

        protected sealed override void SealedInitialize()
        {
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