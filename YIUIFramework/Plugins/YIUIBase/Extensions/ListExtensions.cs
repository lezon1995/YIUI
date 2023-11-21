using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace YIUIFramework
{

    /// <summary>
    /// <see cref="List"/>.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// 洗牌
        /// </summary>
        public static void Shuffle<T>(this IList<T> array)
        {
            for (int i = 0; i < array.Count; ++i)
            {
                var randIdx = Random.Range(0, array.Count);
                (array[randIdx], array[i]) = (array[i], array[randIdx]);
            }
        }

        /// <summary>
        /// 删除重复元素。
        /// </summary>
        public static void RemoveDuplicate<T>(this List<T> list)
        {
            var lookup = new Dictionary<T, int>();
            foreach (var i in list)
            {
                if (!lookup.TryGetValue(i, out _))
                {
                    lookup.Add(i, 0);
                }
            }

            list.Clear();
            foreach (var i in lookup)
            {
                list.Add(i.Key);
            }
        }
    }
}
