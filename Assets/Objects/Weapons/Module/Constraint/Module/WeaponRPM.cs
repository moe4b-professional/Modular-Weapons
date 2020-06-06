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
    public class WeaponRPM : Weapon.Module, WeaponConstraint.IInterface
    {
        [SerializeField]
        uint value = 800;
        public uint Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public float Delay
        {
            get
            {
                return 60f / value;
            }
        }

        private float timer = 0f;

        public bool Constraint { get { return timer > 0f; } }

        public ScaleModifier Scale { get; protected set; }
        public class ScaleModifier : Modifier.Scale<WeaponRPM>
        {

        }

        public override void Configure()
        {
            base.Configure();

            Scale = new ScaleModifier();
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
            Weapon.Action.OnPerform += Action;
        }

        void Process()
        {
            timer = Mathf.MoveTowards(timer, 0f,Time.deltaTime * Scale.Value);
        }

        void Action()
        {
            timer = Delay;
        }
    }
}