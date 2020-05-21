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
	public class ControllerMovementDirection : ControllerMovement.Module
	{
        public Vector3 Forward => Controller.transform.forward;

        public Vector3 Right => Controller.transform.right;

        public Vector3 Up => Controller.transform.up;
    }
}