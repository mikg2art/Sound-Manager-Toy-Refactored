using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSoundEvent : MonoBehaviour, IPoolable
{
    #region IPoolableVeariables
    private string _poolTag;
       
    public string PoolTag
    {
        get { return _poolTag; }
        set { _poolTag = value; }
    }

    #endregion

    #region IPoolableFunctions
    public void OnObjectSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnObjectDespawn(float time = 0)
    {
        gameObject.GetComponent<AudioSource>().Stop();
        gameObject.SetActive(false);
    }
    #endregion
}
