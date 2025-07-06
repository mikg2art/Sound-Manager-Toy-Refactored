using UnityEngine;

public interface IPoolable
{
    string PoolTag { get; set; }
    void OnObjectSpawn();
    void OnObjectDespawn(float time = 0);
}
