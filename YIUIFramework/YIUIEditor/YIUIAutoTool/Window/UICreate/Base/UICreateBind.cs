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
        public static string GetBind(UIBindCDETable cdeTable)
        {
            var sb = SbPool.Get();
            cdeTable.GetComponentTable(sb);
            cdeTable.GetDataTable(sb);
            cdeTable.GetEventTable(sb);
            cdeTable.GetCDETable(sb);
            return SbPool.PutAndToStr(sb);
        }

        static void GetComponentTable(this UIBindCDETable self, StringBuilder sb)
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
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                if (component == null)
                {
                    continue;
                }

                sb.AppendFormat("            {1} = FindComponent<{0}>(\"{1}\");\r\n", component.GetType(), name);
            }
        }

        static void GetDataTable(this UIBindCDETable self, StringBuilder sb)
        {
            var DataDic = self.DataDic;
            var count = DataDic.Count;
            if (count == 0)
            {
                return;
            }

            sb.AppendFormat("            //--------------------------------------Data-----------------------------------------------------------------------------------------------------------\r\n");
            foreach (var (name, uiData) in DataDic)
            {
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                var dataValue = uiData?.DataValue;
                if (dataValue == null)
                {
                    continue;
                }

                sb.AppendFormat("            {1} = FindDataValue<{0}>(\"{1}\");\r\n", dataValue.GetType(), name);
            }
        }

        static void GetEventTable(this UIBindCDETable self, StringBuilder sb)
        {
            var EventDic = self.EventDic;
            var count = EventDic.Count;
            if (count == 0)
            {
                return;
            }

            sb.AppendFormat("            //--------------------------------------Event----------------------------------------------------------------------------------------------------------\r\n");
            foreach (var (name, uiEvent) in EventDic)
            {
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                if (uiEvent == null)
                {
                    continue;
                }

                sb.AppendFormat("            {1} = FindEvent<{0}>(\"{1}\");\r\n", uiEvent.GetEventType(), name);
                sb.AppendFormat("            {0} = {1}.Add({2});\r\n", $"{name}Handle", name, $"OnEvent{name.Replace($"{NameUtility.FirstName}{NameUtility.EventName}", "")}Action");
                sb.AppendLine();
            }
        }

        static void GetCDETable(this UIBindCDETable self, StringBuilder sb)
        {
            var tab = self.ChildTables;
            if (tab == null)
            {
                return;
            }

            var existName = new HashSet<string>();
            foreach (var table in tab)
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

                var newName = UICreateVariables.GetCDEUIName(name);
                if (existName.Contains(newName))
                {
                    Debug.LogError($"{self.name} 内部公共组件存在同名 请修改 当前会被忽略");
                    continue;
                }

                existName.Add(newName);
                sb.AppendFormat("            {0} = CDETable.FindUIBase<{1}>(\"{2}\");\r\n", newName, $"{UIStaticHelper.UINamespace}.{resName}", name);
            }
        }

        public static string GetUnBind(UIBindCDETable cdeTable)
        {
            var sb = SbPool.Get();
            cdeTable.GetUnEventTable(sb);
            return SbPool.PutAndToStr(sb);
        }

        static void GetUnEventTable(this UIBindCDETable self, StringBuilder sb)
        {
            foreach (var (name, uiEvent) in self.EventDic)
            {
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                if (uiEvent == null)
                {
                    continue;
                }

                sb.AppendFormat("            {0}.Remove({1});\r\n", name, $"{name}Handle");
            }
        }
    }
}
#endif