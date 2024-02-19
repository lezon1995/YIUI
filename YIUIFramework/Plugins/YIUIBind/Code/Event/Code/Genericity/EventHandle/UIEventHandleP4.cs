using System.Collections.Generic;
using YIUIFramework;

namespace YIUIBind
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP4<P1, P2, P3, P4>
    {
        public static readonly ObjectPool<UIEventHandleP4<P1, P2, P3, P4>> HandlerPool = new ObjectPool<UIEventHandleP4<P1, P2, P3, P4>>(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 4个泛型参数
    /// </summary>
    public sealed class UIEventHandleP4<P1, P2, P3, P4>
    {
        LinkedList<UIEventHandleP4<P1, P2, P3, P4>> list;
        LinkedListNode<UIEventHandleP4<P1, P2, P3, P4>> node;
        public UIEventAction<P1, P2, P3, P4> Action { get; private set; }

        public UIEventHandleP4()
        {
        }

        internal UIEventHandleP4<P1, P2, P3, P4> Init(LinkedList<UIEventHandleP4<P1, P2, P3, P4>> uiEventList, LinkedListNode<UIEventHandleP4<P1, P2, P3, P4>> uiEventNode, UIEventAction<P1, P2, P3, P4> uiEventAction)
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