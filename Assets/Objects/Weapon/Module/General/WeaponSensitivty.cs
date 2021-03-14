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
	public class WeaponSensitivty : Weapon.Module
	{
        public IProcessor Processor { get; protected set; }
        public interface IProcessor : Weapon.IProcessor
        {
            float Scale { get; set; }
        }

        public Modifier.Scale Scale { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Scale = new Modifier.Scale();

            Processor = Weapon.GetProcessor<IProcessor>(this);
        }

        public override void Init()
        {
            base.Init();

            Weapon.Activation.OnDisable += DisableCallback;

            Weapon.OnProcess += Process;
        }

        void DisableCallback()
        {
            Processor.Scale = 1f;
        }

        void Process()
        {
            Processor.Scale = Scale.Value;
        }
    }
}