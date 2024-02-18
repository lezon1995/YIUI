using System;
using System.Collections.Generic;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    public class UIEventP5<P1, P2, P3, P4, P5> : UIEventBase, IUIEventInvoke<P1, P2, P3, P4, P5>
    {
        public LinkedList<UIEventHandleP5<P1, P2, P3, P4, P5>> Actions { get; private set; }

        public UIEventP5()
        {
        }

        public UIEventP5(string name) : base(name)
        {
        }

        void IUIEventInvoke<P1, P2, P3, P4, P5>.Invoke(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            if (Actions == null)
            {
                Logger.LogWarning($"{EventName} 未绑定任何事件");
                return;
            }

            var itr = Actions.First;
            while (itr != null)
            {
                var next = itr.Next;
                var value = itr.Value;
                try
                {
                    value.Action?.Invoke(p1, p2, p3, p4, p5);
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
            if (Actions == null)
            {
                return false;
            }

            var first = Actions.First;
            while (first != null)
            {
                PublicUIEventP5<P1, P2, P3, P4, P5>.HandlerPool.Release(first.Value);
                first = Actions.First;
            }

            LinkedListPool<UIEventHandleP5<P1, P2, P3, P4, P5>>.Release(Actions);
            Actions = null;
            return true;
        }

        public UIEventHandleP5<P1, P2, P3, P4, P5> Add(UIEventAction<P1, P2, P3, P4, P5> callback)
        {
            if (Actions == null)
            {
                Actions = LinkedListPool<UIEventHandleP5<P1, P2, P3, P4, P5>>.Get();
            }

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP5<P1, P2, P3, P4, P5>.HandlerPool.Get();
            var node = Actions.AddLast(handler);
            return handler.Init(Actions, node, callback);
        }

        public bool Remove(UIEventHandleP5<P1, P2, P3, P4, P5> handle)
        {
            if (Actions == null)
            {
                Actions = LinkedListPool<UIEventHandleP5<P1, P2, P3, P4, P5>>.Get();
            }

            if (handle == null)
            {
                Logger.LogError($"{EventName} UIEventParamHandle == null");
                return false;
            }

            return Actions.Remove(handle);
        }

#if UNITY_EDITOR
        public override string GetEventType()
        {
            return $"UIEventP5<{GetParamTypeString(0)},{GetParamTypeString(1)},{GetParamTypeString(2)},{GetParamTypeString(3)},{GetParamTypeString(4)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UIEventHandleP5<{GetParamTypeString(0)},{GetParamTypeString(1)},{GetParamTypeString(2)},{GetParamTypeString(3)},{GetParamTypeString(4)}>";
        }
#endif
    }
}