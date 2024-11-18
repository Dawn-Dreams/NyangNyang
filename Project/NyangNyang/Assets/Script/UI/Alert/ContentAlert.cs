using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AlertState
{
    // 평상시, 잠금, 콘텐츠알림
    Null, Locked, RedDot, Count
}

public class ContentAlert : MonoBehaviour
{
    private Dictionary<AlertState, AddressableHandle<GameObject>> _alertIcons = new Dictionary<AlertState, AddressableHandle<GameObject>>();
    private Image _targetImage;

    private AlertState _currentAlertState = AlertState.Null;
    private GameObject _alertIconObj = null;

    void Awake()
    {
        _alertIcons.Add(AlertState.Locked, new AddressableHandle<GameObject>().Load("Alert/Locked"));
        _alertIcons.Add(AlertState.RedDot, new AddressableHandle<GameObject>().Load("Alert/RedDot"));

        gameObject.TryGetComponent<Button>(out var tempButton);
        if (tempButton != null)
        {
            _targetImage = tempButton.image;
        }
        else
        {
            _targetImage = gameObject.GetComponent<Image>();
        }
    }

    public void ChangeAlertState(AlertState newState)
    {
        if (newState == _currentAlertState)
        {
            return;
        }

        if (newState != _currentAlertState)
        {
            if (_alertIconObj != null)
            {
                Destroy(_alertIconObj);
            }
            _currentAlertState = newState;
        }

        _alertIconObj = Instantiate(_alertIcons[_currentAlertState].obj, transform);
        if (_currentAlertState == AlertState.Locked)
        {
            _targetImage.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            _targetImage.color = new Color(1,1,1);
        }
    }

}
