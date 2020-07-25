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
	public abstract class PlayerInput : Player.Module
	{
		public float Primary { get; protected set; }
		public float Secondary { get; protected set; }

        public bool Reload { get; protected set; }

        public float SwitchWeapon { get; protected set; }
        public bool SwitchActionMode { get; protected set; }
        public bool SwitchSight { get; protected set; }

        public bool AnyInput
        {
            get
            {
                if (Primary > 0f) return true;
                if (Secondary > 0f) return true;

                if (Reload) return true;

                if (SwitchWeapon > 0f) return true;
                if (SwitchActionMode) return true;
                if (SwitchSight) return true;

                return false;
            }
        }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += Process;
        }

        protected virtual void Process()
        {

        }
    }
}