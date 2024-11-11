using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.U2D;

public class AddressableHandle<T>
{
    public AsyncOperationHandle<T> handle;
    public T obj;

    public AddressableHandle<T> Load(string key)
    {
        handle = Addressables.LoadAssetAsync<T>(key);
        handle.WaitForCompletion();
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            //Debug.Log("key is not valid [" + key + "] but it can be on null like \"NotEquip\"");
            return this;
        }
        obj = handle.Result;

        return this;
    }


    public void Release()
    {
        if (handle.IsValid())
        {
            handle.Release();
        }
        
    }
}


public class AddressableHandleAssets<T> where T : UnityEngine.Object
{
    public AsyncOperationHandle<IList<T>> assetsHandle;
    public List<T> objs;

    public void LoadAssets(string key)
    {
        // 리스트 할당
        objs = new List<T>();

        // 에셋 로드
        assetsHandle = Addressables.LoadAssetsAsync<T>(key, (result) => {objs.Add(result); });
            
        assetsHandle.WaitForCompletion();
    }
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
            Debug.Log($"{key} -> {result.name}");
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


    public void Release()
    {
        if (assetsHandle.IsValid())
        {
            Addressables.Release(assetsHandle);
        }
    }

}

//public class AddressableManager
//{
//    //https://discussions.unity.com/t/can-i-ask-addressables-if-a-specified-key-addressable-name-is-valid/719356/2
//    private static Dictionary<string, IResourceLocator> _locators;
//
//    public static bool AddressableResourceExists(string locatorKey, string key)
//    {
//        if (!_locators.ContainsKey(locatorKey))
//        {
//            ResourceLocatorInfo locatorInfo = Addressables.GetLocatorInfo(locatorKey);
//            if (locatorInfo != null)
//            {
//                _locators.Add(locatorKey, locatorInfo.Locator);
//            }
//        }
//
//        IList<IResourceLocation> location;
//        if (_locators[locatorKey].Locate(key, null, out location))
//        {
//            return true;
//        }
//
//        return false;
//    }
//}