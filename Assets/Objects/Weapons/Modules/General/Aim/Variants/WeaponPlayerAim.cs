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
	public class WeaponPlayerAim : WeaponAim
	{
		[SerializeField]
        protected InputAggregationMode mode = InputAggregationMode.Toggle;
        public InputAggregationMode Mode { get { return mode; } }

        public ButtonInput Button { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Button = new ButtonInput();
        }

        protected override bool Detect(IData data)
        {
            Button.Process(data.Input);

            if (mode == InputAggregationMode.Hold)
            {
                return data.Input;
            }
            else if (mode == InputAggregationMode.Toggle)
            {
                if (Button.Press) IsOn = !IsOn;

                return IsOn;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}