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

        public class Module : FirstPersonController.Behaviour, IModule<ControllerGround>
        {
            public ControllerGround Ground { get; protected set; }
            public virtual void Set(ControllerGround value) => Ground = value;

            public FirstPersonController Controller => Ground.Controller;
        }

        public Modules<ControllerGround> Modules { get; protected set; }

        public ControllerGroundData Data => Detect.Data;
        public bool IsDetected => Data != null;

        public ControllerAirTravel AirTravel => Controller.AirTravel;

        public override void Set(FirstPersonController value)
        {
            base.Set(value);

            Modules = new Modules<ControllerGround>(this);
            Modules.Register(Controller.Behaviours);

            Detect = Modules.Depend<ControllerGroundDetect>();
            Change = Modules.Depend<ControllerGroundChange>();

            Modules.Set();
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