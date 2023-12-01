using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YIUIFramework
{
    public class YIUIButton : Button
    {
        public bool PointerEnter { get; private set; }
        public bool Pressed { get; private set; }

        public Action OnPointerEnterAction { get; set; }
        public Action OnPointerExitAction { get; set; }
        public Action OnPointerDownAction { get; set; }
        public Action OnPointerUpAction { get; set; }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            PointerEnter = true;
            OnPointerEnterAction?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            PointerEnter = false;
            OnPointerExitAction?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            Pressed = true;
            OnPointerDownAction?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            Pressed = false;
            OnPointerUpAction?.Invoke();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (Pressed)
            {
                Pressed = false;
                OnPointerUpAction?.Invoke();
            }

            if (PointerEnter)
            {
                PointerEnter = false;
                OnPointerExitAction?.Invoke();
            }
        }
    }
}