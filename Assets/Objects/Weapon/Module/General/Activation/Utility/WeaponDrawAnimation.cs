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
	public class WeaponDrawAnimation : Weapon.Module, WeaponOperation.IInterface, WeaponConstraint.IInterface
	{
        public const string ID = "Draw";

        public WeaponMesh Mesh => Weapon.Mesh;

        public bool IsProcessing => Weapon.Operation.Is(this);

        bool WeaponConstraint.IInterface.Active => IsProcessing;

        public override void Init()
        {
            base.Init();

            Weapon.Activation.OnEnable += Perform;

            Mesh.TriggerRewind.OnTrigger += AnimationTriggerCallback;
        }

        void AnimationTriggerCallback(string trigger)
        {
            if (AnimationTrigger.End.Is(trigger, ID)) End();
        }

        public virtual void Perform()
        {
            Mesh.Animator.SetTrigger(ID);
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