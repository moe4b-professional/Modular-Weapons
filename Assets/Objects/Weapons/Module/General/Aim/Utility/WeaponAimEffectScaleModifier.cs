using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game
{
	public class WeaponAimEffectScaleModifier : WeaponAim.Module
	{
        [SerializeField]
        protected DefaultsContext defaults;
        public DefaultsContext Defaults { get { return defaults; } }
        [Serializable]
        public class DefaultsContext : Context
        {
            [SerializeField]
            protected bool enabled = true;
            public bool Enabled { get { return enabled; } }

            public override IList<WeaponEffects.IInterface> Targets => Weapon.Effects.List;

            public override void UpdateState()
            {
                if(enabled) base.UpdateState();
            }
        }

        [SerializeField]
        protected GameObjectsContext[] contexts;
        public GameObjectsContext[] Contexts { get { return contexts; } }
        [Serializable]
        public class GameObjectsContext : Context
        {
            [SerializeField]
            protected GameObject[] gameObjects;
            public GameObject[] GameObjects { get { return gameObjects; } }

            protected IList<WeaponEffects.IInterface> targets;
            public override IList<WeaponEffects.IInterface> Targets => targets;

            protected virtual List<WeaponEffects.IInterface> GetTargets()
            {
                var list = new List<WeaponEffects.IInterface>();

                for (int i = 0; i < gameObjects.Length; i++)
                {
                    var instance = Dependancy.GetAll<WeaponEffects.IInterface>(gameObjects[i]);

                    list.AddRange(instance);
                }

                return list;
            }

            public override void Configure()
            {
                base.Configure();

                targets = GetTargets();
            }

            public virtual void UpdateState(float rate)
            {
                rate = Mathf.Lerp(1f, 0f, rate);

                for (int i = 0; i < Targets.Count; i++)
                    Targets[i].Scale = range.Lerp(rate);

                for (int i = 0; i < Targets.Count; i++)
                    Debug.Log(range.Lerp(rate));
            }
        }

        [Serializable]
        public abstract class Context : IModule<WeaponAimEffectScaleModifier>
        {
            [SerializeField]
            protected ValueRange range = new ValueRange(0.3f, 1f);
            public ValueRange Range { get { return range; } }

            public abstract IList<WeaponEffects.IInterface> Targets { get; }

            public WeaponAimEffectScaleModifier Modifier { get; protected set; }
            public WeaponAim Aim => Modifier.Aim;
            public Weapon Weapon => Aim.Weapon;

            public virtual void Setup(WeaponAimEffectScaleModifier reference)
            {
                Modifier = reference;
            }

            public virtual void Configure()
            {

            }

            public virtual void Init()
            {
                Modifier.OnUpdateState += UpdateState;
            }

            public virtual void UpdateState()
            {
                for (int i = 0; i < Targets.Count; i++)
                    Targets[i].Scale = Mathf.Lerp(range.Max, range.Min, Aim.Rate);
            }
        }

        public Modules.Collection<WeaponAimEffectScaleModifier> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<WeaponAimEffectScaleModifier>(this, Weapon.gameObject);

            Modules.Add(defaults);

            for (int i = 0; i < contexts.Length; i++)
                Modules.Add(contexts[i]);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();

            Aim.OnRateChange += RateChangeCallback;

            UpdateState();
        }

        void RateChangeCallback(float rate) => UpdateState();

        public event Action OnUpdateState;
        protected virtual void UpdateState()
        {
            OnUpdateState?.Invoke();
        }
    }
}