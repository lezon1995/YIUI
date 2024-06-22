#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using YIUIFramework.Editor;

namespace YIUIFramework
{
    //Editor
    public sealed partial class UITable
    {
        #region 界面参数

        [ReadOnly, HideLabel, HorizontalGroup("UIInfo", 0.2F), OnValueChanged(nameof(OnChangeUIType))]
        public UIType UICodeType = UIType.Component;

        [HideIf(nameof(UICodeType), UIType.Component)]
        [LabelText("窗口选项")]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EWindowOption WindowOption = EWindowOption.None;

        [ShowIf(nameof(UICodeType), UIType.Panel)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        [OnValueChanged(nameof(OnChangePanelLayer))]
        public EPanelLayer PanelLayer = EPanelLayer.Panel;

        [ShowIf(nameof(UICodeType), UIType.Panel)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EPanelOption PanelOption = EPanelOption.None;

        [ShowIf(nameof(UICodeType), UIType.Panel)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EPanelStackOption PanelStackOption = EPanelStackOption.VisibleTween;

        [ShowIf(nameof(UICodeType), UIType.View)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EViewWindowType ViewWindowType = EViewWindowType.View;

        [ShowIf(nameof(UICodeType), UIType.View)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public EViewStackOption ViewStackOption = EViewStackOption.VisibleTween;

        [ShowIf(nameof(ShowCachePanelTime), UIType.Panel)]
        [LabelText("CacheTime")]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public float CachePanelTime = 10;

        bool ShowCachePanelTime => PanelOption.Has(EPanelOption.TimeCache);

        [Tooltip("同层级时，优先级高的在前面，相同时后打开的在前面")]
        [LabelText("Priority")]
        [ShowIf(nameof(UICodeType), UIType.Panel)]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        public int Priority;

        void OnChangeUIType()
        {
            var uiPanelName = UIConst.PanelName;
            if (name.EndsWith(uiPanelName) || name.EndsWith(UIConst.PanelSourceName))
            {
                if (UICodeType != UIType.Panel)
                {
                    Debug.LogWarning($"{name} 结尾{uiPanelName} 必须设定为{uiPanelName}类型");
                }

                UICodeType = UIType.Panel;
            }
            else
            {
                var uiViewName = UIConst.ViewName;
                if (name.EndsWith(uiViewName))
                {
                    if (UICodeType != UIType.View)
                    {
                        Debug.LogWarning($"{name} 结尾{uiViewName} 必须设定为{uiViewName}类型");
                    }

                    UICodeType = UIType.View;
                }
                else
                {
                    if (UICodeType != UIType.Component)
                    {
                        Debug.LogWarning($"{name} 想设定为其他类型 请按照规则设定 请勿强行修改");
                    }

                    UICodeType = UIType.Component;
                }
            }
        }

        void OnChangePanelLayer()
        {
            if (PanelLayer >= EPanelLayer.Cache)
            {
                Debug.LogError($" {name} 层级类型 选择错误 请重新选择");
                PanelLayer = EPanelLayer.Panel;
            }
        }

        #endregion

        bool ShowAutoCheckBtn()
        {
            return UIOperationHelper.CheckUIOperation(false);
        }

        [GUIColor(1, 1, 0)]
        [Button("Auto Check", 30)]
        [PropertyOrder(-100)]
        [ShowIf(nameof(ShowAutoCheckBtn))]
        public void ManualAutoCheck()
        {
            AutoCheck();
        }

        public bool AutoCheck()
        {
            if (!UIOperationHelper.CheckUIOperation(this))
                return false;

            if (!UICreateModule.InitVoName(this))
                return false;

            OnChangeUIType();
            OnChangePanelLayer();
            if (UICodeType == UIType.Panel && IsSplitData)
            {
                PanelSplitData.Panel = gameObject;
                if (!PanelSplitData.AutoCheck())
                    return false;
            }

            UICreateModule.RefreshChildTable(this);
            return true;
        }

        bool ShowCreateBtnByHierarchy()
        {
            if (string.IsNullOrEmpty(PkgName) || string.IsNullOrEmpty(ResName))
                return false;

            if (ResName.Contains("Source"))
                return false;

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
                return;

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
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

            var table = AssetDatabase.LoadAssetAtPath<UITable>(path);
            if (table == null)
                return;

            table.GenUICode();
            AssetDatabase.OpenAsset(table);
        }

        internal void GenUICode()
        {
            if (UIOperationHelper.CheckUIOperation(this))
            {
                GenUICode(true, true);
            }
        }

        bool ShowPanelSourceSplit()
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

        internal void GenUICode(bool refresh, bool tips)
        {
            UICreateModule.Create(this, refresh, tips);
        }


        /// <summary>
        /// 检查所有绑定命名
        /// 必须m_ 开头
        /// 如果没用命名则使用对象的名字拼接
        /// 会尝试强制修改
        /// 如果还有同名则报错
        /// </summary>
        internal void AddComponent(Component component)
        {
            if (UIOperationHelper.CheckUIOperation(this))
            {
                var oldName = component.gameObject.name;
                if (component == null)
                {
                    Logger.LogErrorContext(this, $"{name} 空对象  所以 {oldName} 已忽略");
                    return;
                }

                var newName = oldName;

                var cName = NameUtility.ComponentName;
                if (!oldName.CheckFirstName(cName))
                {
                    if (newName.IsEmpty())
                    {
                        if (component)
                        {
                            newName = $"{NameUtility.FirstName}{cName}{component.name}";
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        newName = $"{NameUtility.FirstName}{cName}{oldName}";
                    }
                }

                newName = newName.ChangeToBigName(cName);

                if (newName.IsEmpty())
                {
                    Logger.LogErrorContext(this, $"{name} 存在空名称 {component.name} 已忽略");
                    return;
                }

                if (component == null)
                {
                    Logger.LogErrorContext(this, $"{name} 空对象  所以 {newName} 已忽略");
                    return;
                }

                if (m_AllBindDic.ContainsValue(component))
                {
                    Logger.LogErrorContext(component, $"{name} 这个组件已经存在了 重复对象 {component.name} 已忽略");
                    return;
                }

                if (!m_AllBindDic.TryAdd(newName, component))
                {
                    Logger.LogErrorContext(component, $"{name} 这个命名已经存在了 重复添加 {newName} 已忽略");
                }
            }
        }
    }
}
#endif