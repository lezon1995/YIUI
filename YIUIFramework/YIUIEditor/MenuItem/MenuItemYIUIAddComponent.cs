#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [CustomEditor(typeof(Component))]
    public class MenuItemYIUIAddComponent : UnityEditor.Editor
    {
        [MenuItem("CONTEXT/Component/Auto Add %w")]
        static void CustomMenuItem(MenuCommand command)
        {
            var component = (Component)command.context;
            var table = component.gameObject.GetComponentInParent<UITable>();
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

        [MenuItem("CONTEXT/Component/Remove All Missing Script")]
        static void RemoveMissingScript(MenuCommand command)
        {
            var component = (Component)command.context;

            var transform = component.transform;
            Clean(transform);
        }

        // 移除指定游戏对象上指定索引的组件
        static void TraverseTransform(Transform t)
        {
            // 获取游戏对象上的所有组件
            Component[] components = t.GetComponents<Component>();

            // 遍历组件列表
            for (int i = components.Length - 1; i >= 0; i--)
            {
                // 如果组件为空，则表示是 Missing 脚本
                if (components[i] == null)
                {
                    // 移除 Missing 脚本
                    RemoveComponentAtIndex(t.gameObject, i);
                    Debug.Log($"删除Missing组件 from{t.gameObject.name}");
                }
            }

            foreach (Transform c in t)
            {
                TraverseTransform(c);
            }
        }

        static void Clean(Transform obj)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj.gameObject);
            for (int i = 0; i < obj.childCount; i++)
            {
                Clean(obj.GetChild(i));
            }
        }

        // 移除指定游戏对象上指定索引的组件
        static void RemoveComponentAtIndex(GameObject go, int index)
        {
            SerializedObject serializedObject = new SerializedObject(go);
            SerializedProperty componentsProperty = serializedObject.FindProperty("m_Component");

            // 删除指定索引处的组件
            componentsProperty.DeleteArrayElementAtIndex(index);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif