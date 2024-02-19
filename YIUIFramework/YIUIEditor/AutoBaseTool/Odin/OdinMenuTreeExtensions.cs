#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// 奥丁扩展
    /// </summary>
    public static class OdinMenuTree_Extensions
    {
        //仿照源码扩展的 根据 AssetImporter 类型
        public static IEnumerable<OdinMenuItem> AddAllAssetImporterAtPath(this OdinMenuTree tree,
            string menuPath, string assetFolderPath, Type type, bool includeSubDirectories = false, bool flattenSubDirectories = false)
        {
            var s = assetFolderPath;
            if (s == null)
            {
                s = "";
            }

            assetFolderPath = s.TrimEnd('/') + "/";
            string lower = assetFolderPath.ToLower();
            if (!lower.StartsWith("assets/") && !lower.StartsWith("packages/"))
            {
                assetFolderPath = "Assets/" + assetFolderPath;
            }

            assetFolderPath = assetFolderPath.TrimEnd('/') + "/";
            IEnumerable<string> strings = AssetDatabase.GetAllAssetPaths().Where(
                x =>
                {
                    if (includeSubDirectories)
                    {
                        return x.StartsWith(assetFolderPath, StringComparison.InvariantCultureIgnoreCase);
                    }

                    return string.Compare(PathUtilities.GetDirectoryName(x).Trim('/'), assetFolderPath.Trim('/'), StringComparison.OrdinalIgnoreCase) == 0;
                });
            if (menuPath == null)
            {
                menuPath = "";
            }

            menuPath = menuPath.TrimStart('/');
            var result = new HashSet<OdinMenuItem>();
            foreach (string str1 in strings)
            {
                var assetImporter = AssetImporter.GetAtPath(str1);
                if (assetImporter && type.IsInstanceOfType(assetImporter))
                {
                    string withoutExtension = Path.GetFileNameWithoutExtension(str1);
                    string path = menuPath;
                    if (!flattenSubDirectories)
                    {
                        string str2 = (PathUtilities.GetDirectoryName(str1).TrimEnd('/') + "/").Substring(assetFolderPath.Length);
                        if (str2.Length != 0)
                        {
                            path = path.Trim('/') + "/" + str2;
                        }
                    }

                    path = path.Trim('/') + "/" + withoutExtension;
                    string name;
                    SplitMenuPath(path, out path, out name);
                    tree.AddMenuItemAtPath(result, path, new OdinMenuItem(tree, name, assetImporter));
                }
            }

            return result;
        }

        static void SplitMenuPath(string menuPath, out string path, out string name)
        {
            menuPath = menuPath.Trim('/');
            int length = menuPath.LastIndexOf('/');
            if (length == -1)
            {
                path = "";
                name = menuPath;
            }
            else
            {
                path = menuPath.Substring(0, length);
                name = menuPath.Substring(length + 1);
            }
        }
    }
}
#endif