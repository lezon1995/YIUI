using UnityEngine;

namespace YIUIFramework
{
    public sealed class UI3DDisplayCamera : MonoBehaviour
    {
        private GameObject m_ShowObject;

        internal GameObject ShowObject
        {
            get => m_ShowObject;
            set
            {
                if (m_ShowObject && m_ShowObject != value)
                {
                    ResetRenderer(m_ShowObject.transform);
                }

                m_ShowObject = value;
                if (value)
                {
                    SetupRenderer(value.transform);
                }
            }
        }

        private int m_ShowLayer;

        internal int ShowLayer
        {
            get => m_ShowLayer;
            set
            {
                if (m_ShowLayer != value)
                {
                    m_ShowLayer = value;
                    if (ShowObject)
                    {
                        SetupRenderer(ShowObject.transform);
                    }
                }
            }
        }

        private static void ResetRenderer(Transform trans)
        {
            var renderers = ListPool<Renderer>.Get();
            trans.GetComponentsInChildren(true, renderers);
            foreach (var renderer in renderers)
            {
                renderer.gameObject.layer = 0;
            }

            ListPool<Renderer>.Put(renderers);
        }

        public void SetupRenderer(Transform trans)
        {
            if (ShowObject == null) return;

            var renderers = ListPool<Renderer>.Get();
            trans.GetComponentsInChildren(true, renderers);
            foreach (var renderer in renderers)
            {
                renderer.gameObject.layer = ShowLayer;
            }

            ListPool<Renderer>.Put(renderers);
        }

        private void OnEnable()
        {
            if (ShowObject)
            {
                SetupRenderer(ShowObject.transform);
            }
        }

        private void OnDisable()
        {
            if (ShowObject)
            {
                ResetRenderer(ShowObject.transform);
            }
        }
    }
}