﻿using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    public interface IManagerAsyncInit : IManager
    {
        UniTask<bool> InitManagerAsync();
    }

    public interface IManagerUpdate : IManager
    {
        void ManagerUpdate();
    }

    public interface IManagerLateUpdate : IManager
    {
        void ManagerLateUpdate();
    }

    public interface IManagerFixedUpdate : IManager
    {
        void ManagerFixedUpdate();
    }
}