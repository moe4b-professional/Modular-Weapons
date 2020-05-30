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
	public class WeaponPivot : Weapon.Module
	{
        public Coordinates Initial { get; protected set; }

        public override void Init()
        {
            base.Init();

            Initial = new Coordinates(transform);

            Weapon.OnProcess += Process;
        }

        public event Action OnProcess;
        void Process()
        {
            transform.localPosition = Initial.Position;
            transform.localRotation = Initial.Rotation;

            OnProcess?.Invoke();
        }
    }
}