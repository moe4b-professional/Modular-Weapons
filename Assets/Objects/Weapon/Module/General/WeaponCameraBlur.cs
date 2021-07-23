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
	public class WeaponCameraBlur : Weapon.Module
	{
        public Modifier.Average Average { get; protected set; }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor : Weapon.IProcessor
        {
            float Value { get; set; }
        }

        public override void Configure()
        {
            base.Configure();

            Average = new Modifier.Average();

            Processor = Weapon.GetProcessor<IProcessor>(this);
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            Processor.Value = Average.Value;
        }
    }
}