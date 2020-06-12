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

namespace Game
{
	public class PlayerCameraEffects : Player.Module
	{
        [SerializeField]
        protected PostProcessVolume volume;
        public PostProcessVolume Volume { get { return volume; } }

        public PostProcessProfile Profile => volume.profile;

        public PlayerCameraBlurEffect Blur { get; protected set; }

        public class Module : Player.BaseModule<PlayerCameraEffects>
        {
            public PlayerCameraEffects CameraEffects => Reference;

            public override Player Player => Reference.Player;
        }

        public Modules.Collection<PlayerCameraEffects> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<PlayerCameraEffects>(this);

            Blur = Modules.Depend<PlayerCameraBlurEffect>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();
        }
    }
}