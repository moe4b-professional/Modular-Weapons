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
	public class PlayerWeaponAimProcessor : PlayerWeapons.Processor, WeaponAim.IProcessor
    {
        [SerializeField]
        protected InputAggregationMode mode = InputAggregationMode.Toggle;
        public InputAggregationMode Mode { get { return mode; } }

        public bool Input { get; protected set; }

        public float Rate { get; set; }

        public SingleAxisInput Axis => Player.Controls.Secondary;

        public override void Init()
        {
            base.Init();

            Player.OnProcess += Process;
        }

        void Process()
        {
            if (mode == InputAggregationMode.Hold)
            {
                Input = Axis.Button.Held;
            }
            else if (mode == InputAggregationMode.Toggle)
            {
                if (Axis.Button.Press) Input = !Input;
            }
        }

        public virtual void ClearInput()
        {
            Input = false;
        }
    }
}