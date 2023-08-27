using System.Collections.Generic;

namespace YIUIFramework
{
    public abstract partial class BasePanel
    {
        private readonly HashSet<string> m_ViewOpening = new HashSet<string>();

        private void AddOpening(string name)
        {
            m_ViewOpening.Add(name);
        }

        private void RemoveOpening(string name)
        {
            m_ViewOpening.Remove(name);
        }

        public bool ViewIsOpening(string name)
        {
            return m_ViewOpening.Contains(name);
        }
    }
}