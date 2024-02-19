using System.Collections.Generic;
using YIUIFramework;

namespace YIUIBind
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP0
    {
        public static readonly ObjectPool<UIEventHandleP0> HandlerPool = new ObjectPool<UIEventHandleP0>(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 无参数
    /// </summary>
    public sealed class UIEventHandleP0
    {
        LinkedList<UIEventHandleP0> list;
        LinkedListNode<UIEventHandleP0> node;
        public UIEventAction Action { get; private set; }

        public UIEventHandleP0()
        {
        }

        internal UIEventHandleP0 Init(LinkedList<UIEventHandleP0> uiEventList, LinkedListNode<UIEventHandleP0> uiEventNode, UIEventAction uiEventAction)
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