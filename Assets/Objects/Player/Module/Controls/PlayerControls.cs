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
	public class PlayerControls : Player.Module
	{
		public PlayerInput Input { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Input = Player.Modules.Find<PlayerInput>();
        }
    }
}