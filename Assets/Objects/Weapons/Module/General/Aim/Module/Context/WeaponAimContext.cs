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
	public class WeaponAimContext : WeaponAim.Module
	{
		public Coordinates Idle { get; protected set; }

        public class Module : Weapon.Module<WeaponAimContext>
        {
            public WeaponAimContext Context => Reference;

            public WeaponAim Aim => Context.Aim;

            public override Weapon Weapon => Aim.Weapon;
        }

        public override void Configure(WeaponAim reference)
        {
            base.Configure(reference);

            References.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            Idle = new Coordinates(transform);

            Weapon.OnLateProcess += LateProcess;

            References.Init(this);
        }

        void LateProcess(Weapon.IProcessData data)
        {
            Apply();
        }

        public event Action OnApply;
        protected virtual void Apply()
        {
            transform.localPosition = Idle.Position;
            transform.localRotation = Idle.Rotation;

            OnApply?.Invoke();
        }
    }
}