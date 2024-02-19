﻿#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;

namespace YIUIFramework.Editor
{
    /// <summary>
    ///  宏
    /// </summary>
    public class UIMacroModule : BaseYIUIToolModule
    {
        [GUIColor(0.4f, 0.8f, 1)]
        [BoxGroup("目标平台切换", centerLabel: true)]
        [OnValueChanged("OnBuildTargetGroupChange")]
        [ShowInInspector]
        [HideLabel]
        public static BuildTargetGroup BuildTargetGroup = BuildTargetGroup.Standalone;

        [BoxGroup("当前平台所有宏", centerLabel: true)]
        [HideLabel]
        public MacroCurrentData MacroStaticData;

        [BoxGroup("自定义宏数据", centerLabel: true)]
        [LabelText(" ")]
        [ShowInInspector]
        [ListDrawerSettings(IsReadOnly = true)]
        [HideReferenceObjectPicker]
        List<MacroDataBase> AllMacroData;

        [Button("更新自定义宏", 50), GUIColor(0.53f, 0.95f, 0.72f)]
        public void Refresh()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            var allRemove = new List<string>();
            var allSelect = new List<string>();
            foreach (var macroData in AllMacroData)
            {
                allRemove.AddRange(macroData.GetAll());
                allSelect.AddRange(macroData.GetSelect());
            }

            MacroHelper.ChangeSymbols(allRemove, allSelect, BuildTargetGroup);

            SelfInitialize();

            YIUIAutoTool.CloseWindowRefresh();
        }

        public override void Initialize()
        {
            BuildTargetGroup = GetCurrentBuildPlatform();
            SelfInitialize();
        }

        void SelfInitialize()
        {
            MacroStaticData = new MacroCurrentData(SelfInitialize);

            AllMacroData = new List<MacroDataBase>
            {
                new UIMacroData(),
            };
        }

        static BuildTargetGroup GetCurrentBuildPlatform()
        {
            var buildTargetName = EditorUserBuildSettings.activeBuildTarget.ToString();
            buildTargetName = buildTargetName.ToLower();
            if (buildTargetName.IndexOf("standalone", StringComparison.Ordinal) >= 0)
            {
                return BuildTargetGroup.Standalone;
            }

            if (buildTargetName.IndexOf("android", StringComparison.Ordinal) >= 0)
            {
                return BuildTargetGroup.Android;
            }

            if (buildTargetName.IndexOf("iphone", StringComparison.Ordinal) >= 0)
            {
                return BuildTargetGroup.iOS;
            }

            if (buildTargetName.IndexOf("ios", StringComparison.Ordinal) >= 0)
            {
                return BuildTargetGroup.iOS;
            }

            return BuildTargetGroup.Standalone;
        }

        void OnBuildTargetGroupChange()
        {
            SelfInitialize();
        }

        public override void OnDestroy()
        {
        }
    }
}
#endif