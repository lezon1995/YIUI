using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 用于记录原始层，并在渲染器中可见。
    /// </summary>
    public sealed class UI3DDisplayRecord : MonoBehaviour
    {
        private int m_Layer;
        private bool m_Visible;
        private Renderer m_AttachRenderer;
        private UI3DDisplayCamera m_ShowCamera;

        internal void Initialize(Renderer _renderer, UI3DDisplayCamera _camera)
        {
            m_AttachRenderer = _renderer;
            m_ShowCamera = _camera;
            m_Layer = _renderer.gameObject.layer;
            m_Visible = _renderer.enabled;
        }

        private static bool IsParentOf(Transform obj, Transform parent)
        {
            while (true)
            {
                if (obj == parent)
                {
                    return true;
                }

                if (obj.parent == null)
                {
                    return false;
                }

                obj = obj.parent;
            }
        }

        private void OnTransformParentChanged()
        {
            if (m_ShowCamera == null || !IsParentOf(transform, m_ShowCamera.transform))
            {
                this.SafeDestroySelf();
            }
        }

        private void OnDestroy()
        {
            if (m_AttachRenderer)
            {
                m_AttachRenderer.enabled = m_Visible;
            }

            gameObject.layer = m_Layer;
        }
    }
}