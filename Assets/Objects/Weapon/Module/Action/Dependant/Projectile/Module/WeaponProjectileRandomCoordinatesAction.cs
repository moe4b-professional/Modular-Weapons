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
	public class WeaponProjectileRandomCoordinatesAction : WeaponProjectileAction.Module
	{
        [SerializeField]
        protected ValueRange position;
        public ValueRange Position { get { return position; } }

        [SerializeField]
        protected ValueRange rotation;
        public ValueRange Rotation { get { return rotation; } }

        public override void Init()
        {
            base.Init();

            Action.OnPerform += ActionCallback;
        }

        void ActionCallback(Projectile projectile)
        {
            projectile.transform.position += SampleVector(position);

            projectile.transform.eulerAngles += SampleVector(rotation);
        }

        public static Vector3 SampleVector(ValueRange range)
        {
            return new Vector3()
            {
                x = range.Random,
                y = range.Random,
                z = range.Random,
            };
        }
    }
}