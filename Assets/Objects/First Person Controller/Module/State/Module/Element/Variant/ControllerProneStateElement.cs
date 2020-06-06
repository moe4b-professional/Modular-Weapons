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
	public class ControllerProneStateElement : DefaultControllerStateElement
    {
        protected override void Process()
        {
            base.Process();

            if (Input.Prone.Press)
                Toggle(Sets.Normal);
        }
    }
}