using System.Collections.Generic;

namespace YIUIFramework
{
    internal static partial class LoadHelper
    {
        private static string s_NullPkgName = "Default";

        private static readonly Dictionary<string, Dictionary<string, LoadHandle>> m_AllLoadDic = new Dictionary<string, Dictionary<string, LoadHandle>>();

        internal static LoadHandle GetLoad(string pkgName, string resName)
        {
            if (string.IsNullOrEmpty(pkgName))
            {
                pkgName = s_NullPkgName;
            }

            if (!m_AllLoadDic.ContainsKey(pkgName))
            {
                m_AllLoadDic.Add(pkgName, new Dictionary<string, LoadHandle>());
            }

            var pkgDic = m_AllLoadDic[pkgName];

            if (pkgDic.TryGetValue(resName, out var load))
            {
                return load;
            }

            var handle = RefPool.Get<LoadHandle>();
            handle.SetGroupHandle(pkgName, resName);
            pkgDic.Add(resName, handle);

            return handle;
        }

        internal static bool PutLoad(string pkgName, string resName)
        {
            if (string.IsNullOrEmpty(pkgName))
            {
                pkgName = s_NullPkgName;
            }

            if (!m_AllLoadDic.ContainsKey(pkgName))
            {
                return false;
            }

            var pkgDic = m_AllLoadDic[pkgName];

            if (!pkgDic.ContainsKey(resName))
            {
                return false;
            }

            var handle = pkgDic[resName];
            pkgDic.Remove(resName);
            RemoveLoadHandle(handle);
            RefPool.Put(handle);
            return true;
        }

        internal static void PutAll()
        {
            foreach (var pkgDic in m_AllLoadDic.Values)
            {
                foreach (var handle in pkgDic.Values)
                {
                    RefPool.Put(handle);
                }
            }

            m_AllLoadDic.Clear();
            m_ObjLoadHandle.Clear();
        }
    }
}