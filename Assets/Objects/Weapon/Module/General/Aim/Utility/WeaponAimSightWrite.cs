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
	public class WeaponAimSightWrite : WeaponAim.Module
    {
        public List<WeaponAimSight> List { get; protected set; }

        public WeaponPivot Pivot => Weapon.Pivot;
        public TransformAnchor Anchor => Pivot.Anchor;
        public Transform Context => Anchor.transform;

        public override void Configure()
        {
            base.Configure();

            List = Aim.Modules.FindAll<WeaponAimSight>();
        }

        public override void Init()
        {
            base.Init();

            Anchor.OnWriteDefaults += Perform;
        }

        protected virtual void Perform()
        {
            var offset = Coordinates.Zero;

            for (int i = 0; i < List.Count; i++)
                offset += Coordinates.Lerp(Coordinates.Zero, List[i].Offset + List[i].Additive, List[i].Weight * Aim.Rate);

            Context.localPosition += offset.Position;
            Context.localRotation *= offset.Rotation;
        }
    }
}