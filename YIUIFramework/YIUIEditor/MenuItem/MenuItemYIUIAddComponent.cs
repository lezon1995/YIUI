#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using YIUIBind;

namespace YIUIFramework.Editor
{
    [CustomEditor(typeof(Component))]
    public class MenuItemYIUIAddComponent : UnityEditor.Editor
    {
        [MenuItem("CONTEXT/Component/Auto Add %w")]
        static void CustomMenuItem(MenuCommand command)
        {
            var component = (Component)command.context;
            var table = component.gameObject.GetComponentInParent<UIBindCDETable>();
            var name = component.gameObject.name;

            if (table)
            {
                table.AddComponent(component);
                Debug.Log($"Add Component Succeed: [{name},{component.GetType()}] ");
            }
            else
            {
                Debug.LogWarning($"Add Component Fail: [{name},{component.GetType()}] ");
            }
        }
    }
}
#endif