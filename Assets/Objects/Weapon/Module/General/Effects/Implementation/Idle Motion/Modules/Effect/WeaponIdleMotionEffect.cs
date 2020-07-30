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
	public abstract class WeaponIdleMotionEffect : WeaponIdleMotion.Module
	{
        public Vector3 Offset { get; protected set; }

        public Transform Context => IdleMotion.Context;

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            IdleMotion.Anchor.OnWriteDefaults += Write;
        }

        void Process() => CalculateOffset();
        protected abstract void CalculateOffset();

        protected abstract void Write();
    }
}