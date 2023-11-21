using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 效果控制
    /// </summary>
    [AddComponentMenu("YIUIBind/Effect/Effect Control 特效控制器")]
    public sealed class EffectControl : MonoBehaviour
    {
        [SerializeField]
        [LabelText("循环中")]
        private bool looping;

        [SerializeField]
        [LabelText("延迟时间")]
        private float delay;

        [SerializeField]
        [LabelText("效果持续时间")]
        private float duration = 5.0f;

        [SerializeField]
        [LabelText("随着时间的推移而逐渐消失")]
        private float fadeout = 1.0f;

        private PlayState state = PlayState.Stopping;
        private float timer;
        private List<ParticleSystem> particleSystems;
        private List<Animator> animators;
        private List<Animation> animations;
        private float playbackSpeed = 1.0f;

        private bool releaseAfterFinish;

        public bool ReleaseAfterFinish
        {
            get { return releaseAfterFinish; }
            set { releaseAfterFinish = value; }
        }

        /// <summary>
        /// 淡出事件
        /// </summary>
        public event Action FadeoutEvent;

        /// <summary>
        /// 完成事件
        /// </summary>
        public event Action FinishEvent;

        /// <summary>
        /// 效果的播放状态
        /// </summary>
        private enum PlayState
        {
            Stopping,
            Pending,
            Playing,
            Pausing,
            FadeOuting,
        }

        /// <summary>
        /// 获取一个值，该值指示此效果是否正在循环
        /// </summary>
        public bool IsLooping
        {
            get { return looping; }
        }

        /// <summary>
        /// 获取一个值，该值指示是否暂停此效果。
        /// </summary>
        public bool IsPaused
        {
            get { return PlayState.Pausing == state; }
        }

        /// <summary>
        /// 获取一个值，该值指示是否停止此效果。
        /// </summary>
        public bool IsStopped
        {
            get { return PlayState.Stopping == state; }
        }

        /// <summary>
        /// 获取效果持续时间。
        /// </summary>
        public float Duration
        {
            get { return duration; }
        }

        /// <summary>
        /// 获取效果淡出时间。
        /// </summary>
        public float Fadeout
        {
            get { return fadeout; }
        }

        /// <summary>
        /// 获取或设置此效果的播放速度。
        /// </summary>
        public float PlaybackSpeed
        {
            get { return playbackSpeed; }

            set
            {
                playbackSpeed = value;

                foreach (var particleSystem in ParticleSystems)
                {
                    var main = particleSystem.main;
                    main.simulationSpeed = playbackSpeed;
                }

                foreach (var animator in Animators)
                {
                    animator.speed = playbackSpeed;
                }

                foreach (var animation in Animations)
                {
                    var clip = animation.clip;
                    if (clip != null)
                    {
                        animation[clip.name].speed = playbackSpeed;
                    }
                }
            }
        }

        /// <summary>
        /// 得到粒子系统。
        /// </summary>
        private List<ParticleSystem> ParticleSystems
        {
            get
            {
                if (particleSystems == null)
                {
                    particleSystems = ListPool<ParticleSystem>.Get();
                    GetComponentsInChildren(true, particleSystems);
                    foreach (var particleSystem in ParticleSystems)
                    {
                        var main = particleSystem.main;
                        main.simulationSpeed = playbackSpeed;
                    }
                }

                return particleSystems;
            }
        }

        /// <summary>
        /// 获取动画器
        /// </summary>
        private List<Animator> Animators
        {
            get
            {
                if (animators == null)
                {
                    animators = ListPool<Animator>.Get();
                    GetComponentsInChildren(true, animators);
                    foreach (var animator in animators)
                    {
                        animator.speed = playbackSpeed;
                    }
                }

                return animators;
            }
        }

        /// <summary>
        /// 获取动画
        /// </summary>
        private List<Animation> Animations
        {
            get
            {
                if (animations == null)
                {
                    animations = ListPool<Animation>.Get();
                    GetComponentsInChildren(true, animations);
                    foreach (var animation in animations)
                    {
                        var clip = animation.clip;
                        if (clip != null)
                        {
                            animation[clip.name].speed = playbackSpeed;
                        }
                    }
                }

                return animations;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// 估计持续时间。
        /// </summary>
        public void EstimateDuration()
        {
            looping = false;
            duration = 0.0f;
            fadeout = 0.0f;

            foreach (var particleSystem in ParticleSystems)
            {
                if (particleSystem == null)
                {
                    continue;
                }

                if (particleSystem.main.loop)
                {
                    looping = true;
                }

                if (duration < particleSystem.main.duration)
                {
                    duration = particleSystem.main.duration;
                }

                if (fadeout < particleSystem.main.startLifetimeMultiplier)
                {
                    fadeout = particleSystem.main.startLifetimeMultiplier;
                }
            }

            foreach (var animation in Animations)
            {
                if (animation == null)
                {
                    continue;
                }

                var clip = animation.clip;
                if (clip == null)
                {
                    continue;
                }

                if (clip.isLooping)
                {
                    looping = true;
                }

                if (duration < clip.length)
                {
                    duration = clip.length;
                }
            }

            foreach (var animator in Animators)
            {
                if (animator == null)
                {
                    continue;
                }

                var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.loop)
                {
                    looping = true;
                }

                if (duration < stateInfo.length)
                {
                    duration = stateInfo.length;
                }
            }
        }

        /// <summary>
        /// 刷新此效果。
        /// </summary>
        public void Refresh()
        {
            if (particleSystems != null)
            {
                ListPool<ParticleSystem>.Put(particleSystems);
                particleSystems = null;
            }

            if (animations != null)
            {
                ListPool<Animation>.Put(animations);
                animations = null;
            }

            if (animators != null)
            {
                ListPool<Animator>.Put(animators);
                animators = null;
            }
        }

        private void OnDestroy()
        {
            Refresh();
        }

        /// <summary>
        /// 开始模拟。
        /// </summary>
        public void SimulateInit()
        {
            // 烘焙所有动画师。
            var animators = Animators;
            foreach (var animator in animators)
            {
                if (animator == null)
                {
                    continue;
                }

                if (animator.runtimeAnimatorController == null)
                {
                    continue;
                }

                const float FrameRate = 30f;
                var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                int frameCount = (int)((stateInfo.length * FrameRate) + 2);

                animator.Rebind();
                animator.StopPlayback();
                animator.recorderStartTime = 0;
                animator.StartRecording(frameCount);

                for (var i = 0; i < frameCount - 1; ++i)
                {
                    animator.Update(i / FrameRate);
                }

                animator.StopRecording();
                animator.StartPlayback();
            }
        }

        /// <summary>
        /// 开始模拟效果。
        /// </summary>
        public void SimulateStart()
        {
            var particleSystems = ParticleSystems;
            foreach (var ps in particleSystems)
            {
                if (ps == null)
                {
                    continue;
                }

                ps.Simulate(0, false, true);
                ps.time = 0;
                ps.Play();
            }

            var animators = Animators;
            foreach (var animator in animators)
            {
                if (animator == null)
                {
                    continue;
                }

                if (animator.runtimeAnimatorController == null)
                {
                    continue;
                }

                animator.playbackTime = 0.0f;
                animator.Update(0.0f);
            }

            var animations = Animations;
            foreach (var animation in animations)
            {
                if (animation == null)
                {
                    continue;
                }

                var clip = animation.clip;
                if (clip == null)
                {
                    continue;
                }

                clip.SampleAnimation(animation.gameObject, 0.0f);
            }
        }

        /// <summary>
        /// 通过增量时间更新模拟的效果。
        /// </summary>
        public void SimulateDelta(float time, float deltaTime)
        {
            var particleSystems = ParticleSystems;
            foreach (var ps in particleSystems)
            {
                if (ps == null)
                {
                    continue;
                }

                ps.Simulate(deltaTime, false, false);
            }

            var animators = Animators;
            foreach (var animator in animators)
            {
                if (animator == null)
                {
                    continue;
                }

                if (animator.runtimeAnimatorController == null)
                {
                    continue;
                }

                animator.playbackTime = time;
                animator.Update(0.0f);
            }

            var animations = Animations;
            foreach (var animation in animations)
            {
                if (animation == null)
                {
                    continue;
                }

                var clip = animation.clip;
                if (clip == null)
                {
                    continue;
                }

                clip.SampleAnimation(animation.gameObject, time);
            }
        }

        /// <summary>
        /// 模拟在编辑器模式下。
        /// </summary>
        public void Simulate(float time)
        {
            var randomKeeper = new Dictionary<ParticleSystem, KeyValuePair<bool, uint>>();
            var particleSystems = ParticleSystems;
            foreach (var ps in particleSystems)
            {
                if (ps == null)
                {
                    continue;
                }

                ps.Stop(false);
                var pair = new KeyValuePair<bool, uint>(
                    ps.useAutoRandomSeed, ps.randomSeed);
                randomKeeper.Add(ps, pair);
                if (!ps.isPlaying)
                {
                    ps.useAutoRandomSeed = false;
                    ps.randomSeed = 0;
                }

                ps.Simulate(0, false, true);
                ps.time = 0;
                ps.Play();
            }

            for (float i = 0.0f; i < time; i += 0.02f)
            {
                foreach (var ps in particleSystems)
                {
                    if (ps == null)
                    {
                        continue;
                    }

                    ps.Simulate(0.02f, false, false);
                }
            }

            foreach (var ps in particleSystems)
            {
                if (ps == null)
                {
                    continue;
                }

                ps.Stop(false);
                var pair = randomKeeper[ps];
                ps.randomSeed = pair.Value;
                ps.useAutoRandomSeed = pair.Key;
            }

            var animators = Animators;
            foreach (var animator in animators)
            {
                if (animator == null)
                {
                    continue;
                }

                if (animator.runtimeAnimatorController == null)
                {
                    continue;
                }

                animator.playbackTime = time;
                animator.Update(0.0f);
            }

            var animations = Animations;
            foreach (var animation in animations)
            {
                if (animation == null)
                {
                    continue;
                }

                var clip = animation.clip;
                if (clip == null)
                {
                    continue;
                }

                clip.SampleAnimation(animation.gameObject, time);
            }
        }
#endif

        /// <summary>
        /// 开始
        /// </summary>
        public void Play()
        {
            if (PlayState.Playing == state)
            {
                Stop();
            }

            state = PlayState.Pending;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (PlayState.Playing == state)
            {
                foreach (var particleSystem in ParticleSystems)
                {
                    particleSystem.Pause(false);
                }

                foreach (var animator in Animators)
                {
                    animator.speed = 0.0f;
                }

                foreach (var animation in Animations)
                {
                    var clip = animation.clip;
                    if (clip != null)
                    {
                        animation[clip.name].speed = 0.0f;
                    }
                }

                state = PlayState.Pausing;
            }
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        public void Resume()
        {
            if (PlayState.Pausing == state)
            {
                foreach (var particleSystem in ParticleSystems)
                {
                    particleSystem.Play(false);
                }

                foreach (var animator in Animators)
                {
                    animator.speed = playbackSpeed;
                }

                foreach (var animation in Animations)
                {
                    var clip = animation.clip;
                    if (clip != null)
                    {
                        animation[clip.name].speed = playbackSpeed;
                    }
                }

                state = PlayState.Playing;
            }
        }

        public void ForceCallFinishEvent()
        {
            FinishEvent?.Invoke();
        }

        public void ClearFinishEvent()
        {
            FinishEvent = null;
            ReleaseAfterFinish = false;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Stop()
        {
            if (state != PlayState.Stopping)
            {
                state = PlayState.FadeOuting;
                foreach (var particleSystem in ParticleSystems)
                {
                    particleSystem.Stop(false);
                }

                foreach (var animator in Animators)
                {
                    animator.gameObject.SetActive(false);
                }

                foreach (var animation in Animations)
                {
                    if (animation.playAutomatically)
                    {
                        animation.gameObject.SetActive(false);
                    }
                    else
                    {
                        animation.Stop();
                    }
                }

                if (FadeoutEvent != null)
                {
                    FadeoutEvent();
                    FadeoutEvent = null;
                }
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            timer = 0.0f;
            state = PlayState.Stopping;
        }

        private void Awake()
        {
            if (particleSystems == null)
            {
                particleSystems = ListPool<ParticleSystem>.Get();
                GetComponentsInChildren(true, particleSystems);
            }

            Reset();
        }

        private void LateUpdate()
        {
            if (PlayState.Stopping == state ||
                PlayState.Pausing == state)
            {
                return;
            }

            timer += Time.deltaTime * playbackSpeed;
            if (PlayState.Pending == state && timer >= delay)
            {
                foreach (var particleSystem in ParticleSystems)
                {
                    particleSystem.Play(false);
                }

                foreach (var animator in Animators)
                {
                    animator.gameObject.SetActive(false);
                    animator.gameObject.SetActive(true);
                }

                foreach (var animation in Animations)
                {
                    if (animation.playAutomatically)
                    {
                        animation.gameObject.SetActive(false);
                        animation.gameObject.SetActive(true);
                    }
                    else
                    {
                        animation.Stop();
                        animation.Play();
                    }
                }

                state = PlayState.Playing;
            }

            if (!looping)
            {
                if (PlayState.Playing == state &&
                    timer >= duration)
                {
                    Stop();
                }
            }

            if (PlayState.FadeOuting == state &&
                timer >= duration + fadeout)
            {
                state = PlayState.Stopping;
                if (FinishEvent != null)
                {
                    FinishEvent();
                    FinishEvent = null;
                    if (ReleaseAfterFinish)
                    {
                        ReleaseAfterFinish = false;

                        //Debug.LogError("TODO GameObjectPool");
                        //GameObjectPool.Instance.Free(gameObject);
                    }
                }
            }
        }
    }
}