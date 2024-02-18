// using System;
// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using Sirenix.Serialization;
// using UnityEngine;
// using YIUIFramework;
// using Logger = YIUIFramework.Logger;
//
// namespace YIUIBind
// {
//     [LabelText("UI 数据表")]
//     [Serializable]
//     [AddComponentMenu("YIUIBind/Data Table")]
//     public sealed partial class UIBindDataTable : SerializedMonoBehaviour
//     {
//         [FoldoutGroup("Data", 2)]
//         [OdinSerialize]
//         [LabelText("Runtime Datas")]
//         [ShowInInspector]
//         [DictionaryDrawerSettings(KeyLabel = "Data Name", ValueLabel = "数据内容", IsReadOnly = true, DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
//         private Dictionary<string, UIData> m_DataDic = new Dictionary<string, UIData>();
//         
//         public IReadOnlyDictionary<string, UIData> DataDic => m_DataDic;
//
//         private void Awake()
//         {
//             InitDataTable();
//         }
//
//         public UIData FindData(string dataName)
//         {
//             if (dataName.IsEmpty())
//             {
//                 return null;
//             }
//
//             return m_DataDic.TryGetValue(dataName, out var data) ? data : null;
//         }
//
//         public T FindDataValue<T>(string dataName) where T : UIDataValue
//         {
//             var uiData = FindData(dataName);
//             if (uiData == null)
//             {
//                 Logger.LogErrorContext(this, $"{name} 未找到这个数据请检查 {dataName}");
//                 return default;
//             }
//
//             if (uiData.DataValue == null)
//             {
//                 Logger.LogErrorContext(this, $"{name} 数据没有初始化没有值 {dataName}");
//                 return default;
//             }
//
//             return uiData.DataValue as T;
//         }
//
//         #region 递归初始化所有绑定数据
//
//         private void InitDataTable()
//         {
//             InitializeBinds(transform);
//         }
//
//         private static void InitializeBinds(Transform transform)
//         {
// #if YIUIMACRO_BIND_INITIALIZE
//             Logger.LogErrorContext(transform,$"{transform.name} 初始化调用所有子类 UIDataBind 绑定");
// #endif
//             var binds = ListPool<UIDataBind>.Get();
//             transform.GetComponents(binds);
//             foreach (var bind in binds)
//             {
//                 bind.Initialize(true);
//             }
//
//             ListPool<UIDataBind>.Put(binds);
//
//             for (int i = 0; i < transform.childCount; i++)
//             {
//                 var child = transform.GetChild(i);
//                 InitializeBindsDeep(child);
//             }
//         }
//
//         private static void InitializeBindsDeep(Transform transform)
//         {
//             if (transform.HasComponent<UIBindCDETable>())
//             {
//                 return;
//             }
//
//             var binds = ListPool<UIDataBind>.Get();
//             transform.GetComponents(binds);
//             foreach (var bind in binds)
//             {
//                 bind.Initialize(true);
//             }
//
//             ListPool<UIDataBind>.Put(binds);
//
//             for (int i = 0; i < transform.childCount; i++)
//             {
//                 var child = transform.GetChild(i);
//                 InitializeBindsDeep(child);
//             }
//         }
//
//         #endregion
//     }
// }