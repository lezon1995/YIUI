namespace YIUIFramework
{
    public class PanelInfo
    {
        public UIPanel Panel { get; set; }

        // 包名
        public string PkgName { get; }

        // 资源名称 因为每个包分开 这个资源名称是有可能重复的 虽然设计上不允许  
        public string ResName { get; }

        // C#文件名 因为有可能存在Res名称与文件名不一致的问题
        public string Name { get; }

        public PanelInfo(string panelName, string pkgName, string resName)
        {
            Name = panelName;
            PkgName = pkgName;
            ResName = resName;
        }

        public static implicit operator bool(PanelInfo self)
        {
            return self != null;
        }
    }
}