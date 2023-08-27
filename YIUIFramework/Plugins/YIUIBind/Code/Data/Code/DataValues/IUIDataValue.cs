namespace YIUIBind
{
    public interface IUIDataValue<out T>
    {
        T GetValue();
    }
}