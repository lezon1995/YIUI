﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public partial class PanelMgr
    {
        /// <summary>
        /// 所有已经打开过的UI
        /// K = C#文件名
        /// 主要是作为缓存PanelInfo
        /// </summary>
        private readonly Dictionary<string, PanelInfo> m_PanelCfgMap = new Dictionary<string, PanelInfo>();

        private readonly HashSet<string> m_PanelOpening = new HashSet<string>();


        /// <summary>
        /// 获取PanelInfo
        /// 没有则创建  相当于一个打开过了 UI基础配置档
        /// 这个根据BindVo创建  为什么没有直接用VO  因为里面有Panel 实例对象
        /// 这个k 根据resName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private PanelInfo GetPanelInfo<T>() where T : BasePanel
        {
            var type = typeof(T);
            var data = UIBindHelper.GetBindVoByType(type);
            if (data == null) return null;
            var vo = data.Value;
            var name = type.Name;

            if (!m_PanelCfgMap.ContainsKey(name))
            {
                var info = new PanelInfo()
                {
                    Name = name,
                    PkgName = vo.PkgName,
                    ResName = vo.ResName,
                };

                m_PanelCfgMap.Add(name, info);
            }

            return m_PanelCfgMap[name];
        }

        /// <summary>
        /// 获取UI名称 用字符串开界面 不用类型 减少GC
        /// 另外也方便之后有可能需要的扩展 字符串会更好使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private string GetPanelName<T>() where T : BasePanel
        {
            var panelInfo = GetPanelInfo<T>();
            return panelInfo?.Name;
        }

        /// <summary>
        /// 打开之前
        /// </summary>
        private async UniTask OpenPanelBefore(PanelInfo info)
        {
            if (!info.UIBasePanel.WindowFitstOpen)
            {
                await AddUICloseElse(info);
            }
        }

        /// <summary>
        /// 打开之后
        /// </summary>
        private async UniTask<BasePanel> OpenPanelAfter(PanelInfo info, bool success)
        {
            if (success)
            {
                if (info.UIBasePanel.WindowFitstOpen)
                {
                    await AddUICloseElse(info);
                }
            }
            else
            {
                info.UIBasePanel.Close();
            }

            return info.UIBasePanel;
        }

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