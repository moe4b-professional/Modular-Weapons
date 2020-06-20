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
	public class WeaponEffects : Weapon.Module
	{
        public List<IInterface> List { get; protected set; }
        public interface IInterface
        {
            bool enabled { get; set; }

            Modifier.Scale Scale { get; }
        }

        public virtual void Register(IInterface element)
        {
            List.Add(element);
        }

        public override void Configure()
        {
            base.Configure();

            List = new List<IInterface>();
        }
    }
}