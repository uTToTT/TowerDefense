using System;

public interface IPoolable : IDisposable
{
    bool IsActive { get; set; }
}
