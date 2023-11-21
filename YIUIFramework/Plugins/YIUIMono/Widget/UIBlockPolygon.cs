using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIFramework
{
    /// <summary>
    /// 图像与多边形块
    /// </summary>
    [RequireComponent(typeof(PolygonCollider2D))]
    public sealed class UIBlockPolygon : Graphic, ICanvasRaycastFilter
    {
        public override bool raycastTarget
        {
            get { return true; }
            set { }
        }

        private PolygonCollider2D polygon;

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        public override Texture mainTexture
        {
            get { return null; }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        public override Material materialForRendering
        {
            get { return null; }
        }

        private PolygonCollider2D Polygon
        {
            get
            {
                if (polygon == null)
                {
                    polygon = GetComponent<PolygonCollider2D>();
                    Physics2D.Simulate(0);
                }

                return polygon;
            }
        }

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (eventCamera)
            {
                Vector3 worldPoint;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out worldPoint))
                {
                    return Polygon.OverlapPoint(worldPoint);
                }

                return false;
            }

            return Polygon.OverlapPoint(screenPoint);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        public override void SetAllDirty()
        {
        }

        public override void SetLayoutDirty()
        {
        }

        public override void SetVerticesDirty()
        {
        }

        public override void SetMaterialDirty()
        {
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            transform.localPosition = Vector3.zero;
            float w = (rectTransform.sizeDelta.x * 0.5f) + 0.1f;
            float h = (rectTransform.sizeDelta.y * 0.5f) + 0.1f;
            Polygon.points = new[]
            {
                new Vector2(-w, -h),
                new Vector2(w, -h),
                new Vector2(w, h),
                new Vector2(-w, h)
            };
        }
#endif
    }
}