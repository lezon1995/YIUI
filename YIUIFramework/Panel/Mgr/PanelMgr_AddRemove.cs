﻿namespace YIUIFramework
{
    /// <summary>
    /// 加入 移除
    /// </summary>
    public partial class PanelMgr
    {
        /// <summary>
        /// 加入UI到对应层级 设置顺序 大小
        /// </summary>
        private void AddUI(PanelInfo panelInfo)
        {
            var panel = panelInfo.Panel;
            var panelLayer = panel.Layer;
            var priority = panel.Priority;
            var uiRect = panel.OwnerRectTransform;

            var layerRect = GetLayerRect(panelLayer);
            if (layerRect == null)
            {
                panelLayer = EPanelLayer.Bottom;
                layerRect = GetLayerRect(panelLayer);
                UnityEngine.Debug.LogError($"没有找到这个UILayer {panelLayer}  强制修改为使用最低层 请检查");
            }

            var addLast = true; //放到最后 也就是最前面

            var infoList = GetLayerPanelInfoList(panelLayer);
            var removeResult = infoList.Remove(panelInfo);
            if (removeResult)
            {
                uiRect.SetParent(UILayerRoot);
            }

            /*
             * 使用Unity的层级作为前后显示
             * 大的在前 小的在后
             * 所以根据优先级 从小到大排序
             * 当前优先级 >= 目标优先级时 插入
             */

            for (var i = infoList.Count - 1; i >= 0;)
            {
                var info = infoList[i];
                var infoPriority = info.Panel != null ? info.Panel.Priority : 0;

                //当前优先级比最大的都还大 那么直接放到最前面
                if (priority >= infoPriority)
                {
                    break;
                }

                infoList.Insert(i, panelInfo);
                uiRect.SetParent(layerRect);
                uiRect.SetSiblingIndex(i);
                addLast = false;
                break;
            }


            if (addLast)
            {
                infoList.Add(panelInfo);
                uiRect.SetParent(layerRect);
                uiRect.SetAsLastSibling();
            }

            uiRect.ResetToFullScreen();
            uiRect.ResetLocalPosAndRot();

            if (panel.PanelTimeCache)
            {
                panel.StopCountDownDestroyPanel();
            }
        }

        /// <summary>
        /// 移除一个UI
        /// </summary>
        private void RemoveUI(PanelInfo panelInfo)
        {
            if (panelInfo.Panel == null)
            {
                UnityEngine.Debug.LogError($"无法移除一个null panelInfo 数据 {panelInfo.ResName}");
                return;
            }

            var panel = panelInfo.Panel;
            var foreverCache = panel.PanelForeverCache;
            var timeCache = panel.PanelTimeCache;
            var panelLayer = panel.Layer;
            RemoveLayerPanelInfo(panelLayer, panelInfo);

            if (foreverCache || timeCache)
            {
                //缓存界面只是单纯的吧界面隐藏
                //再次被打开 如何重构界面需要自行设置
                var layerRect = GetLayerRect(EPanelLayer.Cache);
                var uiRect = panel.OwnerRectTransform;
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
                var uiObj = panel.OwnerGameObject;
                UnityEngine.Object.Destroy(uiObj);
                panelInfo.Reset(null);
            }
        }

        internal void RemoveUIReset(string panelName)
        {
            if (TryGetPanelInfo(panelName, out var panelInfo))
            {
                panelInfo.Reset(null);
            }
        }
    }
}