using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interfaces;
using UnityEngine;

namespace Core.Initialization
{
    public class HubAssigner: MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour>  assignedObjects = new List<MonoBehaviour>();
        [Obsolete("Obsolete")]
        private void Awake()
        {
            ExecuteAssign();
        }

        [Obsolete("Obsolete")]
        private void ExecuteAssign()
        {
            var injectables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<ISingle>();

            var hubFields = typeof(Hub).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var injectable in injectables)
            {
                var targetField = hubFields.FirstOrDefault(f => f.FieldType.IsInstanceOfType(injectable));

                if (targetField == null) continue;
                targetField.SetValue(null, injectable);
                assignedObjects.Add((MonoBehaviour)injectable);
            }
        }
    }
}