using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class AddressableHandle<T>
{
    public AsyncOperationHandle<T> handle;
    public T obj;

    public void Load(string key)
    {
        handle = Addressables.LoadAssetAsync<T>(key);
        handle.WaitForCompletion();
        obj = handle.Result;
    }

    public void Release()
    {
        handle.Release();
    }
}
