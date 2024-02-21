#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace YIUIFramework
{
    internal class UIBindProvider : ICodeGenerator<UIBindVo>
    {
        //业务代码相关程序集的名字
        //默认有Unity默认程序集 可以根据需求修改
        static readonly string[] LogicAssemblyNames = {"Assembly-CSharp"};

        static Type[] GetLogicTypes()
        {
            return AppDomain.CurrentDomain.GetTypesByAssemblyName(LogicAssemblyNames);
        }

        Type panelType = typeof(UIPanel);
        Type viewType = typeof(UIView);
        Type componentType = typeof(UIComponent);

        public UIBindVo[] Get()
        {
            var gameTypes = GetLogicTypes();
            if (gameTypes.Length < 1)
            {
                return Array.Empty<UIBindVo>();
            }

            var panelTypes = new List<Type>(); //继承panel的
            var viewTypes = new List<Type>(); //继承View的
            var componentTypes = new List<Type>(); //继承Component的
            var binds = new List<UIBindVo>();

            foreach (var gameType in gameTypes)
            {
                if (panelType.IsAssignableFrom(gameType))
                {
                    panelTypes.Add(gameType);
                }
                else if (viewType.IsAssignableFrom(gameType))
                {
                    viewTypes.Add(gameType);
                }
                else if (componentType.IsAssignableFrom(gameType))
                {
                    componentTypes.Add(gameType);
                }
            }

            //panel绑定
            foreach (var type in panelTypes)
            {
                if (type.BaseType == null)
                {
                    continue;
                }

                if (GetBindVo(type.BaseType, type, panelType, out var bindVo))
                {
                    binds.Add(bindVo);
                }
            }

            //view绑定
            foreach (var type in viewTypes)
            {
                if (type.BaseType == null)
                {
                    continue;
                }

                if (GetBindVo(type.BaseType, type, viewType, out var bindVo))
                {
                    binds.Add(bindVo);
                }
            }

            //component绑定
            foreach (var type in componentTypes)
            {
                if (type.BaseType == null)
                {
                    continue;
                }

                if (GetBindVo(type.BaseType, type, componentType, out var bindVo))
                {
                    binds.Add(bindVo);
                }
            }

            return binds.ToArray();
        }

        static bool GetBindVo(Type uiBaseType, Type creatorType, Type groupType, out UIBindVo bindVo)
        {
            bindVo = new UIBindVo();
            if (uiBaseType == null ||
                !uiBaseType.GetFieldValue("PkgName", out bindVo.PkgName) ||
                !uiBaseType.GetFieldValue("ResName", out bindVo.ResName))
            {
                return false;
            }

            bindVo.CodeType = uiBaseType.BaseType;
            bindVo.BaseType = uiBaseType;
            bindVo.CreatorType = creatorType;
            return true;
        }

        public void WriteCode(UIBindVo info, StringBuilder sb)
        {
            sb.Append("            {\r\n");
            sb.AppendFormat("                PkgName     = {0}.PkgName,\r\n", info.BaseType.FullName);
            sb.AppendFormat("                ResName     = {0}.ResName,\r\n", info.BaseType.FullName);
            sb.AppendFormat("                CodeType    = {0},\r\n", GetCodeTypeName(info.CodeType));
            sb.AppendFormat("                BaseType    = typeof({0}),\r\n", info.BaseType.FullName);
            sb.AppendFormat("                CreatorType = typeof({0}),\r\n", info.CreatorType.FullName);
            sb.Append("            };\r\n");
        }

        string GetCodeTypeName(Type uiBaseType)
        {
            if (uiBaseType == panelType)
            {
                return UIConst.BasePanelName;
            }

            if (uiBaseType == viewType)
            {
                return UIConst.BaseViewName;
            }

            if (uiBaseType == componentType)
            {
                return UIConst.BaseComponentName;
            }

            Debug.LogError($"当前类型错误 是否新增了类型 {uiBaseType}");
            return UIConst.UIBaseName;
        }

        public void NewCode(UIBindVo info, StringBuilder sb)
        {
        }
    }
}
#endif