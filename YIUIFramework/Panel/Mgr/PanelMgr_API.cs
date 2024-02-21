using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public partial class PanelMgr
    {
        public bool IsActive(string panelName)
        {
            if (TryGetPanelInfo(panelName, out var panelInfo))
            {
                return panelInfo.Panel is { ActiveSelf: true };
            }

            return false;
        }

        public bool IsActive<T>() where T : UIPanel
        {
            if (TryGetPanelInfo<T>(out var panelInfo))
            {
                return panelInfo.Panel is { ActiveSelf: true };
            }

            return false;
        }

        public void Show<T>(bool tween = false) where T : UIPanel
        {
            if (TryGetPanelInfo<T>(out var panelInfo))
            {
                if (tween)
                {
                    panelInfo.Panel.Show().Forget();
                }
                else
                {
                    panelInfo.Panel.SetActive(true);
                }
            }
        }

        public void Hide<T>(bool tween = false) where T : UIPanel
        {
            if (TryGetPanelInfo<T>(out var panelInfo))
            {
                if (tween)
                {
                    panelInfo.Panel.Hide().Forget();
                }
                else
                {
                    panelInfo.Panel.SetActive(false);
                }
            }
        }
    }
}