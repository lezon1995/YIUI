using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YIUIFramework
{
    public class YIUIButton : Button
    {
        public bool PointerEnter { get; private set; }
        public bool Pressed { get; private set; }

        public Action onPointerEnter { get; set; }
        public Action onPointerExit { get; set; }
        public Action onPointerDown { get; set; }
        public Action onPointerUp { get; set; }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            PointerEnter = true;
            onPointerEnter?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            PointerEnter = false;
            onPointerExit?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            Pressed = true;
            onPointerDown?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            Pressed = false;
            onPointerUp?.Invoke();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (Pressed)
            {
                Pressed = false;
                onPointerUp?.Invoke();
            }

            if (PointerEnter)
            {
                PointerEnter = false;
                onPointerExit?.Invoke();
            }
        }
    }
}