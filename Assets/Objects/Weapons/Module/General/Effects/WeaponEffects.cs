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
        public IList<IInterface> List { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            List = Dependancy.GetAll<IInterface>(Weapon.gameObject);
        }

        public interface IInterface
        {
            bool enabled { get; set; }

            float Scale { get; set; }
        }
    }
}