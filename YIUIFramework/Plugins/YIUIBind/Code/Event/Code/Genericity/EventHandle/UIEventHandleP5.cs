using System.Collections.Generic;
using YIUIFramework;

namespace YIUIBind
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP5<P1, P2, P3, P4, P5>
    {
        public static readonly ObjectPool<UIEventHandleP5<P1, P2, P3, P4, P5>> HandlerPool = new ObjectPool<UIEventHandleP5<P1, P2, P3, P4, P5>>(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 5个泛型参数
    /// </summary>
    public sealed class UIEventHandleP5<P1, P2, P3, P4, P5>
    {
        LinkedList<UIEventHandleP5<P1, P2, P3, P4, P5>> list;
        LinkedListNode<UIEventHandleP5<P1, P2, P3, P4, P5>> node;
        public UIEventAction<P1, P2, P3, P4, P5> Action { get; private set; }

        public UIEventHandleP5()
        {
        }

        internal UIEventHandleP5<P1, P2, P3, P4, P5> Init(LinkedList<UIEventHandleP5<P1, P2, P3, P4, P5>> uiEventList, LinkedListNode<UIEventHandleP5<P1, P2, P3, P4, P5>> uiEventNode, UIEventAction<P1, P2, P3, P4, P5> uiEventAction)
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