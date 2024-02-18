using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 简单的窗口动画
    /// </summary>
    public static class WindowFadeAnim
    {
        private static readonly Vector3 m_AnimScale = new Vector3(1.2F, 1.2F, 1.2F);

        //淡入
        public static async UniTask In(UIBase uiBase, float time = 0.25f)
        {
            var obj = uiBase.GameObject;
            var canvasGroup = uiBase.CanvasGroup;
            if (obj == null) return;

            uiBase.SetActive(true);
            obj.transform.localScale = m_AnimScale;
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, time);
            await obj.transform.DOScale(Vector3.one, time);
        }

        //淡出
        public static async UniTask Out(UIBase uiBase, float time = 0.25f)
        {
            var obj = uiBase.GameObject;
            var canvasGroup = uiBase.CanvasGroup;
            if (obj == null) return;

            obj.transform.localScale = Vector3.one;

            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, time);
            await obj.transform.DOScale(m_AnimScale, time);

            uiBase.SetActive(false);
            obj.transform.localScale = Vector3.one;
        }
    }
}