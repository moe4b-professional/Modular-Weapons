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
	public interface IModules
	{
		TTarget Find<TTarget>()
			where TTarget : class;

		List<TTarget> FindAll<TTarget>()
			where TTarget : class;
	}

	[Serializable]
	public class Modules<TReference> : Modules<TReference, IModule<TReference>>
		where TReference : Component
	{
		public Modules(TReference reference) : base(reference) { }
	}

	[Serializable]
	public abstract class Modules<TReference, TModule> : IModules
		where TReference : Component
		where TModule : class, IModule<TReference>
	{
		public TReference Reference { get; protected set; }

		public List<TModule> List { get; protected set; }

		public virtual void Add(TModule module)
		{
			List.Add(module);
		}
		public virtual void Remove(TModule module)
		{
			List.Remove(module);
		}

		public virtual void Register(IBehaviours behaviours) => Register(behaviours, ModuleScope.Local);
		public virtual void Register(IBehaviours behaviours, ModuleScope scope)
        {
			var selection = behaviours.FindAll<TModule>();

			for (int i = 0; i < selection.Count; i++)
				if (ValidateScope(Reference, selection[i], scope))
					Add(selection[i]);
		}

		public void Set()
		{
			for (int i = 0; i < List.Count; i++)
				List[i].Set(Reference);
		}

		#region Iteration
		public void ForAll(Action<TModule> action)
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

			var module = typeof(TModule);

			if (module.IsAssignableFrom(type) == false)
				throw new Exception($"Invalid Query For {type.Name} Within Collection of {module.Name}'s" +
					$", Please Ensure that {type} Inherits from {module.Name}");
#endif
		}
		#endregion

		public TTarget Depend<TTarget>()
			where TTarget : class
		{
			var target = Find<TTarget>();

			if (target == null)
				throw new NullReferenceException($"No Component of Type {typeof(TTarget)} found on {Reference}");

			return target;
		}

		public Modules(TReference reference)
		{
			this.Reference = reference;

			List = new List<TModule>();
		}

		public static bool ValidateScope(TReference reference, TModule module, ModuleScope scope)
		{
			switch (scope)
			{
				case ModuleScope.Local:
					return module.transform.IsChildOf(reference.transform);

				case ModuleScope.Global:
					return true;
			}

			throw new NotImplementedException();
		}
    }

	public enum ModuleScope
	{
		Local, Global
	}

	public interface IModule<TReference>
	{
		Transform transform { get; }

		void Set(TReference value);
	}
}