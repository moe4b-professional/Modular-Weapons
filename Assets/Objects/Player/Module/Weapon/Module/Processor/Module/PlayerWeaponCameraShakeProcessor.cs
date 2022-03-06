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
    public class PlayerWeaponCameraShakeProcessor : PlayerWeapons.Processor, WeaponCameraShake.IProcessor
    {
        [SerializeField]
        protected TransformShake shake;
        public TransformShake Shake { get { return shake; } }

        public float Value => shake.Value;

        public virtual void Add(float target) => shake.Add(target);

        public override void Initialize()
        {
            base.Initialize();

            Player.Controller.OnProcess += Process;
        }

        void Process()
        {
            shake.Calculate();
        }
    }
}