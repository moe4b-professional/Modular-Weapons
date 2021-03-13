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
	public interface IBehaviours
	{
		TTarget Find<TTarget>()
			where TTarget : class;

		List<TTarget> FindAll<TTarget>()
			where TTarget : class;
	}

	[Serializable]
	public class Behaviours<TReference> : Behaviours<TReference, IBehaviour<TReference>>
		where TReference : Component
	{
		public Behaviours(TReference reference) : base(reference) { }
	}

	[Serializable]
	public abstract class Behaviours<TReference, TBehaviour> : IBehaviours
		where TReference : Component
		where TBehaviour : IBehaviour<TReference>
	{
		public TReference Reference { get; protected set; }

		public List<TBehaviour> List { get; protected set; }

		public class Lookup : List<TBehaviour> { }

		public virtual void Add(TBehaviour behaviour)
		{
			List.Add(behaviour);
		}

		#region Iteration
		public void ForAll(Action<TBehaviour> action)
		{
			for (int i = 0; i < List.Count; i++)
				action(List[i]);
		}

		public void ForAll<TTarget>(Action<TTarget> action)
			where TTarget : class
		{
			for (int i = 0; i < List.Count; i++)
				if (List[i] is TTarget target)
					action(target);
		}
		#endregion

		#region Query
		public TTarget Find<TTarget>()
			where TTarget : class
		{
			ValidateQuery<TTarget>();

			for (int i = 0; i < List.Count; i++)
				if (List[i] is TTarget target)
					return target;

			return null;
		}

		public List<TTarget> FindAll<TTarget>()
			where TTarget : class
		{
			ValidateQuery<TTarget>();

			var selection = new List<TTarget>();

			for (int i = 0; i < List.Count; i++)
				if (List[i] is TTarget target)
					selection.Add(target);

			return selection;
		}

		public void ValidateQuery<T>()
		{
#if UNITY_EDITOR
			var type = typeof(T);
			if (type.IsInterface) return;

			var behaviour = typeof(TBehaviour);

			if (behaviour.IsAssignableFrom(type) == false)
				throw new Exception($"Invalid Query For {type} Within Collection of {behaviour}'s" +
					$", Please Ensure that {type} Inherits from {behaviour}");
#endif
		}
		#endregion

		public virtual void Configure()
		{
			for (int i = 0; i < List.Count; i++)
				List[i].Configure();
		}

		public virtual void Init()
		{
			for (int i = 0; i < List.Count; i++)
				List[i].Init();
		}

		public Behaviours(TReference reference)
		{
			List = new List<TBehaviour>();

			var selection = reference.GetComponentsInChildren<TBehaviour>(true);

			for (int i = 0; i < selection.Length; i++)
				Add(selection[i]);
		}
	}

	public interface IBehaviour<TReference>
	{
		void Configure();

		void Init();
	}
}