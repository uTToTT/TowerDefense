using System;
using System.Collections.Generic;
using System.Linq;

public class DIContainer
{
    private readonly Dictionary<Type, Binding> _bindings = new();

    // =========================
    // Bind
    // =========================
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
        return (T)Resolve(typeof(T), new HashSet<Type>());
    }

    private object Resolve(Type type, HashSet<Type> resolvingStack)
    {
        if (resolvingStack.Contains(type))
            throw new Exception($"Cyclic dependency detected: {type}");

        resolvingStack.Add(type);

        if (!_bindings.TryGetValue(type, out var binding))
        {
            return CreateInstance(type, resolvingStack);
        }

        // Singleton
        if (binding.Lifetime == Lifetime.Singleton)
        {
            if (binding.Instance == null)
            {
                binding.Instance = CreateBindingInstance(binding, resolvingStack);
            }

            return binding.Instance;
        }

        // Transient
        return CreateBindingInstance(binding, resolvingStack);
    }

    private object CreateBindingInstance(Binding binding, HashSet<Type> resolvingStack)
    {
        // Factory
        if (binding.Instance is Func<DIContainer, object> factory)
        {
            return factory(this);
        }

        return CreateInstance(binding.ImplementationType, resolvingStack);
    }

    private object CreateInstance(Type type, HashSet<Type> resolvingStack)
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
            args[i] = Resolve(parameters[i].ParameterType, resolvingStack);
        }

        var instance = Activator.CreateInstance(type, args);

        resolvingStack.Remove(type);

        return instance;
    }
}
