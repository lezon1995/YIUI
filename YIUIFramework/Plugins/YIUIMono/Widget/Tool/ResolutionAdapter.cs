
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YIUIFramework
{
    /// <summary>
    ///自适应不同分辨率。
    /// </summary>
    [RequireComponent(typeof(CanvasScaler))]
    [ExecuteInEditMode]
    public class ResolutionAdapter : MonoBehaviour
    {
        private Canvas canvas;
        private CanvasScaler scaler;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            if (null == canvas || !canvas.isRootCanvas)
            {
                return;
            }

            AdaptResolution();
        }

#if UNITY_EDITOR
        private void Update()
        {
            AdaptResolution();
        }

        private void OnValidate()
        {
            AdaptResolution();
        }
#endif

        private void AdaptResolution()
        {
#if UNITY_EDITOR
            var prefabType = PrefabUtility.GetPrefabAssetType(gameObject);
            if (prefabType == PrefabAssetType.Regular)
            {
                return;
            }
#endif

            if (null == scaler)
            {
                scaler = GetComponent<CanvasScaler>();
            }

            var radio = (float)Screen.width / Screen.height;
            var refRadio = scaler.referenceResolution.x / scaler.referenceResolution.y;
            if (radio >= refRadio)
            {
                scaler.matchWidthOrHeight = 1.0f;
            }
            else
            {
                scaler.matchWidthOrHeight = 0.0f;
            }
        }
    }
}