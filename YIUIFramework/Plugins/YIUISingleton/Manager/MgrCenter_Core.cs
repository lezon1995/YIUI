﻿#define YIUIMACRO_SINGLETON_LOG
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    public partial class MgrCenter
    {
        //内部类 核心管理
        private class MgrCore
        {
            private List<IManager> m_MgrList = new List<IManager>();
            private List<IManagerUpdate> m_MgrUpdateList = new List<IManagerUpdate>();
            private List<IManagerLateUpdate> m_MgrLateUpdateList = new List<IManagerLateUpdate>();
            private List<IManagerFixedUpdate> m_MgrFixedUpdateList = new List<IManagerFixedUpdate>();
            private HashSet<IManager> m_CacheInitMgr = new HashSet<IManager>();

            public async UniTask<bool> Add(IManager manager)
            {
                if (m_MgrList.Contains(manager))
                {
                    Debug.LogError($"已存在Mgr {manager.GetType().Name} 请勿重复注册");
                    return false;
                }

                if (m_CacheInitMgr.Contains(manager))
                {
                    Debug.LogError($"{manager.GetType().Name} 请等待异步初始化中的管理器  请检查是否调用错误");
                    return false;
                }

                m_CacheInitMgr.Add(manager);

                if (manager is IManagerAsyncInit init)
                {
                    var result = await init.InitManagerAsync();
                    if (!result)
                    {
                        //初始化失败的管理器 不添加
                        return false;
                    }
                }

                m_CacheInitMgr.Remove(manager);

#if YIUIMACRO_SINGLETON_LOG
                Debug.Log($"<color=navy>MgrCenter: 管理器[<color=Brown>{manager.GetType().Name}</color>]启动</color>");
#endif

                m_MgrList.Add(manager);

                if (manager is DisposerMonoSingleton)
                {
                    //Mono单例无需检查UP 所以写了也不给你跑
                    //MonoUP自行管理
                    return true;
                }

                if (manager is IManagerUpdate update)
                {
                    m_MgrUpdateList.Add(update);
                }

                if (manager is IManagerLateUpdate lateUpdate)
                {
                    m_MgrLateUpdateList.Add(lateUpdate);
                }

                if (manager is IManagerFixedUpdate fixedUpdate)
                {
                    m_MgrFixedUpdateList.Add(fixedUpdate);
                }

                return true;
            }

            public void Update()
            {
                for (int i = 0; i < m_MgrUpdateList.Count; i++)
                {
                    IManagerUpdate manager = m_MgrUpdateList[i];

                    if (manager.Disposed)
                    {
                        continue;
                    }

                    if (manager.Enabled)
                    {
                        try
                        {
                            manager.ManagerUpdate();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"manager={manager.GetType().Name}, err={e.Message}{e.StackTrace}");
                        }
                    }
                }

                CheckDisposed();
            }

            public void LateUpdate()
            {
                for (int i = 0; i < m_MgrLateUpdateList.Count; i++)
                {
                    IManagerLateUpdate manager = m_MgrLateUpdateList[i];

                    if (manager.Disposed)
                    {
                        continue;
                    }

                    if (!manager.Enabled)
                    {
                        continue;
                    }

                    try
                    {
                        manager.ManagerLateUpdate();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"manager={manager.GetType().Name}, err={e.Message}{e.StackTrace}");
                    }
                }
            }

            public void FixedUpdate()
            {
                for (int i = 0; i < m_MgrFixedUpdateList.Count; i++)
                {
                    IManagerFixedUpdate manager = m_MgrFixedUpdateList[i];

                    if (manager.Disposed)
                    {
                        continue;
                    }

                    if (!manager.Enabled)
                    {
                        continue;
                    }

                    try
                    {
                        manager.ManagerFixedUpdate();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"manager={manager.GetType().Name}, err={e.Message}{e.StackTrace}");
                    }
                }
            }

            private void CheckDisposed()
            {
                for (int i = m_MgrList.Count - 1; i >= 0; i--)
                {
                    IManager manager = m_MgrList[i];
                    if (manager.Disposed)
                    {
                        Remove(manager);
                    }
                }
            }

            private void Remove(IManager manager)
            {
                if (manager == null)
                {
                    return;
                }

#if YIUIMACRO_SINGLETON_LOG
                Debug.Log($"<color=navy>MgrCenter: 管理器[<color=Brown>{manager.GetType().Name}</color>]移除</color>");
#endif

                m_MgrList.Remove(manager);

                if (manager is IManagerUpdate update)
                {
                    m_MgrUpdateList.Remove(update);
                }

                if (manager is IManagerLateUpdate lateUpdate)
                {
                    m_MgrLateUpdateList.Remove(lateUpdate);
                }

                if (manager is IManagerFixedUpdate fixedUpdate)
                {
                    m_MgrFixedUpdateList.Remove(fixedUpdate);
                }
            }

            public void Dispose()
            {
#if YIUIMACRO_SINGLETON_LOG
                Debug.Log("<color=navy>MgrCenter: 关闭MgrCenter</color>");
#endif

                //倒过来dispose
                for (int i = m_MgrList.Count - 1; i >= 0; i--)
                {
                    IManager manager = m_MgrList[i];
                    manager.Dispose();
                }
            }
        }
    }
}