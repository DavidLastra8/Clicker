using System.Collections.Generic;
using CookieClicker.Core;
using UnityEngine;

namespace CookieClicker.Runtime
{
    /// <summary>
    /// Único puente entre el Update de Unity y la lógica en C# puro (ayudantes, producción pasiva...).
    /// </summary>
    public sealed class TickRunner : MonoBehaviour
    {
        private readonly List<ITickable> _tickables = new();

        public void Register(ITickable tickable)
        {
            if (tickable != null && !_tickables.Contains(tickable))
                _tickables.Add(tickable);
        }

        public void Unregister(ITickable tickable) => _tickables.Remove(tickable);

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < _tickables.Count; i++)
                _tickables[i].Tick(deltaTime);
        }
    }
}
