using Sirenix.OdinInspector;

namespace YIUIFramework
{
    //一个项目不可能随时换项目路径 这里就是强制设置的只可读 初始化项目的时候手动改这个一次就可以了
    /// <summary>
    /// UI静态助手
    /// </summary>
    public static class UIConst
    {
        [LabelText("YIUI根目录名称")]
        public const string ProjectName = "YIUI";

        [LabelText("YIUI项目命名空间")]
        public const string Namespace = "YIUI"; //所有生成文件的命名空间

        [LabelText("YIUI项目编辑器资源路径")]
        public const string EditorPath = "Assets/Editor/" + ProjectName; //编辑器才会用到的资源
        
        [LabelText("YIUI项目资源路径")]
        public const string ResPath = "Assets/LocalResources/" + ProjectName; //玩家的预设/图片等资源存放的地方
        
        [LabelText("YIUI项目脚本路径")]
        public const string GenPath = "Assets/Scripts/YIUIGen"; //自动生成的代码

        [LabelText("YIUI项目自定义脚本路径")]
        public const string CodePath = "Assets/Scripts/" + ProjectName; //玩家可编写的核心代码部分

        [LabelText("YIUI框架所处位置路径")]
        public const string FrameworkPath = "Assets/YIUI/YIUIFramework";

        [LabelText("YIUI项目代码模板路径")]
        public const string TemplatePath = FrameworkPath + "/YIUIEditor/YIUIAutoTool/Template";

        public const string RootPrefabPath = FrameworkPath + "/YIUIEditor/UIRootPrefab/" + PanelMgr.UIRootName + ".prefab";

        public const string UIBaseName               = nameof(UIBase);
        public const string BasePanelName          = nameof(UIPanel);
        public const string BaseViewName           = nameof(UIView);
        public const string BaseComponentName      = nameof(UIComponent);
        public const string PanelName              = "Panel";
        public const string ViewName               = "View";
        public const string ParentName             = "Parent";
        public const string Prefabs                = "Prefabs";
        public const string PrefabsCN              = "预制";
        public const string Sprites                = "Sprites";
        public const string SpritesCN              = "精灵";
        public const string Atlas                  = "Atlas";
        public const string AtlasCN                = "图集";
        public const string Source                 = "Source";
        public const string SourceCN               = "源文件";
        public const string AtlasIgnore            = "AtlasIgnore"; //图集忽略文件夹名称
        public const string SpritesAtlas1          = "Atlas1";      //图集1 不需要华丽的取名 每个包内的自定义图集就按顺序就好 当然你也可以自定义其他
        public const string ViewTabsName      = "ViewTabs";
        public const string ViewPopupsName = "ViewPopups";
        public const string YIUIPanelSourceName    = ProjectName + PanelName + Source;
        public const string PanelSourceName        = PanelName + Source;
        public const string YIUIViewName           = ProjectName + ViewName;
        public const string ViewParentName         = ViewName + ParentName;
        public const string YIUIViewParentName     = ProjectName + ViewName + ParentName;
    }
}