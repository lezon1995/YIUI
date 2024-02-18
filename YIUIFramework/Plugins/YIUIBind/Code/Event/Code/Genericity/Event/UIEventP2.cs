using System;
using System.Collections.Generic;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    public class UIEventP2<P1, P2> : UIEventBase, IUIEventInvoke<P1, P2>
    {
        public LinkedList<UIEventHandleP2<P1, P2>> Actions { get; private set; }

        public UIEventP2()
        {
        }

        public UIEventP2(string name) : base(name)
        {
        }

        void IUIEventInvoke<P1, P2>.Invoke(P1 p1, P2 p2)
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
                    value.Action?.Invoke(p1, p2);
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
                PublicUIEventP2<P1, P2>.HandlerPool.Release(first.Value);
                first = Actions.First;
            }

            LinkedListPool<UIEventHandleP2<P1, P2>>.Release(Actions);
            Actions = null;
            return true;
        }

        public UIEventHandleP2<P1, P2> Add(UIEventAction<P1, P2> callback)
        {
            if (Actions == null)
            {
                Actions = LinkedListPool<UIEventHandleP2<P1, P2>>.Get();
            }

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP2<P1, P2>.HandlerPool.Get();
            var node = Actions.AddLast(handler);
            return handler.Init(Actions, node, callback);
        }

        public bool Remove(UIEventHandleP2<P1, P2> handle)
        {
            if (Actions == null)
            {
                Actions = LinkedListPool<UIEventHandleP2<P1, P2>>.Get();
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
            return $"UIEventP2<{GetParamTypeString(0)},{GetParamTypeString(1)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UIEventHandleP2<{GetParamTypeString(0)},{GetParamTypeString(1)}>";
        }
#endif
    }
}