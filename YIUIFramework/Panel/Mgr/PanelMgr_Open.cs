using System.Collections.Generic;
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
        readonly Dictionary<string, PanelInfo> panelInfos = new Dictionary<string, PanelInfo>();
        readonly HashSet<string> openings = new HashSet<string>();

        /// <summary>
        /// 获取PanelInfo
        /// 没有则创建  相当于一个打开过了 UI基础配置档
        /// 这个根据BindVo创建  为什么没有直接用VO  因为里面有Panel 实例对象
        /// 这个k 根据resName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool TryGetPanelInfo<T>(out PanelInfo panelInfo) where T : UIPanel
        {
            var type = typeof(T);
            if (UIBindHelper.TryGetBindVo(type, out var vo))
            {
                var name = type.Name;
                if (panelInfos.TryGetValue(name, out panelInfo))
                {
                    return true;
                }

                panelInfo = new PanelInfo(name, vo.PkgName, vo.ResName);
                panelInfos.Add(name, panelInfo);
                return true;
            }

            panelInfo = null;
            return false;
        }

        /// <summary>
        /// 获取UI名称 用字符串开界面 不用类型 减少GC
        /// 另外也方便之后有可能需要的扩展 字符串会更好使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetPanelName<T>() where T : UIPanel
        {
            if (TryGetPanelInfo<T>(out var panelInfo))
            {
                return panelInfo.Name;
            }

            return null;
        }

        /// <summary>
        /// 打开之前
        /// </summary>
        async UniTask OpenPanelBefore(PanelInfo info)
        {
            if (!info.Panel.WindowFirstOpen)
            {
                await AddUICloseElse(info);
            }
        }

        /// <summary>
        /// 打开之后
        /// </summary>
        async UniTask<UIPanel> OpenPanelAfter(PanelInfo info, bool success)
        {
            if (success)
            {
                if (info.Panel.WindowFirstOpen)
                {
                    await AddUICloseElse(info);
                }
            }
            else
            {
                //如果打开失败直接屏蔽
                info?.Panel?.SetActive(false);
                info?.Panel?.Close();
            }

            return info.Panel;
        }

        void AddOpening(string name)
        {
            openings.Add(name);
        }

        void RemoveOpening(string name)
        {
            openings.Remove(name);
        }

        public bool PanelIsOpening(string name)
        {
            return openings.Contains(name);
        }
    }
}