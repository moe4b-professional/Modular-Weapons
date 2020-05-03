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
	public class WeaponConstraint : Weapon.Module
	{
        public IList<IInterface> List { get; protected set; }
        
        public bool Active
        {
            get
            {
                for (int i = 0; i < List.Count; i++)
                    if (List[i].Active)
                        return true;

                return false;
            }
        }

        public interface IInterface
        {
            bool Active { get; }
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            List = Weapon.GetComponentsInChildren<IInterface>();
        }
    }
}