#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;
using YIUIFramework.Editor;

namespace YIUIFramework
{
    //Editor
    public sealed partial class UIBindCDETable
    {
        #region 界面参数

        [HorizontalGroup("UIInfo", 0.2F)]
        [HideLabel]
        [ReadOnly]
        [OnValueChanged(nameof(OnValueChangedEUICodeType))]
        public EUICodeType UICodeType = EUICodeType.Component;

        [HideIf(nameof(UICodeType), EUICodeType.Component)]
        [LabelText("窗口选项")]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EWindowOption WindowOption = EWindowOption.None;

        [ShowIf(nameof(UICodeType), EUICodeType.Panel)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        [OnValueChanged(nameof(OnValueChangedEPanelLayer))]
        public EPanelLayer PanelLayer = EPanelLayer.Panel;

        [ShowIf(nameof(UICodeType), EUICodeType.Panel)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EPanelOption PanelOption = EPanelOption.None;

        [ShowIf(nameof(UICodeType), EUICodeType.Panel)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EPanelStackOption PanelStackOption = EPanelStackOption.VisibleTween;

        [ShowIf(nameof(UICodeType), EUICodeType.View)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EViewWindowType ViewWindowType = EViewWindowType.View;

        [ShowIf(nameof(UICodeType), EUICodeType.View)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EViewStackOption ViewStackOption = EViewStackOption.VisibleTween;

        [ShowIf(nameof(ShowCachePanelTime), EUICodeType.Panel)]
        [LabelText("缓存时间")]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public float CachePanelTime = 10;

        private bool ShowCachePanelTime => PanelOption.Has(EPanelOption.TimeCache);

        [Tooltip("同层级时，优先级高的在前面，相同时后打开的在前面")]
        [LabelText("优先级")]
        [ShowIf(nameof(UICodeType), EUICodeType.Panel)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public int Priority;

        private void OnValueChangedEUICodeType()
        {
            var uiPanelName = UIStaticHelper.UIPanelName;
            if (name.EndsWith(uiPanelName) || name.EndsWith(UIStaticHelper.UIPanelSourceName))
            {
                if (UICodeType != EUICodeType.Panel)
                {
                    Debug.LogWarning($"{name} 结尾{uiPanelName} 必须设定为{uiPanelName}类型");
                }

                UICodeType = EUICodeType.Panel;
            }
            else
            {
                var uiViewName = UIStaticHelper.UIViewName;
                if (name.EndsWith(uiViewName))
                {
                    if (UICodeType != EUICodeType.View)
                    {
                        Debug.LogWarning($"{name} 结尾{uiViewName} 必须设定为{uiViewName}类型");
                    }

                    UICodeType = EUICodeType.View;
                }
                else
                {
                    if (UICodeType != EUICodeType.Component)
                    {
                        Debug.LogWarning($"{name} 想设定为其他类型 请按照规则设定 请勿强行修改");
                    }

                    UICodeType = EUICodeType.Component;
                }
            }
        }

        private void OnValueChangedEPanelLayer()
        {
            if (PanelLayer >= EPanelLayer.Cache)
            {
                Debug.LogError($" {name} 层级类型 选择错误 请重新选择");
                PanelLayer = EPanelLayer.Panel;
            }
        }

        #endregion

        private bool ShowAutoCheckBtn()
        {
            return UIOperationHelper.CheckUIOperation(false);
        }


        [GUIColor(1, 1, 0)]
        [Button("自动检查所有", 30)]
        [PropertyOrder(-100)]
        [ShowIf(nameof(ShowAutoCheckBtn))]
        public bool AutoCheck()
        {
            if (!UIOperationHelper.CheckUIOperation(this))
            {
                return false;
            }

            if (!UICreateModule.InitVoName(this))
            {
                return false;
            }

            OnValueChangedEUICodeType();
            OnValueChangedEPanelLayer();
            if (UICodeType == EUICodeType.Panel && IsSplitData)
            {
                PanelSplitData.Panel = gameObject;
                if (!PanelSplitData.AutoCheck())
                {
                    return false;
                }
            }

            UICreateModule.RefreshChildTable(this);

            return true;
        }

        private bool ShowCreateBtnByHierarchy()
        {
            if (string.IsNullOrEmpty(PkgName) || string.IsNullOrEmpty(ResName))
            {
                return false;
            }

            if (ResName.Contains("Source"))
            {
                return false;
            }

            if (UIOperationHelper.CheckUIOperation(this, false))
            {
                return !PrefabUtility.IsPartOfPrefabAsset(this);
            }

            return false;
        }

        [GUIColor(0f, 0.5f, 1f)]
        [Button("生成", 50)]
        [PropertyOrder(10000)]
        [ShowIf(nameof(ShowCreateBtnByHierarchy))]
        internal void CreateUICodeByHierarchy()
        {
            if (!ShowCreateBtnByHierarchy())
            {
                return;
            }

            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage == null)
            {
                Debug.LogError($"当前不在预制体编辑器模式下");
                return;
            }

            var path = prefabStage.assetPath;
            var root = prefabStage.prefabContentsRoot;
            PrefabUtility.SaveAsPrefabAsset(root, path, out var success);
            if (!success)
            {
                Debug.LogError("快捷保存失败 请检查");
                return;
            }

            prefabStage.ClearDirtiness();

            var cdeTable = AssetDatabase.LoadAssetAtPath<UIBindCDETable>(path);
            if (cdeTable == null)
            {
                return;
            }

            cdeTable.CreateUICode();

            AssetDatabase.OpenAsset(cdeTable);
        }

        private bool ShowCreateBtn()
        {
            if (IsSplitData)
            {
                return false;
            }

            return UIOperationHelper.CheckUIOperationAll(this, false);
        }

        [GUIColor(0.7f, 0.4f, 0.8f)]
        [Button("Generate Code", 50)]
        [ShowIf(nameof(ShowCreateBtn))]
        internal void CreateUICode()
        {
            if (UIOperationHelper.CheckUIOperation(this))
            {
                CreateUICode(true, true);
            }
        }

        private bool ShowPanelSourceSplit()
        {
            return UIOperationHelper.CheckUIOperationAll(this, false) && IsSplitData;
        }

        [GUIColor(0f, 0.4f, 0.8f)]
        [Button("源数据拆分", 50)]
        [ShowIf(nameof(ShowPanelSourceSplit))]
        internal void PanelSourceSplit()
        {
            if (UIOperationHelper.CheckUIOperation(this))
            {
                if (IsSplitData)
                {
                    if (AutoCheck())
                    {
                        UIPanelSourceSplit.Do(this);
                    }
                }
                else
                {
                    UnityTipsHelper.ShowError($"{name} 当前数据不是源数据 无法进行拆分 请检查数据");
                }
            }
        }

        internal void CreateUICode(bool refresh, bool tips)
        {
            UICreateModule.Create(this, refresh, tips);
        }
    }
}
#endif