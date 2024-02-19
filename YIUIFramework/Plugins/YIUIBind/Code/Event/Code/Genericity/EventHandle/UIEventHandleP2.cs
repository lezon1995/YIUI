using System.Collections.Generic;
using YIUIFramework;

namespace YIUIBind
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP2<P1, P2>
    {
        public static readonly ObjectPool<UIEventHandleP2<P1, P2>> HandlerPool = new ObjectPool<UIEventHandleP2<P1, P2>>(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 2个泛型参数
    /// </summary>
    public sealed class UIEventHandleP2<P1, P2>
    {
        LinkedList<UIEventHandleP2<P1, P2>> list;
        LinkedListNode<UIEventHandleP2<P1, P2>> node;
        public UIEventAction<P1, P2> Action { get; private set; }

        public UIEventHandleP2()
        {
        }

        internal UIEventHandleP2<P1, P2> Init(LinkedList<UIEventHandleP2<P1, P2>> uiEventList, LinkedListNode<UIEventHandleP2<P1, P2>> uiEventNode, UIEventAction<P1, P2> uiEventAction)
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