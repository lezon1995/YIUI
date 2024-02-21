#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    public static class MenuItemYIUIComponent
    {
        [MenuItem("GameObject/YIUI/Create UIComponent", false, 2)]
        static void CreateYIUIComponent()
        {
            var activeObject = Selection.activeObject as GameObject;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError($"请选择一个目标");
                return;
            }

            //Component
            var componentObject = new GameObject();
            var viewRect = componentObject.GetOrAddComponent<RectTransform>();
            componentObject.GetOrAddComponent<CanvasRenderer>();
            var table = componentObject.GetOrAddComponent<UITable>();
            table.UICodeType = UIType.Component;
            viewRect.SetParent(activeObject.transform, false);

            componentObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));
            Selection.activeObject = componentObject;
        }
    }
}
#endif