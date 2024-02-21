#if UNITY_EDITOR

using Sirenix.OdinInspector;

namespace YIUIFramework.Editor
{
    [HideReferenceObjectPicker]
    [HideLabel]
    public class UICreateResModule : BaseCreateModule
    {
        [LabelText("新增模块名称")]
        public string Name;

        [GUIColor(0, 1, 0)]
        [Button("创建", 30)]
        void Create()
        {
            if (UIOperationHelper.CheckUIOperation())
            {
                Create(Name);
            }
        }

        public static void Create(string createName)
        {
            if (string.IsNullOrEmpty(createName))
            {
                UnityTipsHelper.ShowError("请设定 名称");
                return;
            }

            createName = NameUtility.ToFirstUpper(createName);

            var basePath = $"{UIConst.ResPath}/{createName}";
            var prefabsPath = $"{basePath}/{UIConst.Prefabs}";
            var spritesPath = $"{basePath}/{UIConst.Sprites}";
            var spritesAtlas1Path = $"{basePath}/{UIConst.Sprites}/{UIConst.SpritesAtlas1}";
            var atlasIgnorePath = $"{basePath}/{UIConst.Sprites}/{UIConst.AtlasIgnore}";
            var atlasPath = $"{basePath}/{UIConst.Atlas}";
            var sourcePath = $"{basePath}/{UIConst.Source}";

            EditorHelper.CreateExistsDirectory(prefabsPath);
            EditorHelper.CreateExistsDirectory(spritesPath);
            // EditorHelper.CreateExistsDirectory(spritesAtlas1Path);
            // EditorHelper.CreateExistsDirectory(atlasIgnorePath);
            // EditorHelper.CreateExistsDirectory(atlasPath);
            EditorHelper.CreateExistsDirectory(sourcePath);

            MenuItemYIUIPanel.CreateYIUIPanelByPath(sourcePath, createName);

            YIUIAutoTool.CloseWindowRefresh();
        }
    }
}
#endif