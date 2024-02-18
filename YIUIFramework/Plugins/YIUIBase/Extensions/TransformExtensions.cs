﻿using UnityEngine;

namespace YIUIFramework
{
    public static class TransformExtensions
    {
        public static void SetPosition(this Transform transform, float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }

        public static void SetLocalPosition(this Transform transform, float x, float y, float z)
        {
            transform.localPosition = new Vector3(x, y, z);
        }

        public static void SetLocalScale(this Transform transform, float x, float y, float z)
        {
            transform.localScale = new Vector3(x, y, z);
        }

        public static Transform FindHard(this Transform trans, string path)
        {
            if (path == string.Empty)
            {
                return trans;
            }

            var target = trans;

            var names = path.Split('/');
            foreach (var name in names)
            {
                bool find = false;
                foreach (Transform child in target)
                {
                    if (child.name == name)
                    {
                        target = child;
                        find = true;
                        break;
                    }
                }

                if (!find)
                {
                    target = null;
                    break;
                }
            }

            return target;
        }

        public static Transform FindByName(this Transform trans, string name)
        {
            if (trans.name == name)
            {
                return trans;
            }

            for (int i = 0; i < trans.childCount; ++i)
            {
                var child = trans.GetChild(i);
                var result = child.FindByName(name);
                if (result)
                {
                    return result;
                }
            }

            return null;
        }

        public static Transform FindChildByName(this Transform trans, string childName)
        {
            var childT = trans.Find(childName);
            if (childT)
            {
                return childT;
            }

            for (int i = 0; i < trans.childCount; i++)
            {
                var child = trans.GetChild(i);
                childT = child.FindChildByName(childName);
                if (childT)
                {
                    return childT;
                }
            }

            return null;
        }
    }
}