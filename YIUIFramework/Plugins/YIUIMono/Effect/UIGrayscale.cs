
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YIUIFramework
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIMaterialEffect))]
    public sealed class UIGrayscale : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 255)]
        private int grayscale;

        private UIMaterialEffect materialEffect;

        public int GrayScale
        {
            get { return grayscale; }

            set
            {
                if (grayscale != value)
                {
                    grayscale = value;
                    Refresh();
                }
            }
        }

        private void Awake()
        {
            Refresh();
        }

        private void OnDestroy()
        {
            if (materialEffect != null)
            {
                var key = materialEffect.MaterialKey;
                key.GrayScale = 0;
                materialEffect.MaterialKey = key;
                materialEffect.MarkDirty();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Refresh();
        }
#endif

        private void Refresh()
        {
#if UNITY_EDITOR
            var prefabType = PrefabUtility.GetPrefabType(gameObject);
            if (prefabType == PrefabType.Prefab)
            {
                return;
            }
#endif

            if (materialEffect == null)
            {
                materialEffect = this.GetOrAddComponent<UIMaterialEffect>();
            }

            var key = materialEffect.MaterialKey;
            key.GrayScale = (byte)grayscale;
            materialEffect.MaterialKey = key;
            materialEffect.MarkDirty();
        }
    }
}