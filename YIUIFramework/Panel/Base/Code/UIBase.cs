using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// UI基类
    /// </summary>
    [HideLabel]
    [HideReferenceObjectPicker]
    public abstract partial class UIBase
    {
        protected UITable Table { get; private set; }

        public GameObject GameObject { get; private set; }
        public RectTransform Transform { get; private set; }
        public CanvasGroup CanvasGroup { get; private set; }
        public bool Initialized { get; private set; }
        
        protected PanelMgr manager { get; private set; }

        public string UIPkgName => m_UIBindVo.PkgName;

        public string UIResName => m_UIBindVo.ResName;


        UIBindVo m_UIBindVo;

        internal UIBindVo UIBindVo => m_UIBindVo;

        /// <summary>
        /// 当前显示状态  显示/隐藏
        /// 不要使用这个设置显影
        /// 应该使用控制器 或调用方法 SetActive();
        /// </summary>
        public bool ActiveSelf
        {
            get { return GameObject && GameObject.activeSelf; }
        }

        /// <summary>
        /// 初始化UIBase 由PanelMgr创建对象后调用
        /// 外部禁止
        /// </summary>
        internal bool Initialize(UIBindVo uiBindVo, GameObject gameObject)
        {
            if (gameObject == null)
            {
                Debug.LogError($"null对象无法初始化");
                return false;
            }

            GameObject = gameObject;
            CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            Transform = gameObject.GetComponent<RectTransform>();
            Table = GameObject.GetComponent<UITable>();
            if (Table == null)
            {
                Debug.LogError($"{GameObject.name} 没有UITable组件 这是必须的");
                return false;
            }

            Initialized = true;
            m_UIBindVo = uiBindVo;
            manager = PanelMgr.Inst;
            Table.BindUI(this);
            UIBaseInitialize();
            return true;
        }

        protected T FindComponent<T>(string key) where T : Component
        {
            return Table.FindComponent<T>(key);
        }

        #region 公共方法

        /// <summary>
        /// 设置显隐
        /// </summary>
        public void SetActive(bool value)
        {
            if (GameObject)
            {
                GameObject.SetActive(value);
            }
        }

        //其他的关于 RectTransform 相关的 不建议包一层
        //就直接 OwnerRectTransform. 使用Unity API 就可以了 没必要包一成
        //这么多方法 都有可能用到你都包一层嘛

        #endregion

        #region 生命周期

        //UIBase 生命周期顺序 2
        protected virtual void UIBind()
        {
        }

        //UIBase 生命周期顺序 3
        protected virtual void Initialize()
        {
        }

        void UIBaseInitialize()
        {
            Table.onEnable = UIBaseOnEnable;
            Table.onStart = UIBaseStart;
            try
            {
                SealedInitialize();
                UIBind();
                Initialize();
                if (ActiveSelf)
                    UIBaseOnEnable();
                else
                    UIBaseOnDisable();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        //UIBase 生命周期顺序 6
        protected virtual void Start()
        {
        }

        void UIBaseStart()
        {
            SealedStart();
            Start();
            Table.onDisable = UIBaseOnDisable;
            Table.onDestroy = UIBaseOnDestroy;
        }

        //UIBase 生命周期顺序 4
        protected virtual void OnEnable()
        {
        }

        void UIBaseOnEnable()
        {
            OnEnable();
        }

        //UIBase 生命周期顺序 4
        protected virtual void OnDisable()
        {
        }

        void UIBaseOnDisable()
        {
            OnDisable();
        }

        //UIBase 生命周期顺序 7
        protected virtual void UnUIBind()
        {
        }

        //UIBase 生命周期顺序 8
        protected virtual void OnDestroy()
        {
        }

        void UIBaseOnDestroy()
        {
            UnUIBind();
            OnDestroy();
            SealedOnDestroy();
            UIFactory.Destroy(this);
        }

        #region 密封虚方法由(下级继承后)重写后密封 其他人可以不用关心

        //这是给基类用的生命周期(BasePanel,BaseView) 为了防止有人重写时不调用基类 所以直接独立
        //没有什么穿插需求怎么办
        //基类会重写这个类且会密封你也调用不到
        //不要问为什么...
        //UIBase 生命周期顺序 1
        protected virtual void SealedInitialize()
        {
        }

        //UIBase 生命周期顺序 5
        protected virtual void SealedStart()
        {
        }

        //UIBase 生命周期顺序 9
        protected virtual void SealedOnDestroy()
        {
        }

        #endregion

        #endregion

        public static implicit operator bool(UIBase self)
        {
            return self != null;
        }
    }
}