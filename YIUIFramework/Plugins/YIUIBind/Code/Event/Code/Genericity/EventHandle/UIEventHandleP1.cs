using System.Collections.Generic;
using YIUIFramework;

namespace YIUIBind
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP1<P1>
    {
        public static readonly ObjectPool<UIEventHandleP1<P1>> HandlerPool = new ObjectPool<UIEventHandleP1<P1>>(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 1个泛型参数
    /// </summary>
    public sealed class UIEventHandleP1<P1>
    {
        private LinkedList<UIEventHandleP1<P1>> list;
        private LinkedListNode<UIEventHandleP1<P1>> node;
        public UIEventAction<P1> Action { get; private set; }

        public UIEventHandleP1()
        {
        }

        internal UIEventHandleP1<P1> Init(LinkedList<UIEventHandleP1<P1>> uiEventList, LinkedListNode<UIEventHandleP1<P1>> uiEventNode, UIEventAction<P1> uiEventAction)
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