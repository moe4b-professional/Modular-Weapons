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
	public class ControllerSandbox : FirstPersonController.Module
	{
        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                Controller.rigidbody.isKinematic ^= true;
                Controller.collider.isTrigger = Controller.rigidbody.isKinematic;
            }

            if (Controller.GroundCheck.IsGrounded) Controller.transform.up = Controller.GroundCheck.Hit.Normal;
        }
    }
}