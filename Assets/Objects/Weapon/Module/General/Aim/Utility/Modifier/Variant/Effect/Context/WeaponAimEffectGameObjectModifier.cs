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
	public class WeaponAimEffectGameObjectModifier : WeaponAimEffectModifier.Context
    {
        [SerializeField]
        protected GameObject[] gameObjects;
        public GameObject[] GameObjects { get { return gameObjects; } }

        protected List<WeaponEffects.IInterface> targets;
        public override IList<WeaponEffects.IInterface> Targets => targets;

        public override void Configure()
        {
            base.Configure();

            targets = new List<WeaponEffects.IInterface>();

            for (int i = 0; i < gameObjects.Length; i++)
            {
                var instances = Dependancy.GetAll<WeaponEffects.IInterface>(gameObjects[i]);

                targets.AddRange(instances);
            }
        }
    }
}