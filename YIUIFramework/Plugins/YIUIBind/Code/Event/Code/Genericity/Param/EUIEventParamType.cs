using Sirenix.OdinInspector;

namespace YIUIBind
{
    // [LabelText("事件参数类型")]
    [HideLabel]
    public enum EUIEventParamType
    {
        [LabelText("ParamVo")]
        ParamVo = 0,

        [LabelText("object")]
        Object = 1,

        [LabelText("bool")]
        Bool = 2,

        [LabelText("string")]
        String = 3,

        [LabelText("int")]
        Int = 4,

        [LabelText("float")]
        Float = 5,

        [LabelText("Vector3")]
        Vector3 = 6,

        [LabelText("List<int>")]
        List_Int = 7,

        [LabelText("List<long>")]
        List_Long = 8,

        [LabelText("List<string>")]
        List_String = 9,

        [LabelText("long")]
        Long = 10,

        [LabelText("uint")]
        Uint = 11,

        [LabelText("ulong")]
        Ulong = 12,

        [LabelText("double")]
        Double = 13,

        [LabelText("Vector2")]
        Vector2 = 14,

        [LabelText("GameObject")]
        UnityGameObject = 15,
    }
}