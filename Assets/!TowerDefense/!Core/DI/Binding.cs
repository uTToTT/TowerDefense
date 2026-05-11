using System;
using TToTT.Core.DI;

public class Binding
{
    public Type InterfaceType;
    public Type ImplementationType;
    public Lifetime Lifetime;

    public object Instance;
    public Func<DIContainer, object> Factory;
}