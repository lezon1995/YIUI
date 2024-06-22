﻿#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    public static class UICreateModule
    {
        internal static void Create(UITable table, bool refresh, bool tips)
        {
            if (!UIOperationHelper.CheckUIOperation())
                return;

            /*
            //留在这里看的 方便以后查API
            //当这个是个资源的时候 存在磁盘中
            var is0 = UnityEditor.EditorUtility.IsPersistent(table);
            //返回></para>如果对象是Prefab的一部分，则为True/返回>Functional
            var is1= UnityEditor.PrefabUtility.IsPartOfAnyPrefab(table);
            //</para> . 0如果对象是不能编辑的Prefab的一部分，则为True
            var is2= UnityEditor.PrefabUtility.IsPartOfImmutablePrefab(table);
            //>如果给定对象是Model Prefab Asset或Model Prefab实例的一部分，则返回true
            var is3= UnityEditor.PrefabUtility.IsPartOfModelPrefab(table);
            //<摘要></para>如果给定对象是预制资源的一部分，则返回true
            var is4= UnityEditor.PrefabUtility.IsPartOfPrefabAsset(table);
            //摘要></para>如果给定对象是Prefab实例的一部分，则返回true
            var is5= UnityEditor.PrefabUtility.IsPartOfPrefabInstance(table);
            //a>如果给定对象是常规预制实例或预制资源的一部分，则返回true。</para>
            var is6= UnityEditor.PrefabUtility.IsPartOfRegularPrefab(table);
            //>如果给定对象是Prefab Variant资产或Prefab Variant实例的一部分，则返回true。
            var is7= UnityEditor.PrefabUtility.IsPartOfVariantPrefab(table);
            //>如果给定对象是Prefab实例的一部分，而不是资产的一部分，则返回true。</pa
            var is8= UnityEditor.PrefabUtility.IsPartOfNonAssetPrefabInstance(table);
            //>这个对象是Prefab的一部分，不能应用吗?
            var is9= UnityEditor.PrefabUtility.IsPartOfPrefabThatCanBeAppliedTo(table);
            */


            if (PrefabUtility.IsPartOfPrefabInstance(table))
            {
                UnityTipsHelper.ShowErrorContext(table, $"不能对实体进行操作  必须进入预制体编辑!!!");
                return;
            }

            if (PrefabUtility.IsPartOfPrefabAsset(table) == false)
            {
                UnityTipsHelper.ShowErrorContext(table, $"1: 必须是预制体 2: 不能在Hierarchy面板中使用 必须在Project面板下的预制体原件才能使用使用 ");
                return;
            }

            if (!table.AutoCheck())
                return;

            var baseData = new UICreateBaseData
            {
                AutoRefresh = refresh,
                ShowTips = tips,
                Namespace = UIConst.Namespace,
                PkgName = table.PkgName,
                ResName = table.ResName,
                BaseClass = GetBaseClass(table),
                Variables = UICreateVariables.Get(table),
                UIBind = UICreateBind.GetBind(table),
                UIUnBind = UICreateBind.GetUnBind(table),
                VirtualMethod = UICreateMethod.Get(table),
                PanelViewEnum = UICreatePanelViewEnum.Get(table),
            };

            _ = new UICreateBaseCode(out var resultBase, YIUIAutoTool.Author, baseData);

            if (!resultBase)
                return;

            switch (table.UICodeType)
            {
                case UIType.Panel:
                {
                    var data = new UICreatePanelData
                    {
                        AutoRefresh = refresh,
                        ShowTips = tips,
                        Namespace = UIConst.Namespace,
                        PkgName = table.PkgName,
                        ResName = table.ResName,
                    };

                    _ = new UICreatePanelCode(out var result, YIUIAutoTool.Author, data);

                    if (result)
                        break;

                    return;
                }
                case UIType.View:
                {
                    var data = new UICreateViewData
                    {
                        AutoRefresh = refresh,
                        ShowTips = tips,
                        Namespace = UIConst.Namespace,
                        PkgName = table.PkgName,
                        ResName = table.ResName,
                    };

                    _ = new UICreateViewCode(out var result, YIUIAutoTool.Author, data);

                    if (result)
                        break;

                    return;
                }
                case UIType.Component:
                {
                    //目前看上去3个DATA都一样 是特意设定的 以后可独立扩展
                    var data = new UICreateComponentData
                    {
                        AutoRefresh = refresh,
                        ShowTips = tips,
                        Namespace = UIConst.Namespace,
                        PkgName = table.PkgName,
                        ResName = table.ResName,
                    };

                    _ = new UICreateComponentCode(out var result, YIUIAutoTool.Author, data);

                    if (result)
                        break;

                    return;
                }
                default:
                    Debug.LogError($"是新增了 新类型嘛????? {table.UICodeType}");
                    break;
            }

            AssetDatabase.Refresh();
        }

        static string GetBaseClass(UITable table)
        {
            switch (table.UICodeType)
            {
                case UIType.Panel:
                    return UIConst.BasePanelName;
                case UIType.View:
                    return UIConst.BaseViewName;
                case UIType.Component:
                    return UIConst.BaseComponentName;
                default:
                    Debug.LogError($"是否新增了类型????");
                    return UIConst.UIBaseName;
            }
        }

        internal static bool InitVoName(UITable table)
        {
            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(table);
            var pkgName = GetPkgName(path);
            if (pkgName.IsEmpty())
            {
                UnityTipsHelper.ShowErrorContext(table, $"没有找到模块名 请在预制体上使用 且 必须在指定的文件夹下才可使用 {UIConst.ResPath}");
                return false;
            }

            table.PkgName = Regex.Replace(pkgName, NameUtility.NameRegex, "");
            table.ResName = Regex.Replace(table.name, NameUtility.NameRegex, "");
            if (table.ResName != table.name)
            {
                table.name = table.ResName;
                AssetDatabase.RenameAsset(path, table.ResName);
            }

            return true;
        }

        static string GetPkgName(string path, string currentName = "")
        {
            while (true)
            {
                if (path.Replace("\\", "/").Contains(UIConst.ResPath))
                {
                    var parentInfo = Directory.GetParent(path);
                    if (parentInfo == null)
                        return currentName;

                    if (parentInfo.Name == UIConst.ProjectName)
                        return currentName;

                    path = parentInfo.FullName;
                    currentName = parentInfo.Name;
                }
                else
                {
                    return null;
                }
            }
        }

        //收集所有公共组件
        internal static void RefreshChildTable(UITable table)
        {
            table.ChildTables.Clear();
            AddTableFrom(ref table.ChildTables, table.transform);
            CheckAddUITable(ref table.ChildTables, table);
        }

        //如果自己是panel 则还需要额外检查 是不是把自己的view给收集进去了
        static void CheckAddUITable(ref List<UITable> tables, UITable table)
        {
            if (table.UICodeType == UIType.Panel || table.IsSplitData)
            {
                for (var i = tables.Count - 1; i >= 0; i--)
                {
                    var targetTable = tables[i];
                    var parent = (RectTransform)targetTable.gameObject.transform.parent;
                    var parentName = parent.name;
                    //这里使用的是强判断 如果使用|| 可以弱判断根据需求  如果遵守View规则是没有问题的
                    if (parentName.Contains(UIConst.ParentName) && parentName.Contains(targetTable.gameObject.name))
                    {
                        //常驻View 不需要移除
                        if (table.PanelSplitData.ViewTabsStatic.Contains(parent))
                            break;
                        tables.RemoveAt(i);
                    }
                }
            }
        }

        static void AddTableFrom(ref List<UITable> tableList, Transform transform)
        {
            var childCount = transform.childCount;
            if (childCount == 0)
                return;

            for (var i = childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                var table = child.GetComponent<UITable>();

                if (table == null)
                {
                    AddTableFrom(ref tableList, child);
                    continue;
                }

                if (table.PkgName.IsEmpty() || table.ResName.IsEmpty())
                    continue;

                if (table.UICodeType == UIType.Panel)
                {
                    Debug.LogError($"{transform.name} 公共组件嵌套了 其他面板 这是不允许的 {table.ResName} 已跳过忽略");
                    continue;
                }

                table.name = Regex.Replace(table.name, NameUtility.NameRegex, "");
                tableList.Add(table);
            }
        }
    }
}
#endif