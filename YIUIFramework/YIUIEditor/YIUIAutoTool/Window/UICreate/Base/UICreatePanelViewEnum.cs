#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace YIUIFramework.Editor
{
    /// <summary>
    /// 界面的枚举
    /// </summary>
    public static class UICreatePanelViewEnum
    {
        public static string Get(UITable table)
        {
            var sb = SbPool.Get();
            table.GetViewEnums(sb);
            return SbPool.PutAndToStr(sb);
        }

        static void GetViewEnums(this UITable self, StringBuilder sb)
        {
            var splitData = self.PanelSplitData;
            if (splitData == null)
            {
                return;
            }

            if (!splitData.ShowCreatePanelViewEnum())
            {
                return;
            }

            var index = 1;

            sb.AppendFormat("    public enum E{0}View\r\n    {{\r\n", self.name);
            AddViewEnum(splitData.ViewTabsStatic, sb, ref index);
            AddViewEnum(splitData.ViewTabs, sb, ref index);
            AddViewEnum(splitData.ViewPopups, sb, ref index);
            sb.Append("    }");
        }

        static void AddViewEnum(List<RectTransform> viewList, StringBuilder sb, ref int index)
        {
            foreach (var transform in viewList)
            {
                var viewName = transform.name.Replace(UIConst.ParentName, "");
                sb.AppendFormat("        {0} = {1},\r\n", viewName, index);
                index++;
            }
        }
    }
}
#endif