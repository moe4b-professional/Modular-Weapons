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

        public class Module : FirstPersonController.BaseModule<ControllerGround>
        {
            public ControllerGround Ground => Reference;

            public override FirstPersonController Controller => Ground.Controller;
        }

        public Modules.Collection<ControllerGround> Modules { get; protected set; }

        public ControllerGroundData Data => Detect.Data;
        public bool IsDetected => Data != null;

        public ControllerAirTravel AirTravel => Controller.AirTravel;

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<ControllerGround>(this);

            Detect = Modules.Find<ControllerGroundDetect>();
            Change = Modules.Find<ControllerGroundChange>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Detect.Process();
            Change.Set(Data);

            Modules.Init();
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

        public Rigidbody Rigidbody => Collider.attachedRigidbody;

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