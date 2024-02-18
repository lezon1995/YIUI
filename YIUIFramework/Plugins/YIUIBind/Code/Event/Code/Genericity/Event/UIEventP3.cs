using System;
using System.Collections.Generic;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    public class UIEventP3<P1, P2, P3> : UIEventBase, IUIEventInvoke<P1, P2, P3>
    {
        public LinkedList<UIEventHandleP3<P1, P2, P3>> Actions { get; private set; }

        public UIEventP3()
        {
        }

        public UIEventP3(string name) : base(name)
        {
        }

        void IUIEventInvoke<P1, P2, P3>.Invoke(P1 p1, P2 p2, P3 p3)
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
                    value.Action?.Invoke(p1, p2, p3);
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
                PublicUIEventP3<P1, P2, P3>.HandlerPool.Release(first.Value);
                first = Actions.First;
            }

            LinkedListPool<UIEventHandleP3<P1, P2, P3>>.Release(Actions);
            Actions = null;
            return true;
        }

        public UIEventHandleP3<P1, P2, P3> Add(UIEventAction<P1, P2, P3> callback)
        {
            if (Actions == null)
            {
                Actions = LinkedListPool<UIEventHandleP3<P1, P2, P3>>.Get();
            }

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP3<P1, P2, P3>.HandlerPool.Get();
            var node = Actions.AddLast(handler);
            return handler.Init(Actions, node, callback);
        }

        public bool Remove(UIEventHandleP3<P1, P2, P3> handle)
        {
            if (Actions == null)
            {
                Actions = LinkedListPool<UIEventHandleP3<P1, P2, P3>>.Get();
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
            return $"UIEventP3<{GetParamTypeString(0)},{GetParamTypeString(1)},{GetParamTypeString(2)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UIEventHandleP3<{GetParamTypeString(0)},{GetParamTypeString(1)},{GetParamTypeString(2)}>";
        }
#endif
    }
}