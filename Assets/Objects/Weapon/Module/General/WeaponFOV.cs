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

using MB;

namespace Game
{
	public class WeaponFOV : Weapon.Module
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

            Processor = Weapon.GetProcessor<IProcessor>(this);

            Scale = new Modifier.Scale();
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            Processor.Scale = Scale.Value;
        }
    }
}