using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    public enum UIType
    {
        Panel,
        View,
        Component,
    }

    [Serializable]
    [AddComponentMenu("YIUIBind/UITable")]
    public sealed partial class UITable : SerializedMonoBehaviour
    {
        [ReadOnly, HideLabel, HorizontalGroup("UIInfo", 0.2F)]
        public string PkgName;

        [ReadOnly, HideLabel, HorizontalGroup("UIInfo", 0.6F)]
        public string ResName;


        //关联的UI
        UIBase UI;

        //生成UI类时使用
        [LabelText("Child UITables"), ReadOnly, OdinSerialize, PropertyOrder(1000)]
#if UNITY_EDITOR
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
#endif
        internal List<UITable> ChildTables = new List<UITable>();

        //动态生成后的子类(公共组件) 运行时使用
        [LabelText("运行时所有公共组件"), ReadOnly, OdinSerialize, NonSerialized, ShowInInspector, PropertyOrder(1000)]
#if UNITY_EDITOR
        [HideIf("@UIOperationHelper.CommonShowIf()")]
#endif
        Dictionary<string, UIBase> ChildUIBases = new Dictionary<string, UIBase>();

        internal void AddUIBase(string uiName, UIBase uiBase)
        {
            if (!ChildUIBases.TryAdd(uiName, uiBase))
            {
                Debug.LogError($"{name} 已存在 {uiName} 请检查为何重复添加 是否存在同名组件");
            }
        }

        internal UIBase FindUIBase(string uiName)
        {
            if (ChildUIBases.TryGetValue(uiName, out var uiBase))
            {
                return uiBase;
            }

            Debug.LogError($"{name} 不存在 {uiName} 请检查");
            return null;
        }

        public T FindUIBase<T>(string uiName) where T : UIBase
        {
            return FindUIBase(uiName) as T;
        }


        [OdinSerialize, LabelText("Components"), PropertyOrder(1000), DictionaryDrawerSettings(ValueLabel = "Component")]
        Dictionary<string, Component> m_AllBindDic = new Dictionary<string, Component>();

        public IReadOnlyDictionary<string, Component> AllBindDic => m_AllBindDic;

        Component FindComponent(string key)
        {
            m_AllBindDic.TryGetValue(key, out var value);
            if (value == null)
            {
                Logger.LogErrorContext(this, $" {name} 组件表中没有这个组件 {key}");
            }

            return value;
        }

        public T FindComponent<T>(string key) where T : Component
        {
            return FindComponent(key) as T;
        }
    }
}