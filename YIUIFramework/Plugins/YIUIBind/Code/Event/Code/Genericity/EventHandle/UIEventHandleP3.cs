using System.Collections.Generic;
using YIUIFramework;

namespace YIUIBind
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP3<P1, P2, P3>
    {
        public static readonly ObjectPool<UIEventHandleP3<P1, P2, P3>> HandlerPool = new ObjectPool<UIEventHandleP3<P1, P2, P3>>(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 3个泛型参数
    /// </summary>
    public sealed class UIEventHandleP3<P1, P2, P3>
    {
        private LinkedList<UIEventHandleP3<P1, P2, P3>> list;
        private LinkedListNode<UIEventHandleP3<P1, P2, P3>> node;
        public UIEventAction<P1, P2, P3> Action { get; private set; }

        public UIEventHandleP3()
        {
        }

        internal UIEventHandleP3<P1, P2, P3> Init(LinkedList<UIEventHandleP3<P1, P2, P3>> uiEventList, LinkedListNode<UIEventHandleP3<P1, P2, P3>> uiEventNode, UIEventAction<P1, P2, P3> uiEventAction)
        {
            list = uiEventList;
            node = uiEventNode;
            Action = uiEventAction;
            return this;
        }

        public void Dispose()
        {
            if (list == null || node == null) return;

            list.Remove(node);
            node = null;
            list = null;
        }
    }
}