using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pet : MonoBehaviour
{
    private AddressableHandle<GameObject> _petPrefab;
    private AnimationManager _currentPetAnimMgr;

    private EnemyMonsterType _petType;

    void Awake()
    {
        
    }

    public void SetPetType(EnemyMonsterType type)
    {
        if (_currentPetAnimMgr != null)
        {
            Destroy(_currentPetAnimMgr.gameObject);
            _currentPetAnimMgr = null;
            _petPrefab.Release();
            _petPrefab = null;
        }

        _petType = type;
        _petPrefab = new AddressableHandle<GameObject>().Load("Enemy/"+type);
        _currentPetAnimMgr = Instantiate(_petPrefab.obj, transform).GetComponent<AnimationManager>();
        PlayPetAnim(AnimationManager.AnimationState.IdleA);
    }

    public void PlayPetAnim(AnimationManager.AnimationState state)
    {
        if (_currentPetAnimMgr == null)
        {
            return;
        }
        _currentPetAnimMgr.PlayAnimation(state);
    }
}
