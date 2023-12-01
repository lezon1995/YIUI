#if UNITY_EDITOR
using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    //Editor
    public sealed partial class UIBindDataTable
    {
        [GUIColor(1, 1, 0)]
        [Button("自动检查", 30)]
        [PropertyOrder(-100)]
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        public void AutoCheck()
        {
            if (UIOperationHelper.CheckUIOperation(this))
            {
                var dicKey = m_DataDic.Keys.ToList();
                foreach (var oldName in dicKey)
                {
                    if (oldName.IsEmpty())
                    {
                        continue;
                    }

                    var newName = oldName;

                    if (!oldName.CheckFirstName(NameUtility.DataName))
                    {
                        newName = $"{NameUtility.FirstName}{NameUtility.DataName}{oldName}";
                    }

                    newName = newName.ChangeToBigName(NameUtility.DataName);

                    if (oldName == newName)
                    {
                        continue;
                    }

                    var uiData = m_DataDic[oldName];
                    m_DataDic.Remove(oldName);
                    m_DataDic.Add(newName, uiData);
                }

                OnValidate();
            }
        }

//         [DetailedInfoBox("添加新数据 说明",
//             @"如果出现点击添加一个新的数据界面闪一下
// 然后什么都没加上的情况
// 这是由于预制件嵌套刷新问题
// 需要进入到预制件内部进行添加 外部无法操作
// 为了防止你莫名其妙的吧一个预制件之外的东西拖进来")]
        [ShowInInspector]
        [HorizontalGroup("添加新数据", 0.8F)]
        [HideReferenceObjectPicker]
        [HideLabel]
        [PropertyOrder(-99)]
        [Delayed]
        [NonSerialized]
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        private UINewData m_ToAddData = new UINewData();

        [GUIColor(0, 1, 0)]
        [HorizontalGroup("添加新数据", 0.2F)]
        [Button("Add")]
        [PropertyOrder(-98)]
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        private void AddNewData()
        {
            if (m_ToAddData.Name.IsEmpty())
            {
                UnityTipsHelper.ShowError($"必须填写名称才可以添加");
                return;
            }

            if (m_DataDic.ContainsKey(m_ToAddData.Name))
            {
                UnityTipsHelper.ShowError($"已存在同名数据  请修改 {m_ToAddData.Name}");
                return;
            }

            var data = new UIData(m_ToAddData.Name, m_ToAddData.Data);

            m_DataDic.Add(data.Name, data);
            m_ToAddData = new UINewData();
            AutoCheck();
        }

        private void RemoveCallBack(UIData data)
        {
            data.OnDataRemoveCallBack();
            if (m_DataDic.ContainsKey(data.Name))
            {
                m_DataDic.Remove(data.Name);
            }
            else
            {
                OnRemoveDataByGuid(data);
            }
        }

        private bool OnRemoveDataByGuid(UIData uiData)
        {
            foreach (var cData in m_DataDic)
            {
                if (cData.Value.Guid == uiData.Guid)
                {
                    m_DataDic.Remove(cData.Key);
                    Logger.LogError($"移除了一个不符合规范的数据 {cData.Key}");
                    return true;
                }
            }

            return false;
        }

        private void OnRemoveData(UIData uiData)
        {
            if (uiData.Name.IsEmpty())
            {
                OnRemoveDataByGuid(uiData);
                return;
            }

            if (!m_DataDic.ContainsKey(uiData.Name))
            {
                OnRemoveDataByGuid(uiData);
                return;
            }

            var data = m_DataDic[uiData.Name];

            //如果已经有绑定了 需要提醒是否移除
            if (data.GetBindCount() >= 1)
            {
                var callBackTips = $"{data.Name} 已绑定 {data.GetBindCount()}个目标\n移除会强制清楚所有绑定 请确认是否需要移除!!!";
                UnityTipsHelper.CallBack(callBackTips, () => { RemoveCallBack(data); });
                return;
            }

            RemoveCallBack(data);
        }

        private void OnValidate()
        {
            if (UIOperationHelper.IsPlaying())
            {
                return;
            }

            foreach (var (dataName, data) in m_DataDic)
            {
                if (data == null)
                {
                    continue;
                }

                data.ClearBinds();
                data.OnDataChange(dataName);
                data.OnDataRemoveAction = OnRemoveData;
            }

            InitDataTable();
        }
    }
}
#endif