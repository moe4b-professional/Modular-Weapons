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
	public abstract class WeaponBobEffect : WeaponBob.Module
	{
        public Vector3 Offset { get; protected set; }

        public Transform Context => Bob.Context;

        public WeaponBob.IProcessor Processor => Bob.Processor;

        protected virtual void Reset()
        {

        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Bob.Anchor.OnWriteDefaults += Write;
        }

        protected virtual void Process()
        {
            CalculateOffset();
        }
        protected abstract void CalculateOffset();

        protected abstract void Write();
    }
}