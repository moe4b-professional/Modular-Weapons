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
    public static class Damage
    {
        public interface IInterface
        {
            Meta Meta { get; }
        }

        public interface IDamagable : IInterface
        {
            Result TakeDamage(IDamager source, Request request);
        }

        public interface IDamager : IInterface
        {
            Result DoDamage(IDamagable target, Request request);
        }

        public class Meta
        {
            public IList<IInterface> List { get; protected set; }

            public virtual TType Find<TType>()
                where TType : class, IInterface
            {
                for (int i = 0; i < List.Count; i++)
                    if (List[i] is TType)
                        return List[i] as TType;

                return null;
            }
            public virtual void Find<TType>(out TType variable)
                where TType : class, IInterface
            {
                variable = Find<TType>();
            }

            public virtual bool Contains<TType>()
                where TType : class, IInterface
            {
                var target = Find<TType>();

                return target != null;
            }

            public interface IInterface
            {

            }

            public Meta(GameObject gameObject)
            {
                List = gameObject.GetComponentsInChildren<IInterface>();
            }
        }

        public static Result Invoke(IDamager source, IDamagable target, Request request)
        {
            return target.TakeDamage(source, request);
        }

        public struct Request
        {
            public float Value { get; private set; }

            public Method Method { get; private set; }

            public Request(float value, Method method)
            {
                this.Value = value;
                this.Method = method;
            }
        }

        public struct Result
        {
            public IDamager Source { get; private set; }

            public IDamagable Target { get; private set; }

            public float Value { get; private set; }

            public Method Method { get; private set; }

            public Result(IDamager source, IDamagable target, Request request)
            {
                this.Source = source;
                this.Target = target;
                this.Value = request.Value;
                this.Method = request.Method;
            }
        }

        public enum Method
        {
            Undefined, Contact, Projectile
        }
    }
}