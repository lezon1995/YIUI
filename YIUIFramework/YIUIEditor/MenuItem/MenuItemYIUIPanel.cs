#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


namespace YIUIFramework.Editor
{
    public static class MenuItemYIUIPanel
    {
        [MenuItem("Assets/YIUI/Create UIPanel", false, 0)]
        static void CreateYIUIPanelByFolder()
        {
            var activeObject = Selection.activeObject as DefaultAsset;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError($"请在路径 {UIConst.ResPath}/xxx/{UIConst.Source} 下右键创建");
                return;
            }

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (activeObject.name != UIConst.Source || !path.Contains(UIConst.ResPath))
            {
                UnityTipsHelper.ShowError($"请在路径 {UIConst.ResPath}/xxx/{UIConst.Source} 下右键创建");
                return;
            }

            CreateYIUIPanelByPath(path);
        }

        internal static void CreateYIUIPanelByPath(string path, string name = null)
        {
            if (!path.Contains(UIConst.ResPath))
            {
                UnityTipsHelper.ShowError($"请在路径 {UIConst.ResPath}/xxx/{UIConst.Source} 下右键创建");
                return;
            }

            var saveName = string.IsNullOrEmpty(name)
                ? UIConst.YIUIPanelSourceName
                : $"{name}{UIConst.PanelSourceName}";
            var savePath = $"{path}/{saveName}.prefab";

            if (AssetDatabase.LoadAssetAtPath(savePath, typeof(Object)) != null)
            {
                UnityTipsHelper.ShowError($"已存在 请先重命名 {saveName}");
                return;
            }

            var createPanel = CreateYIUIPanel();
            var panelPrefab = PrefabUtility.SaveAsPrefabAsset(createPanel, savePath);
            Object.DestroyImmediate(createPanel);
            Selection.activeObject = panelPrefab;
        }

        [MenuItem("GameObject/YIUI/Create UIPanel", false, 0)]
        static void CreateYIUIPanelByGameObject()
        {
            var activeObject = Selection.activeObject as GameObject;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError($"请选择UIRoot 右键创建");
                return;
            }

            if (activeObject.name != PanelMgr.UILayerRootName)
            {
                UnityTipsHelper.ShowError($"只能在指定的 {PanelMgr.UILayerRootName} 下使用 快捷创建Panel");
                return;
            }

            Selection.activeObject = CreateYIUIPanel(activeObject);
        }

        static GameObject CreateYIUIPanel(GameObject activeObject = null)
        {
            //panel
            var panel = new GameObject();
            var transform = panel.GetOrAddComponent<RectTransform>();
            panel.GetOrAddComponent<CanvasRenderer>();
            var table = panel.GetOrAddComponent<UITable>();
            table.UICodeType = UIType.Panel;
            table.IsSplitData = true;

            // table.PanelOption |= EPanelOption.DisReset; //如果想要都是默认缓存界面可开启
            var splitData = table.PanelSplitData;
            splitData.Panel = panel;
            panel.name = UIConst.YIUIPanelSourceName;
            if (activeObject)
            {
                transform.SetParent(activeObject.transform, false);
            }

            transform.ResetToFullScreen();

            //UI射线阻挡
            var block = new GameObject();
            var blockRectTrans = block.GetOrAddComponent<RectTransform>();
            block.GetOrAddComponent<CanvasRenderer>();
            block.GetOrAddComponent<UIBlock>();
            block.name = "UIBlock";
            blockRectTrans.SetParent(transform, false);
            blockRectTrans.ResetToFullScreen();

            //Panel主要内容
            var content = new GameObject();
            var contentRectTrans = content.GetOrAddComponent<RectTransform>();
            content.GetOrAddComponent<CanvasRenderer>();
            content.name = "Content";
            contentRectTrans.SetParent(transform, false);
            contentRectTrans.ResetToFullScreen();

            // 添加viewTabs节点
            var viewTabs = new GameObject();
            var viewTabsRect = viewTabs.GetOrAddComponent<RectTransform>();
            viewTabs.name = UIConst.ViewTabsName;
            viewTabsRect.SetParent(transform, false);
            viewTabsRect.ResetToFullScreen();
            splitData.ViewTabsParent = viewTabsRect;

            // 添加viewPopups节点
            var viewPopups = new GameObject();
            var viewPopupsRect = viewPopups.GetOrAddComponent<RectTransform>();
            viewPopups.name = UIConst.ViewPopupsName;
            viewPopupsRect.SetParent(transform, false);
            viewPopupsRect.ResetToFullScreen();
            splitData.ViewPopupsParent = viewPopupsRect;

            panel.SetLayerRecursively(LayerMask.NameToLayer("UI"));

            return panel;
        }
    }
}
#endif