using System.Collections.Generic;
using UnityEngine;

namespace YIUIFramework
{
    //UI最高层级之上的 屏蔽所有操作的模块
    //屏蔽层显示时所有UI操作会被屏蔽 默认隐藏
    //可通过API 快捷屏蔽操作
    //适用于 统一动画播放时 某些操作需要等待时 都可以调节
    public partial class PanelMgr
    {
        //内部屏蔽对象 显示时之下的所有UI将不可操作
        private GameObject layerBlock;

        //倒计时的唯一ID
        private int lastCountDownId;

        //下一次恢复操作时间
        private float lastRecoverTime;

        private void OnBlockDispose()
        {
            RemoveLastCountDown();
        }

        private void RemoveLastCountDown()
        {
            CountDownMgr.Inst.Remove(ref lastCountDownId);
        }

        //初始化添加屏蔽模块
        private void InitAddUIBlock()
        {
            layerBlock = new GameObject("LayerBlock");
            var rect = layerBlock.AddComponent<RectTransform>();
            layerBlock.AddComponent<CanvasRenderer>();
            layerBlock.AddComponent<UIBlock>();
            rect.SetParent(UILayerRoot);
            rect.SetAsLastSibling();
            rect.ResetToFullScreen();
            SetLayerBlockOption(true);
        }

        /// <summary>
        /// 设置UI是否可以操作
        /// 不能提供此API对外操作
        /// 因为有人设置过后就会忘记恢复
        /// 如果你确实需要你可以设置 禁止无限时间
        /// 之后调用恢复操作也可以做到
        /// </summary>
        /// <param name="value">true = 可以操作 = 屏蔽层会被隐藏</param>
        private void SetLayerBlockOption(bool value)
        {
            layerBlock.SetActive(!value);
        }

        /// <summary>
        /// 强制恢复层级到可操作状态
        /// 此方法会强制打断倒计时 根据需求调用
        /// </summary>
        private void RecoverLayerOptionAll()
        {
            SetLayerBlockOption(true);
            lastRecoverTime = 0;
            blockIds.Clear();
            RemoveLastCountDown();
        }

        #region 倒计时屏蔽

        /// <summary>
        /// 禁止层级操作
        /// 适合于知道想屏蔽多久 且可托管的操作
        /// </summary>
        /// <param name="time">需要禁止的时间</param>
        private void BanLayerOptionCountDown(float time)
        {
            SetLayerBlockOption(false);

            //当前的时间不受暂停影响
            var currentTime = Time.realtimeSinceStartup;
            var currentRecoverOptionTime = currentTime + time;

            //假设A 先禁止100秒
            //B 又想禁止10秒  显然 B这个就会被屏蔽最少需要等到A禁止的时间结束
            if (currentRecoverOptionTime > lastRecoverTime)
            {
                lastRecoverTime = currentRecoverOptionTime;
                RemoveLastCountDown();
                CountDownMgr.Inst.Add(ref lastCountDownId, time, OnCountDownLayerOption);
            }
        }

        private void OnCountDownLayerOption(double residue, double elapsed, double total)
        {
            if (residue <= 0)
            {
                lastCountDownId = 0;

                if (IsForeverBlock)
                {
                    //如果当前被其他永久屏蔽 则等待永久屏蔽执行恢复
                    return;
                }

                SetLayerBlockOption(true);
            }
        }

        #endregion

        #region 永久屏蔽 forever

        //当前是否正在被永久屏蔽
        public bool IsForeverBlock => blockIds.Count >= 1;

        //永久屏蔽引用计数 一定要成对使用且保证
        //否则将会出现永久屏蔽的情况只能通过RecoverLayerOptionCountDown 强制恢复
        private readonly HashSet<int> blockIds = new HashSet<int>();

        //永久屏蔽
        //适用于 不知道要屏蔽多久 但是能保证可以成对调用的
        //这个没有放到API类中
        //因为如果你不能保证请不要用
        //至少过程中要try起来finally 保证不会出错 否则请不要使用这个功能
        public int BanLayerOptionForever()
        {
            SetLayerBlockOption(false);
            var blockId = IDHelper.GetGuid();
            blockIds.Add(blockId);
            return blockId;
        }

        //恢复永久屏蔽
        public void RecoverLayerOptionForever(int blockId)
        {
            if (!blockIds.Contains(blockId))
            {
                return;
            }

            blockIds.Remove(blockId);

            if (IsForeverBlock)
            {
                return;
            }

            //如果当前有其他倒计时 就等待倒计时恢复
            //否则可直接恢复
            if (lastCountDownId == 0)
            {
                SetLayerBlockOption(true);
            }
        }

        #endregion
    }
}