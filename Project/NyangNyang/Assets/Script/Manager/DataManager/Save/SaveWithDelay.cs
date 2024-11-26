using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SaveDataType
{
    StatusLevel, CurrencyData, LevelData, SnackBuff,PlayerCostume, Count
}

public class SaveWithDelay
{
    // 저장 시, 바로바로 저장이 되는 방식이 아니라,
    // 저장 요청 후 delayTime 내에 다시 요청 시,
    // 이전 데이터는 저장되지 않고 다시 새로운 값으로 delayTime 이후 저장이 되도록 진행하는 클래스
    public SaveDataType dataType;
    public float startTime;
    public float delayTime;
    public Action saveFunctionCallback;
    

    public SaveWithDelay(SaveDataType type, Action saveAction, float newDelayTime = 5.0f)
    {
        dataType = type;
        saveFunctionCallback = saveAction;
        startTime = Time.time;
        delayTime = newDelayTime;
    }

}
