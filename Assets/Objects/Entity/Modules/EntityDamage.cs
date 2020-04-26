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
	public class EntityDamage : Entity.Module
    {
        public Damage.Meta Meta { get; protected set; }

        public delegate void TakeDelegate(Damage.Result result);
        public event TakeDelegate OnTake;
        public virtual Damage.Result Take(Damage.IDamager source, Damage.Request request)
        {
            if (Entity.IsDead)
            {
                //TODO
            }
            else
            {
                Entity.Health.Value -= request.Value;

                if (Entity.Health.Value == 0f)
                    Entity.Death(source);
            }

            var result = new Damage.Result(source, Entity, request);

            OnTake?.Invoke(result);

            return result;
        }

        public delegate void DoDelegate(Damage.Result result);
        public event DoDelegate OnDo;
        public virtual Damage.Result Do(Damage.IDamagable target, Damage.Request request)
        {
            var result = Damage.Invoke(Entity, target, request);

            OnDo?.Invoke(result);

            return result;
        }

        public override void Configure(Entity reference)
        {
            base.Configure(reference);

            Meta = new Damage.Meta(Entity.gameObject);
        }
    }
}