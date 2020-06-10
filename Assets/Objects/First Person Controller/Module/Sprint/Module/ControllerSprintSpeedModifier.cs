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
    public class ControllerSprintSpeedModifier : ControllerSprint.Module, Modifier.Scale.IInterface
    {
        [SerializeField]
        protected ValueRange scale = new ValueRange(1f, 2f);
        public ValueRange Scale { get { return scale; } }

        public float Value => scale.Lerp(Sprint.Weight);

        public override void Init()
        {
            base.Init();

            Controller.Movement.Speed.Scale.Register(this);
        }
    }
}