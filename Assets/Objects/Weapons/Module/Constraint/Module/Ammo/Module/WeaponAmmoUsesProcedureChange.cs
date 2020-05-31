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
	public class WeaponAmmoUsesProcedureChange : WeaponAmmo.Module
	{
        [SerializeField]
        protected TargetData[] targets;
        public TargetData[] Targets { get { return targets; } }
        [Serializable]
        public class TargetData
        {
            [SerializeField]
            protected string name;
            public string Name { get { return name; } }

            [SerializeField]
            protected GameObject[] gameObjects;
            public GameObject[] GameObjects { get { return gameObjects; } }

            [SerializeField]
            protected ConditionData condition;
            public ConditionData Condition { get { return condition; } }
            [Serializable]
            public class ConditionData
            {
                [SerializeField]
                protected int uses;
                public int Uses { get { return uses; } }

                [SerializeField]
                protected OperatorMode operation;
                public OperatorMode Operation { get { return operation; } }
                [Serializable]
                public enum OperatorMode
                {
                    Equals, Less, More
                }
                
                public virtual bool Evaluate(int remainingUses)
                {
                    switch (operation)
                    {
                        case OperatorMode.Equals:
                            return remainingUses == uses;
                        case OperatorMode.Less:
                            return remainingUses < uses;
                        case OperatorMode.More:
                            return remainingUses > uses;
                    }

                    throw new NotImplementedException();
                }
            }

            public virtual void UpdateState(int remainingUses)
            {
                var active = condition.Evaluate(remainingUses);

                for (int i = 0; i < gameObjects.Length; i++)
                    gameObjects[i].SetActive(active);
            }
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
                targets[i].UpdateState(Ammo.RemainingUses);
        }

        void RefillCallback() => UpdateState();
        void ConsumptionCallback() => UpdateState();
    }
}