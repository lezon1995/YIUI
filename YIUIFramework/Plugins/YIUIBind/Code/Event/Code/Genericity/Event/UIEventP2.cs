using System;
using System.Collections.Generic;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    public class UIEventP2<P1, P2> : UIEventBase, IUIEventInvoke<P1, P2>
    {
        public LinkedList<UIEventHandleP2<P1, P2>> UIEventDelegates { get; private set; }

        public UIEventP2()
        {
        }

        public UIEventP2(string name) : base(name)
        {
        }

        public void Invoke(P1 p1, P2 p2)
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
                    value.UIEventParamDelegate?.Invoke(p1, p2);
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
                PublicUIEventP2<P1, P2>.HandlerPool.Release(first.Value);
                first = UIEventDelegates.First;
            }

            LinkedListPool<UIEventHandleP2<P1, P2>>.Release(UIEventDelegates);
            UIEventDelegates = null;
            return true;
        }

        public UIEventHandleP2<P1, P2> Add(UIEventDelegate<P1, P2> callback)
        {
            if (UIEventDelegates == null)
            {
                UIEventDelegates = LinkedListPool<UIEventHandleP2<P1, P2>>.Get();
            }

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP2<P1, P2>.HandlerPool.Get();
            var node = UIEventDelegates.AddLast(handler);
            return handler.Init(UIEventDelegates, node, callback);
        }

        public bool Remove(UIEventHandleP2<P1, P2> handle)
        {
            if (UIEventDelegates == null)
            {
                UIEventDelegates = LinkedListPool<UIEventHandleP2<P1, P2>>.Get();
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
            return $"UIEventP2<{GetParamTypeString(0)},{GetParamTypeString(1)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UIEventHandleP2<{GetParamTypeString(0)},{GetParamTypeString(1)}>";
        }
#endif
    }
}