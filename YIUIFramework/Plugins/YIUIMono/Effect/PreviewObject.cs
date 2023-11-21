#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 这是一个仅用于编辑器的脚本，用于在编辑模式下显示预览对象
    /// </summary>
    [ExecuteInEditMode]
    public sealed class PreviewObject : MonoBehaviour
    {
        private GameObject preview;
        private bool simulateInEditMode = true; //记录播放时间
        private float playingTime = 0.0f;
        private double lastTime = -1.0;

        /// <summary>
        /// 获取或设置一个值，该值指示是否在编辑模式下模拟。
        /// </summary>
        public bool SimulateInEditMode
        {
            get { return simulateInEditMode; }
            set { simulateInEditMode = value; }
        }

        /// <summary>
        /// 清除预览对象。
        /// </summary>
        public void ClearPreview()
        {
            if (preview != null)
            {
                var deletePreview = preview;
                preview = null;
                EditorApplication.delayCall += () => { DestroyImmediate(deletePreview); };
            }
        }

        /// <summary>
        /// 设置预览对象
        /// </summary>
        public void SetPreview(GameObject previewObj)
        {
            // Destroy the pre-preview.
            if (preview != null)
            {
                var deletePreview = preview;
                preview = null;
                EditorApplication.delayCall += () => { DestroyImmediate(deletePreview); };
            }

            // Attach the preview object.
            preview = previewObj;
            preview.tag = "EditorOnly";
            preview.transform.SetParent(transform, false);

            // Start the animation.
            if (simulateInEditMode)
            {
                var effectControl = preview.GetComponent<EffectControl>();
                if (effectControl != null)
                {
                    effectControl.SimulateInit();
                    effectControl.SimulateStart();
                }
                else
                {
                    var particleSystems = preview.GetComponentsInChildren<ParticleSystem>();
                    foreach (var ps in particleSystems)
                    {
                        ps.Simulate(0.0f, false, true);
                    }
                }
            }

            // Hide this preview.
            SetHideFlags(preview, HideFlags.DontSave);
        }

        private void Awake()
        {
            hideFlags = HideFlags.DontSave;
            EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
            {
                if (EditorApplication.isPlaying ||
                    EditorApplication.isPlayingOrWillChangePlaymode ||
                    EditorApplication.isCompiling)
                {
                    if (preview != null)
                    {
                        DestroyImmediate(preview);
                        preview = null;
                    }
                }
            };
        }

        private void OnDestroy()
        {
            if (preview != null)
            {
                DestroyImmediate(preview);
                preview = null;
            }
        }

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                return;
            }

            EditorApplication.update += UpdatePreview;
            lastTime = EditorApplication.timeSinceStartup;
            if (preview != null)
            {
                preview.SetActive(true);
            }
        }

        private void OnDisable()
        {
            if (Application.isPlaying)
            {
                return;
            }

            EditorApplication.update -= UpdatePreview;
            if (preview != null)
            {
                preview.SetActive(false);
            }
        }

        private void UpdatePreview()
        {
            if (!simulateInEditMode)
            {
                return;
            }

            var timeSinceStartup = EditorApplication.timeSinceStartup;
            var deltaTime = (float)(timeSinceStartup - lastTime);
            lastTime = timeSinceStartup;
            playingTime += deltaTime;

            if (preview == null)
            {
                return;
            }

            // Start the animation.
            var effectControl = preview.GetComponent<EffectControl>();
            if (effectControl != null)
            {
                effectControl.SimulateDelta(playingTime, deltaTime);
            }
            else
            {
                float playTime = 0.0f;
                var particleSystems = preview.GetComponentsInChildren<ParticleSystem>();
                foreach (var ps in particleSystems)
                {
                    if (playTime < ps.main.duration)
                    {
                        playTime = ps.main.duration;
                    }
                }

                foreach (var ps in particleSystems)
                {
                    ps.Simulate(deltaTime, false, false);
                }
            }
        }

        private void SetHideFlags(GameObject obj, HideFlags flags)
        {
            obj.hideFlags = flags;
            for (int i = 0; i < obj.transform.childCount; ++i)
            {
                var child = obj.transform.GetChild(i);
                SetHideFlags(child.gameObject, flags);
            }
        }
    }
}
#endif