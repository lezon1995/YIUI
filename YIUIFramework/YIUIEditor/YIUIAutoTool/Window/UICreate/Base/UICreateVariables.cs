﻿#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using YIUIBind;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// 变量的生成
    /// </summary>
    public static class UICreateVariables
    {
        public static string Get(UIBindCDETable cdeTable)
        {
            var sb = SbPool.Get();
            cdeTable.GetOverrideConfig(sb);
            cdeTable.GetComponentTable(sb);
            cdeTable.GetDataTable(sb);
            cdeTable.GetCDETable(sb);
            cdeTable.GetEventTable(sb);
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

            sb.AppendLine();
            sb.AppendFormat("        #region Component");
            sb.AppendLine();
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

                sb.AppendFormat("        public {0} {1} {{ get; private set; }}\r\n", component.GetType(), name);
            }

            sb.AppendLine();
            sb.AppendFormat("        #endregion");
            sb.AppendLine();
        }

        static void GetDataTable(this UIBindCDETable self, StringBuilder sb)
        {
            var DataDic = self.DataDic;
            var count = DataDic.Count;
            if (count == 0)
            {
                return;
            }

            sb.AppendLine();
            sb.AppendFormat("        #region Data");
            sb.AppendLine();
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

                sb.AppendFormat("        public {0} {1} {{ get; private set; }}\r\n", dataValue.GetType(), name);
            }

            sb.AppendLine();
            sb.AppendFormat("        #endregion");
            sb.AppendLine();
        }

        static void GetEventTable(this UIBindCDETable self, StringBuilder sb)
        {
            var EventDic = self.EventDic;
            var count = EventDic.Count;
            if (count == 0)
            {
                return;
            }

            sb.AppendLine();
            sb.AppendFormat("        #region Event");
            sb.AppendLine();

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

                sb.AppendFormat("        protected {0} {1} {{ get; private set; }}\r\n", uiEvent.GetEventType(), name);
                sb.AppendFormat("        protected {0} {1} {{ get; private set; }}\r\n", uiEvent.GetEventHandleType(), $"{name}Handle");
            }

            sb.AppendLine();
            sb.AppendFormat("        #endregion");
            sb.AppendLine();
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
                if (string.IsNullOrEmpty(pkgName) || string.IsNullOrEmpty(resName))
                {
                    continue;
                }

                var newName = GetCDEUIName(name);
                if (existName.Contains(newName))
                {
                    Debug.LogError($"{self.name} 内部公共组件存在同名 请修改 {name} 当前会被忽略 {newName}");
                    continue;
                }

                existName.Add(newName);
                sb.AppendFormat("        public {0} {1} {{ get; private set; }}\r\n", $"{UIStaticHelper.UINamespace}.{resName}", newName);
            }
        }

        internal static string GetCDEUIName(string oldName)
        {
            var newName = oldName;

            if (!oldName.CheckFirstName(NameUtility.UIName))
            {
                newName = $"{NameUtility.FirstName}{NameUtility.UIName}{oldName}";
            }

            newName = Regex.Replace(newName, NameUtility.NameRegex, "");

            return newName.ChangeToBigName(NameUtility.UIName);
        }

        static void GetOverrideConfig(this UIBindCDETable self, StringBuilder sb)
        {
            switch (self.UICodeType)
            {
                case EUICodeType.Component:
                    return;
                case EUICodeType.Panel:
                    sb.AppendFormat("        public override EWindowOption            WindowOption       =>           EWindowOption.{0};\r\n", self.WindowOption.ToString().Replace(", ", "|EWindowOption."));
                    sb.AppendFormat("        public override EPanelLayer                    Layer                        =>           EPanelLayer.{0};\r\n", self.PanelLayer);
                    sb.AppendFormat("        public override EPanelOption                 PanelOption            =>           EPanelOption.{0};\r\n", self.PanelOption.ToString().Replace(", ", "|EPanelOption."));
                    sb.AppendFormat("        public override EPanelStackOption        StackOption            =>           EPanelStackOption.{0};\r\n", self.PanelStackOption);
                    sb.AppendFormat("        public override int                                    Priority                     =>           {0};\r\n", self.Priority);
                    if (self.PanelOption.Has(EPanelOption.TimeCache))
                    {
                        sb.AppendFormat("        protected override float                           CachePanelTime     => {0};\r\n\r\n", self.CachePanelTime);
                    }

                    break;
                case EUICodeType.View:
                    sb.AppendFormat("        public override EWindowOption              WindowOption       =>           EWindowOption.{0};\r\n", self.WindowOption.ToString().Replace(", ", "|EWindowOption."));
                    sb.AppendFormat("        public override EViewWindowType         ViewWindowType  =>           EViewWindowType.{0};\r\n", self.ViewWindowType);
                    sb.AppendFormat("        public override EViewStackOption           StackOption            =>           EViewStackOption.{0};\r\n", self.ViewStackOption);
                    break;
                default:
                    Debug.LogError($"新增类型未实现 {self.UICodeType}");
                    break;
            }
        }
    }
}
#endif