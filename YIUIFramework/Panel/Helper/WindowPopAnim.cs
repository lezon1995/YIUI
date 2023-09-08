using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 简单的窗口动画
    /// </summary>
    public static class WindowPopAnim
    {
        //淡入
        public static async UniTask SlideUp(UIBase uiBase, float from, float to, float time = 0.15f)
        {
            var rectTransform = uiBase.OwnerRectTransform;
            if (rectTransform == null) return;

            uiBase.SetActive(true);
            var vector2 = rectTransform.anchoredPosition;
            vector2.y = from;
            rectTransform.anchoredPosition = vector2;
            await rectTransform.DOAnchorPosY(to, time);
        }

        //淡出
        public static async UniTask SlideDown(UIBase uiBase, float from, float to, float time = 0.15f)
        {
            var rectTransform = uiBase.OwnerRectTransform;
            if (rectTransform == null) return;

            var vector2 = rectTransform.anchoredPosition;
            vector2.y = from;
            rectTransform.anchoredPosition = vector2;
            await rectTransform.DOAnchorPosY(to, time);

            uiBase.SetActive(false);
        }
    }
}