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
		public Vector3 Value { get; protected set; }

        public ControllerMovementDirection Direction => Movement.Direction;

        public virtual void Calculate()
        {
            Value = (Direction.Forward * Controller.Input.Move.y) + (Direction.Right * Controller.Input.Move.x);

            Value = Vector3.ClampMagnitude(Value, 1f);
        }
	}
}