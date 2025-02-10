using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    public sealed partial class UITable
    {
        [OdinSerialize, LabelText("Components"), PropertyOrder(1000)]
        [DictionaryDrawerSettings(ValueLabel = "Component")]
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

#if UNITY_EDITOR
    public sealed partial class UITable
    {
        /// <summary>
        /// 检查所有绑定命名
        /// 必须m_ 开头
        /// 如果没用命名则使用对象的名字拼接
        /// 会尝试强制修改
        /// 如果还有同名则报错
        /// </summary>
        public void AddComponent(Component component)
        {
            if (UIOperationHelper.CheckUIOperation(this))
            {
                var oldName = component.gameObject.name;
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

                if (m_AllBindDic.ContainsValue(component))
                {
                    Logger.LogErrorContext(component, $"{name} 这个组件已经存在了 重复对象 {component.name} 已忽略");
                    return;
                }

                if (m_AllBindDic.ContainsKey(newName))
                {
                    Logger.LogErrorContext(component, $"{name} 这个命名已经存在了 重复添加 {newName} 已忽略");
                    return;
                }

                m_AllBindDic.Add(newName, component);
            }
        }
    }
#endif
}