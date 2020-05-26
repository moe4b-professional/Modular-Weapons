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
	public class ControllerGround : FirstPersonController.Module
	{
        public ControllerGroundDetect Detect { get; protected set; }

        public ControllerGroundChange Change { get; protected set; }

        public ControllerAirTravel AirTravel => Controller.AirTravel;

        public class Module : FirstPersonController.Module
        {
            public ControllerGround Ground => Controller.Ground;
        }

        public ControllerGroundData Data => Detect.Data;
        public bool IsGrounded => Data != null;

        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            Detect = Dependancy.Get<ControllerGroundDetect>(Controller.gameObject);

            Change = Dependancy.Get<ControllerGroundChange>(Controller.gameObject);
        }

        public override void Init()
        {
            base.Init();

            Detect.Process();

            Change.Set(Data);
        }

        public virtual void Check()
        {
            Detect.Process();

            Change.Process(Data);
        }
    }

    [Serializable]
    public class ControllerGroundData
    {
        public Collider Collider { get; protected set; }

        public Vector3 Point { get; protected set; }

        public Vector3 Normal { get; protected set; }

        public float Angle { get; protected set; }

        public float StepHeight { get; protected set; }

        public ControllerGroundData(RaycastHit hit, float angle, float stepHeight)
        {
            this.Collider = hit.collider;
            this.Point = hit.point;
            this.Normal = hit.normal;
            this.Angle = angle;
            this.StepHeight = stepHeight;
        }
    }
}