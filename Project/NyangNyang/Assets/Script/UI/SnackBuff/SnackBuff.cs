using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class SnackBuff : MonoBehaviour
{
    public RectTransform snackBuffUIPanelRectTransform;

    public SnackPanel[] snackPanels;
    private SnackPanel _currentPickSnackPanel;

    private Dictionary<SnackType, SnackPanel> _snackPanelDict = new Dictionary<SnackType, SnackPanel>();
    private Dictionary<SnackType, DateTime> _buffRemainTime = new Dictionary<SnackType, DateTime>();
    Coroutine BuffTimeCalculateCoroutine = null;
    

    void Start()
    {
        // 서버로부터 정보 받기
        {

        }

        foreach (var snackPanel in snackPanels)
        {
            _snackPanelDict.Add(snackPanel.snackType,snackPanel);

            snackPanel.showAdButton.onClick.AddListener(() => OnClickShowAdButton(snackPanel));
        }

        // UI 위치 갱신 및 SetActive(false)
        {
            snackBuffUIPanelRectTransform.offsetMin = snackBuffUIPanelRectTransform.offsetMax = new Vector2(0, 0);
            snackBuffUIPanelRectTransform.gameObject.SetActive(false);
        }
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
        string buffEndDateTimeString = DateTime.Now.AddSeconds(10).ToString("yyyy/MM/dd tt hh:mm:ss");
        SetActiveSnackBuffDataFromServer(_currentPickSnackPanel.snackType, buffEndDateTimeString);
    }

    public void SetActiveSnackBuffDataFromServer(SnackType snackType, string buffEndDateTimeString)
    {
        _snackPanelDict[snackType].eatingImageObject.SetActive(true);

        DateTime buffEndDateTime = DateTime.Parse(buffEndDateTimeString);
        _buffRemainTime.Add(snackType, buffEndDateTime);
        if (BuffTimeCalculateCoroutine == null)
        {
            BuffTimeCalculateCoroutine = StartCoroutine(BuffTimeCalculate());
        }
    }

    public void RecvBuffEndDataFromServer(SnackType snackType)
    {
        if (_buffRemainTime.ContainsKey(snackType))
        {
            _buffRemainTime.Remove(snackType);
        }
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
