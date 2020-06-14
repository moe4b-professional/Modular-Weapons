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

        public bool CheckAny(Predicate<IInterface> predicate)
        {
            for (int i = 0; i < List.Count; i++)
                if (predicate(List[i]))
                    return true;

            return false;
        }
        
        public bool Active
        {
            get
            {
                bool IsActive(IInterface element) => element.Active;

                return CheckAny(IsActive);
            }
        }

        public interface IInterface
        {
            bool Active { get; }
        }

        public override void Configure()
        {
            base.Configure();

            List = Dependancy.GetAll<IInterface>(Weapon.gameObject);
        }
    }
}