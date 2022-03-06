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

using MB;

namespace Game
{
	public class PlayerControls : Player.Module
	{
        public SingleAxisInput Primary { get; protected set; }
        public SingleAxisInput Secondary { get; protected set; }

        public ButtonInput Reload { get; protected set; }

        public AxisInput SwitchWeapon { get; protected set; }
        public ButtonInput SwitchActionMode { get; protected set; }
        public ButtonInput SwitchSight { get; protected set; }

        [field: SerializeField, DebugOnly]
        public List<PlayerInput> Inputs { get; protected set; }

        public CollectionData Collection { get; protected set; }
        [Serializable]
        public class CollectionData
        {
            public float Primary { get; protected set; }
            public float Secondary { get; protected set; }

            public bool Reload { get; protected set; }

            public float SwitchWeapon { get; protected set; }
            public bool SwitchActionMode { get; protected set; }
            public bool SwitchSight { get; protected set; }

            public virtual void Process(IList<PlayerInput> inputs)
            {
                Clear();

                for (int i = 0; i < inputs.Count; i++)
                {
                    Primary += inputs[i].Primary;
                    Secondary += inputs[i].Secondary;

                    Reload |= inputs[i].Reload;

                    SwitchWeapon += inputs[i].SwitchWeapon;
                    SwitchActionMode |= inputs[i].SwitchActionMode;
                    SwitchSight |= inputs[i].SwitchSight;
                }
            }

            protected virtual void Clear()
            {
                Primary = default;
                Secondary = default;

                Reload = default;

                SwitchWeapon = default;
                SwitchActionMode = default;
                SwitchSight = default;
            }
        }

        public override void Set(Player value)
        {
            base.Set(value);

            Inputs = Player.Behaviours.FindAll<PlayerInput>();
        }

        public override void Configure()
        {
            base.Configure();

            Collection = new CollectionData();

            Primary = new SingleAxisInput();
            Secondary = new SingleAxisInput();

            Reload = new ButtonInput();

            SwitchWeapon = new AxisInput();
            SwitchActionMode = new ButtonInput();
            SwitchSight = new ButtonInput();
        }
        public override void Initialize()
        {
            base.Initialize();

            Player.OnProcess += Process;
        }

        void Process()
        {
            Collection.Process(Inputs);

            Primary.Process(Collection.Primary);
            Secondary.Process(Collection.Secondary);

            Reload.Process(Collection.Reload);

            SwitchWeapon.Process(Collection.SwitchWeapon);
            SwitchActionMode.Process(Collection.SwitchActionMode);
            SwitchSight.Process(Collection.SwitchSight);
        }
    }
}