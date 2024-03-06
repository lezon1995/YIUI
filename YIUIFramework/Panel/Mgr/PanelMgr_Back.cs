using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 界面回退功能 关闭 / 恢复 / Home
    /// </summary>
    public partial class PanelMgr
    {
        /// <summary>
        /// 打开一个同级UI时关闭其他UI 
        /// 只有Panel层才有这个逻辑
        /// </summary>
        async UniTask AddUICloseElse(PanelInfo info)
        {
            if (info.Panel is not { Layer: EPanelLayer.Panel })
            {
                return;
            }

            if (info.Panel.PanelIgnoreBack)
            {
                return;
            }

            var layerList = GetLayerList(EPanelLayer.Panel);
            var skipTween = info.Panel.WindowSkipOtherCloseTween;

            for (var i = layerList.Count - 1; i >= 0; i--)
            {
                var panelInfo = layerList[i];

                if (panelInfo == info)
                {
                    continue;
                }

                if (panelInfo.Panel is IBack back)
                {
                    back.DoBackClose(info);
                }

                switch (panelInfo.Panel.StackOption)
                {
                    case EPanelStackOption.Omit:
                        if (skipTween)
                        {
                            panelInfo.Panel.Close(true, true);
                        }
                        else
                        {
                            await panelInfo.Panel.CloseAsync(true, true);
                        }

                        break;
                    case EPanelStackOption.None:
                        break;
                    case EPanelStackOption.Visible:
                        panelInfo.Panel.SetActive(false);
                        break;
                    case EPanelStackOption.VisibleTween:
                        if (!skipTween)
                        {
                            await panelInfo.Panel.InternalOnWindowCloseTween();
                        }

                        panelInfo.Panel.SetActive(false);
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {panelInfo.Panel.StackOption}");
                        panelInfo.Panel.SetActive(false);
                        break;
                }
            }
        }

        async UniTask RemoveUIAddElse(PanelInfo info)
        {
            if (info.Panel is not { Layer: EPanelLayer.Panel })
            {
                return;
            }

            if (info.Panel.PanelIgnoreBack)
            {
                return;
            }

            var layerList = GetLayerList(EPanelLayer.Panel);
            var skipTween = info.Panel.WindowSkipOtherOpenTween;

            for (var i = layerList.Count - 1; i >= 0; i--)
            {
                var child = layerList[i];

                if (child == info)
                {
                    continue;
                }

                if (child.Panel is IBack back)
                {
                    back.DoBackAdd(info);
                }

                var isBreak = true;
                switch (child.Panel.StackOption)
                {
                    case EPanelStackOption.Omit: //不可能进入这里因为他已经被关闭了 如果进入则跳过这个界面
                        isBreak = false;
                        break;
                    case EPanelStackOption.None:
                        break;
                    case EPanelStackOption.Visible:
                        child.Panel.SetActive(true);
                        break;
                    case EPanelStackOption.VisibleTween:
                        child.Panel.SetActive(true);
                        if (!skipTween)
                        {
                            await child.Panel.InternalOnWindowOpenTween();
                        }

                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {child.Panel.StackOption}");
                        child.Panel.SetActive(true);
                        break;
                }

                if (isBreak)
                {
                    break;
                }
            }
        }

        async UniTask RemoveUIToHome(PanelInfo home, bool tween = true)
        {
            if (home.Panel is not { Layer: EPanelLayer.Panel })
            {
                return;
            }

            var layerList = GetLayerList(EPanelLayer.Panel);
            var skipOtherCloseTween = home.Panel.WindowSkipOtherCloseTween;
            var skipHomeOpenTween = home.Panel.WindowSkipHomeOpenTween;

            for (var i = layerList.Count - 1; i >= 0; i--)
            {
                var child = layerList[i];

                if (child != home)
                {
                    if (child.Panel is IBack back)
                    {
                        back.DoBackHome(home);
                    }

                    if (skipOtherCloseTween)
                    {
                        ClosePanel(child.Name, false, true);
                    }
                    else
                    {
                        await ClosePanelAsync(child.Name, tween, true);
                    }

                    continue;
                }

                switch (child.Panel.StackOption)
                {
                    case EPanelStackOption.Omit:
                    case EPanelStackOption.None:
                    case EPanelStackOption.Visible:
                        child.Panel.SetActive(true);
                        break;
                    case EPanelStackOption.VisibleTween:
                        child.Panel.SetActive(true);
                        if (tween && !skipHomeOpenTween)
                        {
                            await child.Panel.InternalOnWindowOpenTween();
                        }

                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {child.Panel.StackOption}");
                        child.Panel.SetActive(true);
                        break;
                }
            }
        }
    }
}