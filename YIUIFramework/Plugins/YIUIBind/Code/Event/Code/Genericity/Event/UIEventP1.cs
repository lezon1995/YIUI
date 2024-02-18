using System;
using System.Collections.Generic;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    public class UIEventP1<P1> : UIEventBase, IUIEventInvoke<P1>
    {
        public LinkedList<UIEventHandleP1<P1>> Actions { get; private set; }

        public UIEventP1()
        {
        }

        public UIEventP1(string name) : base(name)
        {
        }

        void IUIEventInvoke<P1>.Invoke(P1 p1)
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
                    value.Action?.Invoke(p1);
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
                PublicUIEventP1<P1>.HandlerPool.Release(first.Value);
                first = Actions.First;
            }

            LinkedListPool<UIEventHandleP1<P1>>.Release(Actions);
            Actions = null;
            return true;
        }

        public UIEventHandleP1<P1> Add(UIEventAction<P1> callback)
        {
            if (Actions == null)
            {
                Actions = LinkedListPool<UIEventHandleP1<P1>>.Get();
            }

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP1<P1>.HandlerPool.Get();
            var node = Actions.AddLast(handler);
            return handler.Init(Actions, node, callback);
        }

        public bool Remove(UIEventHandleP1<P1> handle)
        {
            if (Actions == null)
            {
                Actions = LinkedListPool<UIEventHandleP1<P1>>.Get();
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
            return $"UIEventP1<{GetParamTypeString(0)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UIEventHandleP1<{GetParamTypeString(0)}>";
        }
#endif
    }
}