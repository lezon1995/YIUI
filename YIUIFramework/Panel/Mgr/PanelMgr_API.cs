namespace YIUIFramework
{
    public partial class PanelMgr
    {
        public bool ActiveSelf(string panelName)
        {
            if (TryGetPanelInfo(panelName, out var panelInfo))
            {
                return panelInfo.ActiveSelf;
            }

            return false;
        }

        public bool ActiveSelf<T>() where T : BasePanel
        {
            if (TryGetPanelInfo<T>(out var panelInfo))
            {
                return panelInfo.ActiveSelf;
            }

            return false;
        }
    }
}