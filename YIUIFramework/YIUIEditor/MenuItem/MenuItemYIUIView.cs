#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    public static class MenuItemYIUIView
    {
        [MenuItem("GameObject/YIUI/Create UIView", false, 1)]
        static void CreateYIUIView()
        {
            var activeObject = Selection.activeObject as GameObject;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError($"请选择ViewParent 右键创建");
                return;
            }

            var table = activeObject.transform.parent.GetComponentInParent<UITable>();
            if (table == null)
            {
                UnityTipsHelper.ShowError($"只能在AllViewParent / AllPopupViewParent 下使用 快捷创建View");
                return;
            }

            if (table.UICodeType != UIType.Panel)
            {
                UnityTipsHelper.ShowError($"必须是Panel 下使用 快捷创建View");
                return;
            }

            var panelEditorData = table.PanelSplitData;

            if (activeObject != panelEditorData.ViewTabsParent.gameObject &&
                activeObject != panelEditorData.ViewPopupsParent.gameObject)
            {
                UnityTipsHelper.ShowError($"只能在AllViewParent / AllPopupViewParent 下使用 快捷创建View");
                return;
            }


            //ViewParent
            var viewParentObject = new GameObject();
            var viewParentRect = viewParentObject.GetOrAddComponent<RectTransform>();
            viewParentObject.name = UIConst.YIUIViewParentName;
            viewParentRect.SetParent(activeObject.transform, false);
            viewParentRect.ResetToFullScreen();


            //View
            var viewObject = new GameObject();
            var viewRect = viewObject.GetOrAddComponent<RectTransform>();
            viewObject.GetOrAddComponent<CanvasRenderer>();
            var uiTable = viewObject.GetOrAddComponent<UITable>();
            uiTable.UICodeType = UIType.View;
            viewObject.name = UIConst.YIUIViewName;
            viewRect.SetParent(viewParentRect, false);
            viewRect.ResetToFullScreen();


            if (activeObject == panelEditorData.ViewTabsParent.gameObject)
            {
                panelEditorData.ViewTabs.Add(viewParentRect);
                uiTable.ViewWindowType = EViewWindowType.View;
            }
            else if (activeObject == panelEditorData.ViewPopupsParent.gameObject)
            {
                panelEditorData.ViewPopups.Add(viewParentRect);
                uiTable.ViewWindowType = EViewWindowType.Popup;
            }

            viewParentObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));
            Selection.activeObject = viewParentObject;
        }
    }
}
#endif