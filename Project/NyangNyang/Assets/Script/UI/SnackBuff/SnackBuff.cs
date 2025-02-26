using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SnackBuff : MonoBehaviour
{
    private static int _userMaxSnackBuffLevel = 10;

    public HideUi snackBuffUIPanelHideUI;

    public SnackPanel[] snackPanels;
    private SnackPanel _currentPickSnackPanel;
    [SerializeField] private TextMeshProUGUI snackBuffLevelText;
    [SerializeField] private Slider snackBuffLevelSlider;
    [SerializeField] private TextMeshProUGUI snackBuffLevelProgressText;

    private int _snackBuffAdViewCount;
    private int _currentSnackBuffLevel = 0;
    private int _requireLevelStep = 5;
    

    private Dictionary<SnackType, SnackPanel> _snackPanelDict = new Dictionary<SnackType, SnackPanel>();
    private Dictionary<SnackType, DateTime> _buffRemainTime = new Dictionary<SnackType, DateTime>();
    Coroutine BuffTimeCalculateCoroutine = null;
    

    void Start()
    {
        foreach (var snackPanel in snackPanels)
        {
            _snackPanelDict.Add(snackPanel.snackType, snackPanel);

            snackPanel.showAdButton.onClick.AddListener(() => OnClickShowAdButton(snackPanel));
        }

        // 정보 로드
        {
            SnackBuffJsonData data = new SnackBuffJsonData();
            SaveLoadManager.GetInstance().LoadPlayerSnackBuffData(out data);
            int adViewCount = data.snackBuffAdViewCount;

            if (data.buffRemainTime != null)
            {
                for (int i = 0; i < data.buffRemainTime.Count; ++i)
                {
                    SetActiveSnackBuffDataFromServer(data.buffRemainTime[i].type, data.buffRemainTime[i].time);
                }
            }

            SetSnackBuffAdViewCount(adViewCount);
        }
    }

    void SetSnackBuffAdViewCount(int newSnackBuffViewCount)
    {
        _snackBuffAdViewCount = newSnackBuffViewCount;

        // 레벨이 오른다면,
        if (_currentSnackBuffLevel != _snackBuffAdViewCount / _requireLevelStep)
        {
            _currentSnackBuffLevel = _snackBuffAdViewCount / _requireLevelStep;
            foreach (var snackPanel in snackPanels)
            {
                snackPanel.SetSnackBuffValueText(CalculateSnackBuffValue(snackPanel.snackType));
            }
        }
        

        snackBuffLevelText.text = _currentSnackBuffLevel.ToString();

        if (_currentSnackBuffLevel== _userMaxSnackBuffLevel)
        {
            snackBuffLevelProgressText.text = "최대 레벨";
            snackBuffLevelSlider.value = 1;
        }
        else
        {
            int requireAdViewCountForNextLevel = (_currentSnackBuffLevel + 1) * _requireLevelStep;
            snackBuffLevelProgressText.text = _snackBuffAdViewCount + " / " + requireAdViewCountForNextLevel;
            snackBuffLevelSlider.value = (float)_snackBuffAdViewCount / requireAdViewCountForNextLevel;
        }

        SaveDataToJson();
    }

    private void SaveDataToJson()
    {
        SnackBuffJsonData data = new SnackBuffJsonData();
        data.snackBuffAdViewCount = _snackBuffAdViewCount;
        data.buffRemainTime = new List<SnackBuffRemainTimeJsonData>();
        foreach (var buff in _buffRemainTime)
        {
            data.buffRemainTime.Add(new SnackBuffRemainTimeJsonData(buff.Key, buff.Value));
        }

        SaveLoadManager.GetInstance().SavePlayerSnackBuffData(data,2);
    }


    void OnClickShowAdButton(SnackPanel snackPanel)
    {
        _currentPickSnackPanel = snackPanel;

        GoogleMobileAdsManager.GetInstance().ShowRewardedAd(SnackBuffAdReward);
    }


    private void SnackBuffAdReward(Reward reward)
    {
        // 서버에게 플레이어가 광고 영상을 봤다는 것을 전송
        // 서버에서는 해당 플레이어에게 SetActiveSnackBuffDataFromServer 실행시키기.
        //Debug.Log($"플레이어가 {_currentPickSnackPanel.snackType} 광고 영상을 봤습니다.");
        if (_currentSnackBuffLevel < _userMaxSnackBuffLevel)
        {
            SetSnackBuffAdViewCount(_snackBuffAdViewCount + 1);
        }

        long buffEndDateTimeString = DateTime.UtcNow.AddMinutes(120).ToFileTime();
        Debug.Log(DateTime.Now.AddMinutes(120));
        SetActiveSnackBuffDataFromServer(_currentPickSnackPanel.snackType, buffEndDateTimeString);
    }

    public void SetActiveSnackBuffDataFromServer(SnackType snackType, long buffEndDateTimeLong)
    {
        _snackPanelDict[snackType].eatingImageObject.SetActive(true);


        DateTime buffEndDateTime = DateTime.FromFileTime(buffEndDateTimeLong);
        Debug.Log(buffEndDateTime);
        _buffRemainTime.Add(snackType, buffEndDateTime);

        // 해당 간식 버프 적용
        {
            Player.playerStatus.SetActiveSnackBuff(snackType, true, CalculateSnackBuffValue(snackType));
        }

        if (BuffTimeCalculateCoroutine == null)
        {
            BuffTimeCalculateCoroutine = StartCoroutine(BuffTimeCalculate());
        }

        SaveDataToJson();
    }

    private float CalculateSnackBuffValue(SnackType snackType)
    {
        float value = 1.0f;
        switch (snackType)
        {
            case SnackType.Atk:
                value += _currentSnackBuffLevel * 0.2f;
                break;
            case SnackType.Hp:
                value += _currentSnackBuffLevel * 0.2f;
                break;
            case SnackType.Gold:
                value += _currentSnackBuffLevel * 0.5f;
                break;
            default:
                Debug.LogError("타입 에러");
                break;
        }

        return value;
    }

    public void RecvBuffEndDataFromServer(SnackType snackType)
    {
        if (_buffRemainTime.ContainsKey(snackType))
        {
            _buffRemainTime.Remove(snackType);
        }
        Player.playerStatus.SetActiveSnackBuff(snackType, false);
        _snackPanelDict[snackType].eatingImageObject.SetActive(false);
    }

    IEnumerator BuffTimeCalculate()
    {
        while (true)
        {
            if (_buffRemainTime.Count == 0)
            {
                BuffTimeCalculateCoroutine = null;
                yield break;
            }
            else
            {
                DateTime now = DateTime.Now;
                
                // TODO 클라에서 종료를 계산하기위해 넣은 것, 서버 연결 시 삭제
                List<SnackType> endBuffTypes = new List<SnackType>();

                foreach (var buffRemainTimePair in _buffRemainTime)
                {
                    TimeSpan t = buffRemainTimePair.Value - now;
                    string remainTime;
                    if (t.TotalSeconds >= 0)
                    {
                        remainTime = string.Format("{0:D1}:{1:D2}:{2:D2}",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);
                    }
                    else
                    {
                        remainTime = "0:00:00";
                    }
                    
                    
                    _snackPanelDict[buffRemainTimePair.Key].remainTimeText.text = remainTime;

                    // 원래는 서버에서 적용되어야 하지만 임시로 클라에서 적용되도록 구현
                    
                    if ((int)t.TotalSeconds <= 0)
                    {
                        endBuffTypes.Add(buffRemainTimePair.Key);
                    }
                }

                foreach (SnackType type in endBuffTypes)
                {
                    RecvBuffEndDataFromServer(type);
                }
            }

            yield return new WaitForSeconds(1);
        }
        
    }
}
