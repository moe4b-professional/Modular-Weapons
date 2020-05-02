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
	public class WeaponDrawAnimation : Weapon.Module, WeaponOperation.IInterface
	{
        [SerializeField]
        protected string trigger = "Draw";
        public string Trigger { get { return trigger; } }

        public WeaponMesh Mesh => Weapon.Mesh;

        public override void Init()
        {
            base.Init();

            Weapon.Activation.OnEnable += Perform;

            Mesh.TriggerRewind.Add(End, trigger + " End");
        }

        public virtual void Perform()
        {
            Mesh.Animator.SetTrigger(trigger);
            Weapon.Operation.Set(this);
        }

        protected virtual void End()
        {
            Weapon.Operation.Clear();
        }

        public virtual void Stop()
        {
            End();
        }
    }
}