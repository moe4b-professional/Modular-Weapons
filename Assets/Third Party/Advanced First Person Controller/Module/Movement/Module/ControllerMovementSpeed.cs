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
	public class ControllerMovementSpeed : ControllerMovement.Module
	{
		[SerializeField]
        protected float _base = 4f;
        public float Base { get { return _base; } }

        public float Value { get; protected set; }

        public virtual void Calculate(float multiplier)
        {
            Value = Base * multiplier;
        }
    }
}