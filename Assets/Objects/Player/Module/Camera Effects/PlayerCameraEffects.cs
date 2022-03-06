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

using UnityEngine.Rendering.PostProcessing;

using MB;

namespace Game
{
	public class PlayerCameraEffects : Player.Module
	{
        [SerializeField]
        protected PostProcessVolume volume;
        public PostProcessVolume Volume { get { return volume; } }

        public PostProcessProfile Profile => volume.profile;

        [field: SerializeField, DebugOnly]
        public PlayerCameraBlurEffect Blur { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Modules<PlayerCameraEffects> Modules { get; protected set; }
        public class Module : Player.Behaviour, IModule<PlayerCameraEffects>
        {
            [field: SerializeField, DebugOnly]
            public PlayerCameraEffects CameraEffects { get; protected set; }

            public Player Player => CameraEffects.Player;

            public virtual void Set(PlayerCameraEffects value) => CameraEffects = value;
        }

        public override void Set(Player value)
        {
            base.Set(value);

            Modules = new Modules<PlayerCameraEffects>(this);
            Modules.Register(Player.Behaviours);

            Blur = Modules.Depend<PlayerCameraBlurEffect>();

            Modules.Set();
        }
    }
}