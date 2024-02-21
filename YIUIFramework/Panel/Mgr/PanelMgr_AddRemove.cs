using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 加入 移除
    /// </summary>
    public partial class PanelMgr
    {
        /// <summary>
        /// 加入UI到对应层级 设置顺序 大小
        /// </summary>
        void AddUI(PanelInfo panelInfo)
        {
            var panel = panelInfo.Panel;
            var layer = panel.Layer;
            var priority = panel.Priority;
            var transform = panel.Transform;

            var layerRect = GetLayerRect(layer);
            if (layerRect == null)
            {
                layer = EPanelLayer.Bottom;
                layerRect = GetLayerRect(layer);
                Debug.LogError($"没有找到这个UILayer {layer}  强制修改为使用最低层 请检查");
            }

            var addLast = true; //放到最后 也就是最前面

            var infoList = GetLayerPanelInfoList(layer);
            var removeResult = infoList.Remove(panelInfo);
            if (removeResult)
            {
                transform.SetParent(UILayerRoot);
            }

            /*
             * 使用Unity的层级作为前后显示
             * 大的在前 小的在后
             * 所以根据优先级 从小到大排序
             * 当前优先级 >= 目标优先级时 插入
             */

            for (var i = infoList.Count - 1; i >= 0; i--)
            {
                var info = infoList[i];
                var infoPriority = info.Panel ? info.Panel.Priority : 0;

                if (i == infoList.Count - 1 && priority >= infoPriority)
                {
                    break;
                }

                if (priority >= infoPriority)
                {
                    infoList.Insert(i + 1, panelInfo);
                    transform.SetParent(layerRect);
                    transform.SetSiblingIndex(i + 1);
                    addLast = false;
                    break;
                }

                if (i <= 0)
                {
                    infoList.Insert(0, panelInfo);
                    transform.SetParent(layerRect);
                    transform.SetSiblingIndex(0);
                    addLast = false;
                    break;
                }
            }

            if (addLast)
            {
                infoList.Add(panelInfo);
                transform.SetParent(layerRect);
                transform.SetAsLastSibling();
            }

            transform.ResetToFullScreen();
            transform.ResetLocalPosAndRot();
            panel.StopCountDownDestroyPanel();
        }

        /// <summary>
        /// 移除一个UI
        /// </summary>
        void RemoveUI(PanelInfo panelInfo)
        {
            if (panelInfo.Panel == null)
            {
                Debug.LogError($"无法移除一个null panelInfo 数据 {panelInfo.ResName}");
                return;
            }

            var panel = panelInfo.Panel;
            var foreverCache = panel.PanelForeverCache;
            var timeCache = panel.PanelTimeCache;
            var layer = panel.Layer;
            RemoveLayerPanelInfo(layer, panelInfo);

            if (foreverCache || timeCache)
            {
                //缓存界面只是单纯的吧界面隐藏
                //再次被打开 如何重构界面需要自行设置
                var layerRect = GetLayerRect(EPanelLayer.Cache);
                var uiRect = panel.Transform;
                uiRect.SetParent(layerRect, false);
                panel.SetActive(false);

                if (timeCache && !foreverCache)
                {
                    //根据配置时间X秒后自动摧毁
                    //如果X秒内又被打开则可复用
                    panel.CacheTimeCountDownDestroyPanel();
                }
            }
            else
            {
                var uiObj = panel.GameObject;
                Object.Destroy(uiObj);
                panelInfo.Panel = null;
            }
        }

        internal void RemoveUIReset(string panelName)
        {
            if (TryGetPanelInfo(panelName, out var panelInfo))
            {
                panelInfo.Panel = null;
            }
        }
    }
}