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
    public class WeaponRPM : Weapon.Module, Weapon.IConstraint
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

        float time = 0f;

        public bool Active { get { return time > 0f; } }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
            Weapon.OnAction += Action;
        }

        void Process(Weapon.IProcessData data)
        {
            time = Mathf.MoveTowards(time, 0f, Time.deltaTime);
        }

        void Action()
        {
            time = Delay;
        }
    }
}