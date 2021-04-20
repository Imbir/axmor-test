using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public static class ViewHolder
    {
        private static readonly Dictionary<Type, MonoBehaviour> _viewElements = new Dictionary<Type, MonoBehaviour>();

        public static void AddView(MonoBehaviour view)
        {
            var type = view.GetType();
            if (!_viewElements.ContainsKey(type))
                _viewElements.Add(type, view);
        }
        
        public static T GetView<T>() where T : MonoBehaviour
        {
            var key = typeof(T);
            return _viewElements.ContainsKey(key) ? _viewElements[key] as T : default;
        }
    }
}