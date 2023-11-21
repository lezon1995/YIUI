using UnityEngine;
using UnityEngine.UI;

namespace YIUIFramework
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    public sealed class UIMaterialEffect : MonoBehaviour, IMaterialModifier
    {
        private Graphic graphic;
        private UIEffectMaterialKey materialKey;
        private Material material;

        internal UIEffectMaterialKey MaterialKey
        {
            get { return materialKey; }

            set
            {
                if (!materialKey.Equals(value))
                {
                    materialKey = value;
                    if (material)
                    {
                        UIEffectMaterials.Free(material);
                        material = null;
                    }
                }
            }
        }

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            Material usedMaterial = baseMaterial;
            if (enabled)
            {
                if (material == null)
                {
                    material = UIEffectMaterials.Get(materialKey);
                }

                if (material)
                {
                    usedMaterial = material;
                }
            }

            var maskable = graphic as MaskableGraphic;
            if (maskable)
            {
                return maskable.GetModifiedMaterial(usedMaterial);
            }

            return usedMaterial;
        }

        internal void MarkDirty()
        {
            if (graphic)
            {
                graphic.SetMaterialDirty();
            }
        }

        private void Awake()
        {
            graphic = GetComponent<Graphic>();
        }

        private void OnEnable()
        {
            if (graphic)
            {
                graphic.SetMaterialDirty();
            }
        }

        private void OnDisable()
        {
            if (graphic)
            {
                graphic.SetMaterialDirty();
            }
        }
    }
}