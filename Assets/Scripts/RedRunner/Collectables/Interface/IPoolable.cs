using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void OnSpawn(PoolTag tag);
    void ReturnToPool();
}

