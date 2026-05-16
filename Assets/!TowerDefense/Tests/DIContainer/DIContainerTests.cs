using NUnit.Framework;
using System;
using TToTT.Core.DI;

public class DIContainerTests
{
    private DIContainer _container;

    [SetUp]
    public void Setup()
    {
        _container = new DIContainer();
    }

    // =====================
    // Singleton
    // =====================

    [Test]
    public void Singleton_ReturnsSameInstance()
    {
        _container.Bind<ServiceA>(Lifetime.Singleton);

        var first = _container.Resolve<ServiceA>();
        var second = _container.Resolve<ServiceA>();

        Assert.AreSame(first, second,
            "Singleton must return the same copy");
    }

    // =====================
    // Transient
    // =====================

    [Test]
    public void Transient_ReturnsDifferentInstances()
    {
        _container.Bind<ServiceA>(Lifetime.Transient);

        var first = _container.Resolve<ServiceA>();
        var second = _container.Resolve<ServiceA>();

        Assert.AreNotSame(first, second,
            "Transient must return different instances");
    }

    // =====================
    // Interface binding
    // =====================

    [Test]
    public void BindInterface_ResolvesCorrectImplementation()
    {
        _container.Bind<IService, ServiceA>(Lifetime.Singleton);

        var resolved = _container.Resolve<IService>();

        Assert.IsInstanceOf<ServiceA>(resolved);
    }

    // =====================
    // BindInstance
    // =====================

    [Test]
    public void BindInstance_ReturnsExactInstance()
    {
        var instance = new ServiceA();
        _container.BindInstance<IService>(instance);

        var resolved = _container.Resolve<IService>();

        Assert.AreSame(instance, resolved);
    }

    // =====================
    // BindFactory
    // =====================

    [Test]
    public void BindFactory_Singleton_ReturnsSameInstance()
    {
        _container.BindFactory<IService>(
            c => new ServiceA(),
            Lifetime.Singleton);

        var first = _container.Resolve<IService>();
        var second = _container.Resolve<IService>();

        Assert.AreSame(first, second,
            "Factory Singleton should cache the result");
    }

    [Test]
    public void BindFactory_Transient_ReturnsDifferentInstances()
    {
        _container.BindFactory<IService>(
            c => new ServiceA(),
            Lifetime.Transient);

        var first = _container.Resolve<IService>();
        var second = _container.Resolve<IService>();

        Assert.AreNotSame(first, second);
    }

    // =====================
    // Auto dependency resolution
    // =====================

    [Test]
    public void Resolve_AutoResolvesDependencies()
    {
        _container.Bind<ServiceA>(Lifetime.Singleton);
        _container.Bind<ServiceB>(Lifetime.Singleton);

        var b = _container.Resolve<ServiceB>();

        Assert.IsNotNull(b);
        Assert.IsNotNull(b.Dependency,
            "ServiceB must get ServiceA through the constructor");
    }

    // =====================
    // Cyclic dependency
    // =====================

    [Test]
    public void Resolve_ThrowsOnCyclicDependency()
    {
        _container.Bind<CyclicA>(Lifetime.Singleton);
        _container.Bind<CyclicB>(Lifetime.Singleton);

        Assert.Throws<Exception>(() => _container.Resolve<CyclicA>(),
            "DI must detect cyclic dependence");
    }

    // =====================
    // Missing binding
    // =====================

    [Test]
    public void Resolve_ThrowsOnMissingConstructor()
    {
        Assert.Throws<Exception>(() => _container.Resolve<IService>(),
            "Resolving an interface without a binding should throw an exception");
    }

    // =====================
    // Singleton shared across interfaces
    // =====================

    [Test]
    public void Factory_Singleton_SameInstanceForAliasedTypes()
    {
        _container.Bind<ServiceA>(Lifetime.Singleton);
        _container.BindFactory<IService>(
            c => c.Resolve<ServiceA>(),
            Lifetime.Singleton);

        var direct = _container.Resolve<ServiceA>();
        var aliased = _container.Resolve<IService>();

        Assert.AreSame(direct, aliased,
            "Alias via BindFactory should return the same Singleton");
    }

    // =====================
    // Test helpers
    // =====================

    private interface IService { }

    private class ServiceA : IService { }

    private class ServiceB
    {
        public readonly ServiceA Dependency;
        public ServiceB(ServiceA dependency) { Dependency = dependency; }
    }

    private class CyclicA
    {
        public CyclicA(CyclicB b) { }
    }

    private class CyclicB
    {
        public CyclicB(CyclicA a) { }
    }
}