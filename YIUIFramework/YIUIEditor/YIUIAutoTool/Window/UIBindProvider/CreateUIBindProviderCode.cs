#if UNITY_EDITOR
namespace YIUIFramework.Editor
{
    public class CreateUIBindProviderCode : BaseTemplate
    {
        protected override string EventName => "UI反射动态码";

        protected override bool Cover => true;

        protected override bool AutoRefresh => true;

        protected override bool ShowTips => false;

        public CreateUIBindProviderCode(out bool result, string authorName, UIBindProviderData codeData) : base(authorName)
        {
            var path = $"{UIStaticHelper.UIGenerationPath}/{codeData.Name}.cs";
            var template = $"{UIStaticHelper.UITemplatePath}/UIBindProviderTemplate.txt";
            CreateVo = new CreateVo(template, path);

            ValueDic["Count"] = codeData.Count.ToString();
            ValueDic["Content"] = codeData.Content;

            result = CreateNewFile();
        }
    }
}
#endif