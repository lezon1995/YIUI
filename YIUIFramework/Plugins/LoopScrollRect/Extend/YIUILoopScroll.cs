﻿

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YIUIBind;

namespace YIUIFramework
{
    public partial class YIUILoopScroll<TData, TItemRenderer> : LoopScrollPrefabSource, LoopScrollDataSource
        where TItemRenderer : UIBase
    {
        /// <summary>
        /// 列表项渲染器
        /// </summary>
        /// <param name="index">数据的索引</param>
        /// <param name="data">数据项</param>
        /// <param name="item">显示对象</param>
        /// <param name="select">是否被选中</param>
        public delegate void ListItemRenderer(int index, TData data, TItemRenderer item, bool select);

        private ListItemRenderer                     m_ItemRenderer;
        private UIBindVo                             m_BindVo;
        private IList<TData>                         m_Data;
        private LoopScrollRect                       m_Owner;
        private ObjCache<TItemRenderer>              m_UIBasePool;
        private Dictionary<Transform, TItemRenderer> m_ItemTransformDic = new Dictionary<Transform, TItemRenderer>();
        private Dictionary<Transform, int>           m_ItemTransformIndexDic = new Dictionary<Transform, int>();

        public YIUILoopScroll(
            LoopScrollRect   owner,
            ListItemRenderer itemRenderer)
        {
            var data = UIBindHelper.GetBindVoByType<TItemRenderer>();
            if (data == null) return;
            m_ItemTransformDic.Clear();
            m_ItemTransformIndexDic.Clear();
            m_BindVo             = data.Value;
            m_ItemRenderer       = itemRenderer;
            m_UIBasePool         = new ObjCache<TItemRenderer>(OnCreateItemRenderer);
            m_Owner              = owner;
            m_Owner.prefabSource = this;
            m_Owner.dataSource   = this;
            InitCacheParent();
            InitClearContent();
        }

        #region Private

        private void InitCacheParent()
        {
            if (m_Owner.u_CacheRect != null)
            {
                m_Owner.u_CacheRect.gameObject.SetActive(false);
            }
            else
            {
                var cacheObj  = new GameObject("Cache");
                var cacheRect = cacheObj.GetOrAddComponent<RectTransform>();
                m_Owner.u_CacheRect = cacheRect;
                cacheRect.SetParent(m_Owner.transform, false);
                cacheObj.SetActive(false);
            }
        }

        private void InitClearContent()
        {
            //不应该初始化时有内容 所有不管是什么全部摧毁
            for (var i = 0; i < Content.childCount; i++)
            {
                var child = Content.GetChild(i);
                Object.Destroy(child.gameObject);
            }
        }

        private TItemRenderer GetItemRendererByDic(Transform tsf)
        {
            if (m_ItemTransformDic.TryGetValue(tsf, out var value))
            {
                return value;
            }

            Debug.LogError($"{tsf.name} 没找到这个关联对象 请检查错误");
            return null;
        }

        private void AddItemRendererByDic(Transform tsf, TItemRenderer item)
        {
            if (!m_ItemTransformDic.ContainsKey(tsf))
            {
                m_ItemTransformDic.Add(tsf, item);
            }
        }

        private int GetItemIndex(Transform tsf)
        {
            if (m_ItemTransformIndexDic.TryGetValue(tsf, out var value))
            {
                return value;
            }

            return -1;
        }

        private void ResetItemIndex(Transform tsf, int index)
        {
            if (!m_ItemTransformIndexDic.ContainsKey(tsf))
            {
                m_ItemTransformIndexDic.Add(tsf, index);
            }
            else
            {
                m_ItemTransformIndexDic[tsf] = index;
            }
        }

        #endregion

        #region LoopScrollRect Interface

        private TItemRenderer OnCreateItemRenderer()
        {
            var uiBase = YIUIFactory.Instantiate<TItemRenderer>(m_BindVo);
            AddItemRendererByDic(uiBase.OwnerRectTransform, uiBase);
            return AddOnClickEvent(uiBase);
        }

        public GameObject GetObject(int index)
        {
            var uiBase = m_UIBasePool.Get();
            return uiBase.OwnerGameObject;
        }

        public void ReturnObject(Transform transform)
        {
            var uiBase = GetItemRendererByDic(transform);
            if (uiBase == null) return;
            m_UIBasePool.Put(uiBase);
            ResetItemIndex(transform, -1);
            transform.SetParent(m_Owner.u_CacheRect, false);
        }

        public void ProvideData(Transform transform, int index)
        {
            var uiBase = GetItemRendererByDic(transform);
            if (uiBase == null) return;
            ResetItemIndex(transform, index);
            var select = m_OnClickItemHashSet.Contains(index);
            if (m_Data == null)
            {
                Debug.LogError($"当前没有设定数据 m_Data == null");
                return;
            }

            m_ItemRenderer?.Invoke(index, m_Data[index], uiBase, select);
        }

        #endregion
    }
}