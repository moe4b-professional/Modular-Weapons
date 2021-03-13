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
	public class WeaponAimSightSwitch : WeaponAim.Module
	{
		public List<WeaponAimSight> List { get; protected set; }

        public int Index { get; protected set; }

        public WeaponAimSight Current
        {
            get
            {
                if (Index < 0 || Index >= List.Count)
                    return null;

                return List[Index];
            }
        }

        protected virtual bool IsValidTarget(WeaponAimSight sight)
        {
            if (sight.ActiveInHierarchy == false) return false;

            return true;
        }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            bool Input { get; }
        }

        public override void Configure()
        {
            base.Configure();

            Processor = Weapon.GetProcessor<IProcessor>();

            List = Aim.Modules.FindAll<WeaponAimSight>();
        }

        public override void Init()
        {
            base.Init();

            Index = 0;

            for (int i = 0; i < List.Count; i++)
            {
                if (IsValidTarget(List[i]) == false) continue;

                if (List[i].enabled == false) continue;

                Index = i;
                break;
            }

            for (int i = 0; i < List.Count; i++)
            {
                var instance = List[i];

                instance.enabled = i == Index;
            }

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            if (Processor.Input)
                Increment();

            ValidateCurrent();
        }

        protected virtual void ValidateCurrent()
        {
            if (Current == null) return;

            if (IsValidTarget(Current) == false)
                Increment();
        }

        protected virtual void Increment()
        {
            var iterations = 0;
            var target = Index;

            while (iterations < List.Count - 1)
            {
                iterations += 1;

                target += 1;

                if (target >= List.Count) target = 0;

                if (IsValidTarget(List[target]))
                {
                    Set(target);
                    break;
                }
            }
        }
        
        protected virtual void Set(int target)
        {
            if (Current != null)
                Current.enabled = false;

            Index = target;

            Current.enabled = true;
        }
    }
}