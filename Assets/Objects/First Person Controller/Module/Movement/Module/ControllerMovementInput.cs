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
	public class ControllerMovementInput : ControllerMovement.Module
	{
		public Vector3 Relative { get; protected set; }

        public Vector3 Absolute { get; protected set; }

        public ControllerDirection Direction => Controller.Direction;

        public virtual void Calcaulate()
        {
            Relative = new Vector3(Controller.Input.Move.X.Value, 0f, Controller.Input.Move.Y.Value);

            Absolute = (Direction.Forward * Relative.z) + (Direction.Right * Relative.x);
        }
	}
}