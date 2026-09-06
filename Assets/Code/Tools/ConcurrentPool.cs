using System;
using System.Collections.Concurrent;

public class ConcurrentPool
{
    private ConcurrentDictionary<Type, ConcurrentStack<IResettable>> concurrentPool = new ConcurrentDictionary<Type, ConcurrentStack<IResettable>>();

    public T Get<T>(params object[] parameters) where T : IResettable
    {
        Type resettableType = typeof(T);

        // If the pool doesn't contain a stack of the desired type, create it
        if (!concurrentPool.ContainsKey(resettableType))
            concurrentPool.TryAdd(resettableType, new ConcurrentStack<IResettable>());

        T value;
        // If the pool's stack contains 1 or more of the desired type, pop it
        if (concurrentPool[resettableType].Count > 0)
        {
            concurrentPool[resettableType].TryPop(out IResettable resettable);
            value = (T)resettable;
        }
        // Else, instantiate it
        else
        {
            value = (T)Activator.CreateInstance(resettableType);
        }

        value.Assign(parameters);
        return value;
    }

    public void Release<T>(T item) where T : IResettable
    {
        item.Reset();
        concurrentPool[typeof(T)].Push(item);
    }
}