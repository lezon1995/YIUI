using System;
using UnityEngine;

namespace YIUIFramework
{
    internal struct UIEffectMaterialKey : IEquatable<UIEffectMaterialKey>
    {
        internal float BlurDistance;

        internal Texture2D OverlayTexture;
        internal ColorModeEnum OverlayColorMode;
        internal float OverlaySpeed;

        internal bool EnableInnerBevel;
        internal Color HighlightColor;
        internal ColorModeEnum HighlightColorMode;
        internal Color ShadowColor;
        internal ColorModeEnum ShadowColorMode;
        internal Vector2 HighlightOffset;

        internal byte GrayScale;

        public bool Equals(UIEffectMaterialKey o)
        {
            if (BlurDistance != o.BlurDistance)
            {
                return false;
            }

            if (OverlayTexture != o.OverlayTexture)
            {
                return false;
            }

            if (OverlayColorMode != o.OverlayColorMode)
            {
                return false;
            }

            if (OverlaySpeed != o.OverlaySpeed)
            {
                return false;
            }

            if (EnableInnerBevel != o.EnableInnerBevel)
            {
                return false;
            }

            if (HighlightColor != o.HighlightColor)
            {
                return false;
            }

            if (HighlightColorMode != o.HighlightColorMode)
            {
                return false;
            }

            if (ShadowColor != o.ShadowColor)
            {
                return false;
            }

            if (ShadowColorMode != o.ShadowColorMode)
            {
                return false;
            }

            if (HighlightOffset != o.HighlightOffset)
            {
                return false;
            }

            if (GrayScale != o.GrayScale)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = BlurDistance.GetHashCode();
            if (OverlayTexture != null)
            {
                hash = (397 * hash) ^ OverlayTexture.GetHashCode();
            }

            hash = (397 * hash) ^ OverlayColorMode.GetHashCode();
            hash = (397 * hash) ^ OverlaySpeed.GetHashCode();
            hash = (397 * hash) ^ EnableInnerBevel.GetHashCode();
            hash = (397 * hash) ^ HighlightColor.GetHashCode();
            hash = (397 * hash) ^ HighlightColorMode.GetHashCode();
            hash = (397 * hash) ^ ShadowColor.GetHashCode();
            hash = (397 * hash) ^ ShadowColorMode.GetHashCode();
            hash = (397 * hash) ^ HighlightOffset.GetHashCode();
            hash = (397 * hash) ^ GrayScale.GetHashCode();

            return hash;
        }

        internal Material CreateMaterial()
        {
            if (BlurDistance == 0 &&
                OverlayTexture == null &&
                !EnableInnerBevel &&
                GrayScale == 0)
            {
                return null;
            }

            var shader = Shader.Find("YIUI/UIEffect");
            if (shader == null)
            {
                Debug.LogError("Can not found shader: 'YIUI/UIEffect'");
                return null;
            }

            var material = new Material(shader);
            if (BlurDistance > 0)
            {
                material.EnableKeyword("UIEFFECT_BLUR");
                material.SetFloat("_BlurDistance", BlurDistance);
            }

            if (OverlayTexture != null)
            {
                if (OverlaySpeed > 0)
                {
                    material.EnableKeyword("UIEFFECT_OVERLAY_ANIMATION");
                    material.SetFloat("_OverlaySpeed", OverlaySpeed);
                }
                else
                {
                    material.EnableKeyword("UIEFFECT_OVERLAY");
                }

                material.SetTexture("_OverlayTex", OverlayTexture);
                material.SetInt("_OverlayColorMode", (int)OverlayColorMode);
            }

            if (EnableInnerBevel)
            {
                material.EnableKeyword("UIEFFECT_INNER_BEVEL");
                material.SetColor("_HighlightColor", HighlightColor);
                material.SetInt("_HighlightColorMode", (int)HighlightColorMode);
                material.SetColor("_ShadowColor", ShadowColor);
                material.SetInt("_ShadowColorMode", (int)ShadowColorMode);
                material.SetVector("_HighlightOffset", HighlightOffset);
            }

            if (GrayScale > 0)
            {
                if (GrayScale == 255)
                {
                    material.EnableKeyword("UIEFFECT_GRAYSCALE");
                }
                else
                {
                    material.EnableKeyword("UIEFFECT_GRAYSCALE_LERP");
                    material.SetFloat("_GrayLerp", 1.0f - (GrayScale / 255.0f));
                }
            }

            return material;
        }
    }
}