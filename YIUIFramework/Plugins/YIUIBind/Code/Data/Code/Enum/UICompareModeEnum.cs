using Sirenix.OdinInspector;

namespace YIUIBind
{
    /// <summary>
    /// 比较运算枚举y
    /// </summary>
    [LabelText("比较运算")]
    public enum UICompareModeEnum
    {
        [LabelText("<")]
        Less,

        [LabelText("≤")]
        LessEqual,

        [LabelText("=")]
        Equal, //取反就是不等于

        [LabelText(">")]
        Great,

        [LabelText("≥")]
        GreatEqual,
    }
}