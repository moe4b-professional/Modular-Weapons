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
	public class WeaponRotationOnControl : Weapon.Module
	{
        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Acnhor { get { return anchor; } }

        public Transform Context => anchor.transform;

        [SerializeField]
        protected float range = 45f;
        public float Range { get { return range; } }

        [SerializeField]
        protected Vector3 axis = Vector3.forward;
        public Vector3 Axis { get { return axis; } }

        [SerializeField]
        protected bool additive = false;
        public bool Additive { get { return additive; } }

        public int Iterations { get; protected set; }

        public WeaponActionControl ActionControl => Weapon.Action.Control;

        public override void Initialize()
        {
            base.Initialize();

            Weapon.Action.OnPerform += Action;

            anchor.OnWriteDefaults += Write;
        }

        void Action()
        {
            Iterations++;
        }

        void Write()
        {
            if (additive) Context.localEulerAngles += axis * Iterations * range;

            Context.Rotate(axis, range * ActionControl.Rate, Space.Self);
        }
    }
}