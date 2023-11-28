#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace YIUIFramework.Editor
{
    [HideReferenceObjectPicker]
    public class BaseYIUIToolModule : BaseCreateModule
    {
        public YIUIAutoTool AutoTool { get; internal set; }

        public OdinMenuTree Tree { get; internal set; }

        public string ModuleName { get; internal set; }

        public virtual void SelectionMenu()
        {
        }
    }
}
#endif