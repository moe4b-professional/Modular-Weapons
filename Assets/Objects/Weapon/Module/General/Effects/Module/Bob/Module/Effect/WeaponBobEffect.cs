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
        [SerializeField]
        protected float range;
        public float Range { get { return range; } }

        public Transform Context => Bob.Context;

        protected virtual void Reset()
        {

        }

        public override void Init()
        {
            base.Init();

            Bob.OnProcess += Process;
        }

        protected virtual void Process()
        {

        }
    }
}