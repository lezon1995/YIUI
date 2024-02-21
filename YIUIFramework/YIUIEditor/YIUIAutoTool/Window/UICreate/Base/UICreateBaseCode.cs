#if UNITY_EDITOR

namespace YIUIFramework.Editor
{
    public class UICreateBaseCode : BaseTemplate
    {
        string m_EventName = "UI核心代码创建";
        protected override string EventName => m_EventName;

        protected override bool Cover => true;

        bool m_AutoRefresh;
        protected override bool AutoRefresh => m_AutoRefresh;

        bool m_ShowTips;
        protected override bool ShowTips => m_ShowTips;

        public UICreateBaseCode(out bool result, string authorName, UICreateBaseData codeData) : base(authorName)
        {
            var path = $"{UIConst.GenPath}/{codeData.PkgName}/{codeData.ResName}Base.cs";
            var template = $"{UIConst.TemplatePath}/UICreateBaseTemplate.txt";
            CreateVo = new CreateVo(template, path);

            m_EventName = $"{codeData.ResName}Base 自动生成";
            m_AutoRefresh = codeData.AutoRefresh;
            m_ShowTips = codeData.ShowTips;
            ValueDic["Namespace"] = codeData.Namespace;
            ValueDic["PkgName"] = codeData.PkgName;
            ValueDic["ResName"] = codeData.ResName;
            ValueDic["BaseClass"] = codeData.BaseClass;
            ValueDic["Variables"] = codeData.Variables;
            ValueDic["UIBind"] = codeData.UIBind;
            ValueDic["UIUnBind"] = codeData.UIUnBind;
            ValueDic["VirtualMethod"] = codeData.VirtualMethod;
            ValueDic["PanelViewEnum"] = codeData.PanelViewEnum;

            result = CreateNewFile();
        }
    }
}
#endif