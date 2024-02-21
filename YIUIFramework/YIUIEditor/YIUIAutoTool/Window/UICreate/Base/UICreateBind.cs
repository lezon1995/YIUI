#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// 绑定与解绑
    /// </summary>
    public static class UICreateBind
    {
        public static string GetBind(UITable table)
        {
            var sb = SbPool.Get();
            table.GetComponentTable(sb);
            table.GetUITable(sb);
            return SbPool.PutAndToStr(sb);
        }

        static void GetComponentTable(this UITable self, StringBuilder sb)
        {
            var AllBindDic = self.AllBindDic;
            var count = AllBindDic.Count;
            if (count == 0)
            {
                return;
            }

            sb.AppendFormat("            //--------------------------------------Component---------------------------------------------------------------------------------------------------\r\n");
            foreach (var (name, component) in AllBindDic)
            {
                if (name.IsEmpty() || component == null)
                {
                    continue;
                }

                sb.AppendFormat("            {1} = FindComponent<{0}>(\"{1}\");\r\n", component.GetType(), name);
            }
        }

        static void GetUITable(this UITable self, StringBuilder sb)
        {
            var tables = self.ChildTables;
            if (tables == null || tables.Count == 0)
            {
                return;
            }

            var existName = new HashSet<string>();
            foreach (var table in tables)
            {
                var name = table.name;
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                var pkgName = table.PkgName;
                var resName = table.ResName;
                if (string.IsNullOrEmpty(resName))
                {
                    continue;
                }

                var newName = UICreateVariables.GetTableName(name);
                if (existName.Contains(newName))
                {
                    Debug.LogError($"{self.name} 内部公共组件存在同名 请修改 当前会被忽略");
                    continue;
                }

                existName.Add(newName);
                sb.AppendFormat("            {0} = Table.FindUIBase<{1}>(\"{2}\");\r\n", newName, $"{UIConst.Namespace}.{resName}", name);
            }
        }

        public static string GetUnBind(UITable table)
        {
            var sb = SbPool.Get();
            return SbPool.PutAndToStr(sb);
        }
    }
}
#endif