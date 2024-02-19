﻿#if UNITY_EDITOR

namespace YIUIFramework.Editor
{
    public class UICreateViewCode : BaseTemplate
    {
        string m_EventName = "UI继承View代码创建";
        protected override string EventName => m_EventName;

        protected override bool Cover => false;

        bool m_AutoRefresh;
        protected override bool AutoRefresh => m_AutoRefresh;

        bool m_ShowTips;
        protected override bool ShowTips => m_ShowTips;

        public UICreateViewCode(out bool result, string authorName, UICreateViewData codeData) : base(authorName)
        {
            var path = $"{UIStaticHelper.UICodeScriptsPath}/{codeData.PkgName}/{codeData.ResName}.cs";
            var template = $"{UIStaticHelper.UITemplatePath}/UICreateViewTemplate.txt";
            CreateVo = new CreateVo(template, path);

            m_EventName = $"{codeData.ResName} 继承 {codeData.ResName}Base 创建";
            m_AutoRefresh = codeData.AutoRefresh;
            m_ShowTips = codeData.ShowTips;
            ValueDic["Namespace"] = codeData.Namespace;
            ValueDic["PkgName"] = codeData.PkgName;
            ValueDic["ResName"] = codeData.ResName;

            if (!TemplateEngine.FileExists(CreateVo.SavePath))
            {
                result = CreateNewFile();
            }

            if (codeData.OverrideDic == null)
            {
                result = true;
                return;
            }

            result = OverrideCheckCodeFile(codeData.OverrideDic);
        }
    }
}
#endif