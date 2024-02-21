using Sirenix.OdinInspector;

namespace YIUIFramework
{
    //不要修改值 否则已存在的界面会错误
    //只能新增 不允许修改
    [LabelText("ViewType")]
    public enum EViewWindowType
    {
        [LabelText("TabView")]
        View = 0,

        [LabelText("PopupView")]
        Popup = 1,
    }
}