﻿using System;
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

        private float timer = 0f;

        public bool Active { get { return timer > 0f; } }

        public IList<IModifier> Modifiers { get; protected set; }

        public float Scale
        {
            get
            {
                var value = 1f;

                for (int i = 0; i < Modifiers.Count; i++)
                    value *= Modifiers[i].Multiplier;

                return value;
            }
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Modifiers = Weapon.GetComponentsInChildren<IModifier>();
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
            Weapon.OnAction += Action;
        }

        void Process(Weapon.IProcessData data)
        {
            timer = Mathf.MoveTowards(timer, 0f, Scale * Time.deltaTime);
        }

        void Action()
        {
            timer = Delay;
        }

        public interface IModifier
        {
            float Multiplier { get; }
        }
    }
}