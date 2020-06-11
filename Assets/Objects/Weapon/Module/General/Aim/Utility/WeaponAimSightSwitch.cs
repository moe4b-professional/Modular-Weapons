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
	public class WeaponAimSightSwitch : WeaponAim.Module
	{
		public List<WeaponAimSight> List { get; protected set; }

        public int Index { get; protected set; }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            bool Input { get; }
        }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            List = Aim.Modules.FindAll<WeaponAimSight>();
        }

        public override void Init()
        {
            base.Init();

            Index = 0;

            for (int i = 0; i < List.Count; i++)
                List[i].enabled = Index == i;

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            if (Processor.Input)
                Increment();
        }

        protected virtual void Increment()
        {
            var target = Index + 1;

            if (target >= List.Count) target = 0;

            Set(target);
        }

        protected virtual void Set(int target)
        {
            List[Index].enabled = false;

            Index = target;

            List[Index].enabled = true;
        }
    }
}