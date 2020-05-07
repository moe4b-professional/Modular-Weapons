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
	public class WeaponAimPointContext : Weapon.Module
	{
		public Coordinates Idle { get; protected set; }

        public override void Init()
        {
            base.Init();

            Idle = new Coordinates(transform);

            Weapon.OnLateProcess += LateProcess;
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