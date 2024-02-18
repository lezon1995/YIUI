using System;
using System.Collections.Generic;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    public class UIEventP0 : UIEventBase, IUIEventInvoke
    {
        public LinkedList<UIEventHandleP0> Actions { get; private set; }

        public UIEventP0()
        {
        }

        public UIEventP0(string name) : base(name)
        {
        }

        void IUIEventInvoke.Invoke()
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
                    value.Action?.Invoke();
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
                PublicUIEventP0.HandlerPool.Release(first.Value);
                first = Actions.First;
            }

            LinkedListPool<UIEventHandleP0>.Release(Actions);
            Actions = null;
            return true;
        }

        public UIEventHandleP0 Add(UIEventAction callback)
        {
            if (Actions == null)
            {
                Actions = LinkedListPool<UIEventHandleP0>.Get();
            }

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP0.HandlerPool.Get();
            var node = Actions.AddLast(handler);
            return handler.Init(Actions, node, callback);
        }

        public bool Remove(UIEventHandleP0 handle)
        {
            if (Actions == null)
            {
                Actions = LinkedListPool<UIEventHandleP0>.Get();
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
            return "UIEventP0";
        }

        public override string GetEventHandleType()
        {
            return "UIEventHandleP0";
        }
#endif
    }
}