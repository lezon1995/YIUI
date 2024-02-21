#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// 面板逆向 转换为源数据
    /// </summary>
    public static class MenuItemYIUIPanelToSource
    {
        [MenuItem("Assets/YIUI/Panel 逆向 源数据 Source", false, 1)]
        static void CreateYIUIPanelByFolder()
        {
            var activeObject = Selection.activeObject as GameObject;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError("必须选择一个Panel 对象");
                return;
            }

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (!path.Contains(UIConst.ResPath))
            {
                UnityTipsHelper.ShowError($"请在路径 {UIConst.ResPath}/xxx/{UIConst.PanelName} 下右键选择一个Panel 进行转换");
                return;
            }

            var table = activeObject.GetComponent<UITable>();
            if (table == null)
            {
                UnityTipsHelper.ShowError("预设错误 没有 UITable");
                return;
            }

            if (table.UICodeType != UIType.Panel)
            {
                UnityTipsHelper.ShowError("预设错误 必须是Panel");
                return;
            }

            if (table.IsSplitData)
            {
                UnityTipsHelper.ShowError("这是一个源数据 无法逆向转换");
                return;
            }

            if (string.IsNullOrEmpty(table.PkgName))
            {
                table.AutoCheck();
            }

            if (string.IsNullOrEmpty(table.PkgName))
            {
                UnityTipsHelper.ShowError("未知错误 无法识别 包名");
                return;
            }

            var newSourceName = $"{table.name}{UIConst.Source}";
            var savePath = $"{UIConst.ResPath}/{table.PkgName}/{UIConst.Source}/{newSourceName}.prefab";

            if (AssetDatabase.LoadAssetAtPath(savePath, typeof(Object)) != null)
            {
                UnityTipsHelper.CallBack("源数据已存在!!! 此操作将会覆盖旧的数据!!!", () => { CreateNewSource(path, savePath); });
            }
            else
            {
                CreateNewSource(path, savePath);
            }
        }

        static void CreateNewSource(string loadPath, string savePath)
        {
            var loadPanel = (GameObject)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Object));
            if (loadPanel == null)
            {
                UnityTipsHelper.ShowError($"未知错误 没有加载到Panel 请检查 {loadPath}");
                return;
            }

            var newSource = UIMenuItemHelper.CopyGameObject(loadPanel);
            var table = newSource.GetComponent<UITable>();
            table.IsSplitData = true;
            newSource.name = $"{loadPanel.name}{UIConst.Source}";

            CorrelationView(table);

            PrefabUtility.SaveAsPrefabAsset(newSource, savePath);
            Object.DestroyImmediate(newSource);

            UnityTipsHelper.Show($"Panel 逆向 源数据 完毕");
            AssetDatabase.Refresh();
        }

        //关联UI
        static void CorrelationView(UITable table)
        {
            CorrelationViewByParent(table.PkgName, table.PanelSplitData.ViewTabsStatic);
            CorrelationViewByParent(table.PkgName, table.PanelSplitData.ViewTabs);
            CorrelationViewByParent(table.PkgName, table.PanelSplitData.ViewPopups);
        }

        static void CorrelationViewByParent(string pkgName, List<RectTransform> parentList)
        {
            foreach (var viewParent in parentList)
            {
                if (viewParent)
                {
                    var viewName = viewParent.name.Replace(UIConst.ParentName, "");

                    var viewPath = $"{UIConst.ResPath}/{pkgName}/{UIConst.Prefabs}/{viewName}.prefab";

                    var childView = viewParent.FindChildByName(viewName);
                    if (childView)
                    {
                        Object.DestroyImmediate(childView.gameObject);
                    }

                    AddView(viewPath, viewParent);
                }
            }
        }

        //吧其他view 关联上
        static void AddView(string loadPath, Transform parent)
        {
            var loadView = (GameObject)AssetDatabase.LoadAssetAtPath(loadPath, typeof(Object));
            if (loadView == null)
            {
                UnityTipsHelper.ShowError($"未知错误 没有加载到 请检查 {loadPath}");
                return;
            }

            var prefabInstance = PrefabUtility.InstantiatePrefab(loadView) as GameObject;
            if (prefabInstance == null)
            {
                Debug.LogError($"{loadView.name} 未知错误 得到一个null 对象");
                return;
            }

            prefabInstance.transform.SetParent(parent, false);
        }
    }
}
#endif