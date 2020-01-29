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
    public interface IDamagable
    {
        Damage.Result TakeDamage(Damage.Request request);
    }

    public interface IDamager
    {

    }

    public static class Damage
    {
        public static Result? Invoke(GameObject target, Request request)
        {
            var damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                return null;

            return Invoke(damagable, request);
        }
        public static Result Invoke(IDamagable target, Request request)
        {
            return target.TakeDamage(request);
        }

        public struct Request
        {
            public IDamager Source { get; private set; }

            public float Value { get; private set; }

            public Method Method { get; private set; }

            public Request(IDamager cause, float value, Method method)
            {
                this.Source = cause;
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

            public Result(IDamagable target, Request request)
            {
                this.Source = request.Source;
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