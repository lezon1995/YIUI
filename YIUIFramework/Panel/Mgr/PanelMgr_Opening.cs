using System.Collections.Generic;

namespace YIUIFramework
{
    public partial class PanelMgr
    {
        private readonly HashSet<string> m_PanelOpening = new HashSet<string>();

        private void AddOpening(string name)
        {
            m_PanelOpening.Add(name);
        }

        private void RemoveOpening(string name)
        {
            m_PanelOpening.Remove(name);
        }

        public bool PanelIsOpening(string name)
        {
            return m_PanelOpening.Contains(name);
        }
    }
}