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
	public class WeaponAmmoProcedureChange : WeaponAmmo.Module
	{
        [SerializeField]
        protected TargetData[] targets;
        public TargetData[] Targets { get { return targets; } }
        [Serializable]
        public class TargetData
        {
            [SerializeField]
            protected GameObject gameObject;
            public GameObject GameObject { get { return gameObject; } }

            public virtual void SetActive(bool value) => gameObject.SetActive(value ^ constraint);

            [SerializeField]
            protected bool constraint;
            public bool Constraint { get { return constraint; } }
        }

        public override void Init()
        {
            base.Init();

            Ammo.OnRefill += RefillCallback;
            Ammo.OnConsumption += ConsumptionCallback;

            UpdateState();
        }

        protected virtual void UpdateState()
        {
            for (int i = 0; i < targets.Length; i++)
                targets[i].SetActive(Ammo.CanConsume);
        }

        void RefillCallback() => UpdateState();
        void ConsumptionCallback() => UpdateState();
    }
}