using System;
using System.Collections.Generic;
using System.Linq;

namespace TToTT.Core.DI
{
    public class DIContainer
    {
        private readonly Dictionary<Type, Binding> _bindings = new();

        // =========================
        // Bind
        // =========================
        public void Bind<T>(Lifetime lifetime = Lifetime.Transient) => Bind<T, T>(lifetime);

        public void Bind<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient)
        {
            _bindings[typeof(TInterface)] = new Binding
            {
                InterfaceType = typeof(TInterface),
                ImplementationType = typeof(TImplementation),
                Lifetime = lifetime
            };
        }

        public void BindInstance<TInterface>(TInterface instance)
        {
            _bindings[typeof(TInterface)] = new Binding
            {
                InterfaceType = typeof(TInterface),
                ImplementationType = instance.GetType(),
                Lifetime = Lifetime.Singleton,
                Instance = instance
            };
        }

        public void BindFactory<TInterface>(Func<DIContainer, object> factory, Lifetime lifetime)
        {
            _bindings[typeof(TInterface)] = new Binding
            {
                InterfaceType = typeof(TInterface),
                ImplementationType = null,
                Lifetime = lifetime,
                Instance = factory
            };
        }

        // =========================
        // Resolve
        // =========================
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T), new HashSet<Type>(), new List<Type>());
        }

        private object Resolve(Type type, HashSet<Type> resolvingStack, List<Type> resolvingPath)
        {
            if (resolvingStack.Contains(type))
            {
                var cycleStart = resolvingPath.IndexOf(type);
                var cyclePath = resolvingPath
                    .Skip(cycleStart)
                    .Select(t => t.Name)
                    .ToList();
                cyclePath.Add(type.Name); 

                var pathStr = string.Join(" -> ", cyclePath);
                throw new Exception($"Cyclic dependency:\n{pathStr}");
            }

            resolvingStack.Add(type);
            resolvingPath.Add(type);

            iobject result;

            if (!_bindings.TryGetValue(type, out var binding))
            {
                result = CreateInstance(type, resolvingStack, resolvingPath);
            }
            else if (binding.Lifetime == Lifetime.Singleton)
            {
                if (binding.Instance == null)
                    binding.Instance = CreateBindingInstance(binding, resolvingStack, resolvingPath);
                result = binding.Instance;
            }
            else
            {
                result = CreateBindingInstance(binding, resolvingStack, resolvingPath);
            }

            resolvingStack.Remove(type);
            resolvingPath.RemoveAt(resolvingPath.Count - 1);

            return result;
        }

        private object CreateBindingInstance(Binding binding, HashSet<Type> resolvingStack, List<Type> resolvingPath)
        {
            // Factory
            if (binding.Instance is Func<DIContainer, object> factory)
            {
                return factory(this);
            }

            return CreateInstance(binding.ImplementationType, resolvingStack, resolvingPath);
        }

        private object CreateInstance(Type type, HashSet<Type> resolvingStack, List<Type> resolvingPath)
        {
            var constructors = type.GetConstructors();

            if (constructors.Length == 0)
                throw new Exception($"No public constructors for {type}");

            var constructor = constructors
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            var parameters = constructor.GetParameters();

            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Resolve(parameters[i].ParameterType, resolvingStack, resolvingPath);
            }

            return Activator.CreateInstance(type, args);
        }
    }
}