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
	public class ControllerGroundSlopeDetect : ControllerGroundDetect.Module
	{
        [SerializeField]
        [Range(0f, 90f)]
        protected float maxAngle = 50f;
        public float MaxAngle { get { return maxAngle; } }

        public ControllerDirection Direction => Controller.Direction;

        public virtual float CalculateAngle(Vector3 normal) => Vector3.Angle(Direction.Up, normal);

        public virtual bool IsValid(ControllerGroundData data)
        {
            if (IsValid(data.Angle) == false) return false;

            return true;
        }

        public virtual bool IsValid(float angle)
        {
            if (angle > maxAngle) return false;

            return true;
        }
    }
}