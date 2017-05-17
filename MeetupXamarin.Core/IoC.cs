using System;
using System.Collections.Generic;

namespace MeetupXamarin.Core
{
    public static class IoC
    {
        private static Dictionary<Type, object> instances;
        private static Dictionary<string, object> namedInstances;

        static IoC()
        {
            instances = new Dictionary<Type, object>();
            namedInstances = new Dictionary<string, object>();
        }

        public static void Register<T>(object instance)
        {
            instances[typeof(T)] = instance;
        }

        public static T Resolve<T>()
        {
            return (T)instances[typeof(T)];
        }

        public static void Register<T>(string key, object instance)
        {
            namedInstances[key] = instance;
        }

        public static T Resolve<T>(string key)
        {
            return (T)namedInstances[key];
        }

        public static void Reset()
        {
            instances.Clear();
            namedInstances.Clear();
        }


    }
}
