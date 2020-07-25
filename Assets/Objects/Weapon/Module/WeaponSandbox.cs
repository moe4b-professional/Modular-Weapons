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
	public class WeaponSandbox : Weapon.Module
	{
        public float offset;

        public Vector3 axis;

        public Transform target;

        Quaternion initial;

        public bool increment;
        int iterations;

        public WeaponActionControl ActionControl => Weapon.Action.Control;

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            initial = target.localRotation;

            Weapon.Action.OnPerform += Action;
        }

        void Action()
        {
            iterations++;
        }

        void Process()
        {
            target.localRotation = initial;

            if (increment) target.localEulerAngles += axis * iterations * offset;

            target.Rotate(axis, offset * ActionControl.Weight, Space.Self);
        }
    }
}