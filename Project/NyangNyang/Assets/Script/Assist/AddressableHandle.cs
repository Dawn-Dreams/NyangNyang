using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using Unity.Collections;
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


public class AddressableHandleAssets<T> where T : UnityEngine.Object
{
    public AsyncOperationHandle<IList<T>> assetsHandle;
    public List<T> objs;

    // 에셋들 로드 함수
    public void LoadAssets(string key, List<string> sortStandard)
    {
        if (sortStandard == null)
        {
            return;
        }

        // 리스트 할당
        objs = new T[sortStandard.Count].ToList();

        // 에셋 로드
        assetsHandle = Addressables.LoadAssetsAsync<T>(key, (result) =>
        {
            // TODO: 모든 string을 찾는 방법을 추후 최적화된 방법으로 교체
            for (int i = 0; i < sortStandard.Count; i++)
            {
                if (result.name.Contains(sortStandard[i]))
                {
                    objs[i] = result;
                }
            }
        });
        assetsHandle.WaitForCompletion();

    }

}