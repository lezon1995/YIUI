using System.Collections.Generic;

namespace YIUIFramework
{
    internal static partial class LoadHelper
    {
        static string defaultName = "Default";
        static Dictionary<string, Dictionary<string, LoadHandle>> handleDict = new Dictionary<string, Dictionary<string, LoadHandle>>();

        internal static LoadHandle GetLoad(string pkgName, string resName)
        {
            if (pkgName.IsEmpty())
            {
                pkgName = defaultName;
            }

            if (!handleDict.ContainsKey(pkgName))
            {
                handleDict.Add(pkgName, new Dictionary<string, LoadHandle>());
            }

            var pkgDict = handleDict[pkgName];

            if (pkgDict.TryGetValue(resName, out var load))
            {
                return load;
            }

            var handle = RefPool.Get<LoadHandle>();
            handle.SetGroupHandle(pkgName, resName);
            pkgDict.Add(resName, handle);

            return handle;
        }

        internal static bool PutLoad(string pkgName, string resName)
        {
            if (pkgName.IsEmpty())
            {
                pkgName = defaultName;
            }

            if (handleDict.TryGetValue(pkgName, out var pkgDic))
            {
                if (pkgDic.Remove(resName, out var handle))
                {
                    RemoveLoadHandle(handle);
                    RefPool.Put(handle);
                    return true;
                }

                return false;
            }

            return false;
        }

        internal static void PutAll()
        {
            foreach (var pkgDic in handleDict.Values)
            {
                foreach (var handle in pkgDic.Values)
                {
                    RefPool.Put(handle);
                }
            }

            handleDict.Clear();
            objHandles.Clear();
        }
    }
}