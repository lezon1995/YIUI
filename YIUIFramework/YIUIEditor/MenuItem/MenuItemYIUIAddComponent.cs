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
        private static void CustomMenuItem(MenuCommand command)
        {
            Component component = (Component)command.context;

            UIBindComponentTable[] componentTables = component.gameObject.GetComponentsInParent<UIBindComponentTable>();

            var name = component.gameObject.name;
            
            if (componentTables is { Length: > 0 })
            {
                componentTables[0].AddComponent(component);
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