using Sirenix.OdinInspector;

namespace YIUIBind
{
    //禁止修改ID
    //因为涉及到Mask 所以最高只有32个 请注意
    [LabelText("Type")]
    public enum EUIBindDataType
    {
        [LabelText("bool")]
        Bool = 0,

        [LabelText("string")]
        String = 1,

        [LabelText("int")]
        Int = 2,

        [LabelText("float")]
        Float = 3,

        [LabelText("Vector3")]
        Vector3 = 4,

        [LabelText("List<int>")]
        List_Int = 5,

        [LabelText("List<long>")]
        List_Long = 6,

        [LabelText("List<string>")]
        List_String = 7,

        [LabelText("long")]
        Long = 8,

        [LabelText("uint")]
        Uint = 9,

        [LabelText("ulong")]
        Ulong = 10,

        [LabelText("double")]
        Double = 11,

        [LabelText("Vector2")]
        Vector2 = 12,

        [LabelText("Color")]
        Color = 13,
    }
}