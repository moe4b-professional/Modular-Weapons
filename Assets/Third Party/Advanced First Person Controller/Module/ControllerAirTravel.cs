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
	public class ControllerAirTravel : FirstPersonController.Module
	{
        public Data.Builder DataBuilder { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            DataBuilder = new Data.Builder();
        }

        public virtual void Begin()
        {
            DataBuilder.Start = new Data.StartData(Controller.transform.position);
        }

        public virtual Data End()
        {
            DataBuilder.End = new Data.EndData(Controller.transform.position);

            return DataBuilder.Build();
        }

        [Serializable]
        public struct Data
        {
            public StartData Start { get; private set; }
            [Serializable]
            public struct StartData
            {
                public Vector3 Position { get; private set; }

                public StartData(Vector3 position)
                {
                    this.Position = position;
                }
            }

            public EndData End { get; private set; }
            [Serializable]
            public struct EndData
            {
                public Vector3 Position { get; private set; }

                public EndData(Vector3 position)
                {
                    this.Position = position;
                }
            }

            public float Distance { get; private set; }

            public Data(StartData start, EndData end)
            {
                this.Start = start;
                this.End = end;

                Distance = Vector3.Distance(start.Position, end.Position);
            }

            [Serializable]
            public class Builder
            {
                public StartData Start { get; set; }

                public EndData End { get; set; }

                public Data Build()
                {
                    return new Data(Start, End);
                }
            }
        }
    }
}