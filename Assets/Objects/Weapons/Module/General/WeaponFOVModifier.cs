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
	public class WeaponFOVModifier : Weapon.Module
	{
        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            float Scale { get; set; }
        }

        public ScaleModifier Scale { get; protected set; }
        public class ScaleModifier : Modifier.Scale<WeaponFOVModifier> { }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            Scale = new ScaleModifier();
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            Processor.Scale = Scale.Value;
        }
    }
}