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
	public class ControllerLookLeanCast : ControllerLookLean.Module
	{
        public LayerMask Mask => Controller.GenericData.LayerMask;

        [SerializeField]
        protected float offset = 0.1f;
        public float Offset { get { return offset; } }

        public ControllerRig Rig => Controller.Rig;
        public ControllerDirection Direction => Controller.Direction;

        public Vector3 Origin => Rig.Pivot.transform.position;
        public Vector3 End => Rig.camera.transform.position + Direction.Right * Lean.Target * offset;

        public override void Initialize()
        {
            base.Initialize();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            if(Lean.Target != 0f)
            {
                Debug.DrawLine(Origin, End, Color.black);

                if(Physics.Linecast(Origin, End, Mask, QueryTriggerInteraction.Ignore))
                {
                    Lean.Stop();
                }
                else
                {

                }
            }
        }
    }
}