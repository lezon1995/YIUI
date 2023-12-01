using System;
using System.Collections.Generic;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    public class UIEventP1<P1> : UIEventBase, IUIEventInvoke<P1>
    {
        public LinkedList<UIEventHandleP1<P1>> UIEventDelegates { get; private set; }

        public UIEventP1()
        {
        }

        public UIEventP1(string name) : base(name)
        {
        }

        public void Invoke(P1 p1)
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
                    value.UIEventParamDelegate?.Invoke(p1);
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
                PublicUIEventP1<P1>.HandlerPool.Release(first.Value);
                first = UIEventDelegates.First;
            }

            LinkedListPool<UIEventHandleP1<P1>>.Release(UIEventDelegates);
            UIEventDelegates = null;
            return true;
        }

        public UIEventHandleP1<P1> Add(UIEventDelegate<P1> callback)
        {
            if (UIEventDelegates == null)
            {
                UIEventDelegates = LinkedListPool<UIEventHandleP1<P1>>.Get();
            }

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP1<P1>.HandlerPool.Get();
            var node = UIEventDelegates.AddLast(handler);
            return handler.Init(UIEventDelegates, node, callback);
        }

        public bool Remove(UIEventHandleP1<P1> handle)
        {
            if (UIEventDelegates == null)
            {
                UIEventDelegates = LinkedListPool<UIEventHandleP1<P1>>.Get();
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
            return $"UIEventP1<{GetParamTypeString(0)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UIEventHandleP1<{GetParamTypeString(0)}>";
        }
#endif
    }
}