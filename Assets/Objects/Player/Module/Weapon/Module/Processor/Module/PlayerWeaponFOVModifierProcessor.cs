﻿using System;
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
	public class PlayerWeaponFOVModifierProcessor : PlayerWeaponProcessor.Module, WeaponFOVModifier.IProcessor, Modifier.Scale.IInterface
    {
        public float Scale { get; set; } = 1f;

        float Modifier.Scale.IInterface.Value => Scale;

        public override void Init()
        {
            base.Init();

            Player.Controller.camera.FOV.Scale.Register(this);
        }
    }
}