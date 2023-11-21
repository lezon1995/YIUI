using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YIUIFramework
{
    internal static class UIEffectMaterials
    {
        private static Dictionary<UIEffectMaterialKey, MaterialRef> materials =
            new Dictionary<UIEffectMaterialKey, MaterialRef>();

        private static Dictionary<Material, UIEffectMaterialKey> lookup =
            new Dictionary<Material, UIEffectMaterialKey>();

        internal static Material Get(UIEffectMaterialKey key)
        {
            MaterialRef matRef;
            if (materials.TryGetValue(key, out matRef))
            {
                ++matRef.RefCount;
                return matRef.Material;
            }

            var material = key.CreateMaterial();
            if (material == null)
            {
                return null;
            }

            materials.Add(key, new MaterialRef(material));
            lookup.Add(material, key);

            return material;
        }

        internal static void Free(Material material)
        {
            UIEffectMaterialKey key;
            if (!lookup.TryGetValue(material, out key))
            {
                Debug.LogError("Can not find the material key.");
                return;
            }

            MaterialRef matRef;
            if (!materials.TryGetValue(key, out matRef))
            {
                Debug.LogError("Can not find the material reference.");
                return;
            }

            if (--matRef.RefCount <= 0)
            {
                matRef.Material.SafeDestroySelf();
                materials.Remove(key);
                lookup.Remove(material);
            }
        }

#if UNITY_EDITOR
        private static void ClearCache()
        {
            materials.Clear();
            lookup.Clear();
            SceneView.RepaintAll();
        }
#endif

        private class MaterialKeyComparer : IEqualityComparer<UIEffectMaterialKey>
        {
            public bool Equals(UIEffectMaterialKey x, UIEffectMaterialKey y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(UIEffectMaterialKey obj)
            {
                return obj.GetHashCode();
            }
        }

        private class MaterialRef
        {
            private Material material;
            private int refCount;

            public MaterialRef(Material _material)
            {
                material = _material;
                refCount = 1;
            }

            public Material Material
            {
                get { return material; }
            }

            public int RefCount
            {
                get { return refCount; }
                set { refCount = value; }
            }
        }
    }
}