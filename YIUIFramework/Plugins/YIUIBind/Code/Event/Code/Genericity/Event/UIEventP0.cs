using System;
using System.Collections.Generic;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    public class UIEventP0 : UIEventBase, IUIEventInvoke
    {
        public LinkedList<UIEventHandleP0> UIEventDelegates { get; private set; }

        public UIEventP0()
        {
        }

        public UIEventP0(string name) : base(name)
        {
        }

        public void Invoke()
        {
            if (UIEventDelegates == null)
            {
                Logger.LogWarning($"{EventName} 未绑定任何事件");
                return;
            }

            var itr = UIEventDelegates.First;
            while (itr != null)
            {
                var next = itr.Next;
                var value = itr.Value;
                try
                {
                    value.UIEventParamDelegate?.Invoke();
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }

                itr = next;
            }
        }

        public override bool Clear()
        {
            if (UIEventDelegates == null)
            {
                return false;
            }

            var first = UIEventDelegates.First;
            while (first != null)
            {
                PublicUIEventP0.HandlerPool.Release(first.Value);
                first = UIEventDelegates.First;
            }

            LinkedListPool<UIEventHandleP0>.Release(UIEventDelegates);
            UIEventDelegates = null;
            return true;
        }

        public UIEventHandleP0 Add(UIEventDelegate callback)
        {
            if (UIEventDelegates == null)
            {
                UIEventDelegates = LinkedListPool<UIEventHandleP0>.Get();
            }

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP0.HandlerPool.Get();
            var node = UIEventDelegates.AddLast(handler);
            return handler.Init(UIEventDelegates, node, callback);
        }

        public bool Remove(UIEventHandleP0 handle)
        {
            if (UIEventDelegates == null)
            {
                UIEventDelegates = LinkedListPool<UIEventHandleP0>.Get();
            }

            if (handle == null)
            {
                Logger.LogError($"{EventName} UIEventParamHandle == null");
                return false;
            }

            return UIEventDelegates.Remove(handle);
        }

#if UNITY_EDITOR
        public override string GetEventType()
        {
            return "UIEventP0";
        }

        public override string GetEventHandleType()
        {
            return "UIEventHandleP0";
        }
#endif
    }
}