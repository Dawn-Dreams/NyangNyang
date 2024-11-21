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
    private Button _targetButton;

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
            _targetButton = tempButton;
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

        if (_alertIcons.ContainsKey(newState))
        {
            _alertIconObj = Instantiate(_alertIcons[_currentAlertState].obj, transform);
        }
        
        _targetImage.color = _currentAlertState == AlertState.Locked ? new Color(0.5f, 0.5f, 0.5f, 1.0f) : new Color(1,1,1, 1.0f);

        if (_targetButton != null)
        {
            _targetButton.interactable = _currentAlertState != AlertState.Locked;
        }
    }

    public AlertState GetCurrentAlertState()
    {
        return _currentAlertState;
    }
}
