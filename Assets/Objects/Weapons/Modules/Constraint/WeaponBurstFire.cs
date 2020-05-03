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
    public class WeaponBurstFire : Weapon.Module
    {
        [SerializeField]
        protected int iterations = 3;
        public int Iterations { get { return iterations; } }

        public WeaponActionMode FirinMode { get; protected set; }

        public bool Active
        {
            get
            {
                if(FirinMode == null)
                {

                }
                else
                {
                    if (FirinMode.Mode == WeaponActionMode.Enum.Burst)
                        return true;
                }

                return false;
            }
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            FirinMode = Weapon.GetComponentInChildren<WeaponActionMode>();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Action.OnPerform += Action;
        }

        void Action()
        {
            if(Active)
            {
                Coroutine = StartCoroutine(Procedure());

                Weapon.Action.OnPerform -= Action;
            }
        }

        public Coroutine Coroutine { get; protected set; }
        public bool IsProcessing => Coroutine != null;
        IEnumerator Procedure()
        {
            for (int i = 0; i < iterations - 1; i++)
            {
                bool HasNoConstraints() => Weapon.Constraint.Active == false;

                yield return new WaitUntil(HasNoConstraints);

                Weapon.Action.Perform();
            }

            Weapon.Action.OnPerform += Action;
        }
    }
}