using UnityEngine;
using YIUIBind;

namespace YIUIFramework
{
    public abstract partial class UIBase
    {
        public T FindComponent<T>(string comName) where T : Component
        {
            return Table.FindComponent<T>(comName);
        }

        public UIData FindData(string dataName)
        {
            return Table.FindData(dataName);
        }

        public T FindDataValue<T>(string dataName) where T : UIDataValue
        {
            return Table.FindDataValue<T>(dataName);
        }
        
        public UIEventBase FindEvent(string eventName)
        {
            return Table.FindEvent(eventName);
        }

        public T FindEvent<T>(string eventName) where T : UIEventBase
        {
            return Table.FindEvent<T>(eventName);
        }
    }
}