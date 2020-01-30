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
	public class ButtonInput
	{
		public bool Press { get; protected set; }

        public bool Held { get; protected set; }

        public bool Up { get; protected set; }

        public virtual void Process(bool input)
        {
            if(input)
            {
                Press = !Press && !Held;
            }
            else
            {
                Up = !Up && Held;
            }

            Held = input;
        }
    }
}