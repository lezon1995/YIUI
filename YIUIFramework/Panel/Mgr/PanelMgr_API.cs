namespace YIUIFramework
{
    public partial class PanelMgr
    {
        public bool ActiveSelf(string panelName)
        {
            var info = GetPanelInfo(panelName);
            return info is {ActiveSelf: true};
        }

        public bool ActiveSelf<T>() where T : BasePanel
        {
            var info = GetPanelInfo<T>();
            return info is {ActiveSelf: true};
        }
    }
}