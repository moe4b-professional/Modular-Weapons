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
	public class WeaponHitForce : Weapon.Module
	{
        [SerializeField]
        protected float value = 20f;
        public float Value { get { return value; } }

        [SerializeField]
        protected ForceMode mode = ForceMode.Impulse;
        public ForceMode Mode { get { return mode; } }

        public override void Init()
        {
            base.Init();

            Weapon.Hit.OnProcess += Process;
        }

        void Process(HitData data)
        {
            if(data.HasRigidbody)
            {
                data.Rigidbody.AddForceAtPosition(data.Direction * value, data.Contact.Point, mode);
            }
        }
    }
}