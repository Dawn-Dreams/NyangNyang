using System;
using System.Collections;
using System.Numerics;
using TMPro;
using UnityEngine;

public class CombatPowerManager : MonoBehaviour
{
    private static CombatPowerManager _instance;
    public static CombatPowerManager GetInstance()
    {
        return _instance;
    }

    private BigInteger _currentAttackPower = 0;
    private BigInteger _currentDefencePower = 0;

    [SerializeField]
    private TextMeshProUGUI playerAttackPowerText;
    [SerializeField]
    private TextMeshProUGUI playerDefencePowerText;

    [SerializeField] 
    private GameObject combatPowerChangeUIObject;
    [SerializeField] 
    private TextMeshProUGUI attackPowerText;
    [SerializeField]
    private TextMeshProUGUI defencePowerText;
    [SerializeField]
    private TextMeshProUGUI attackPowerChangeText;
    [SerializeField]
    private TextMeshProUGUI defencePowerChangeText;

    private Coroutine _activeCoroutine = null;
    private Coroutine _inactiveCoroutine = null;
    private float _activeTime = 0.0f;

    private bool startUI = false;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        _currentAttackPower = Player.playerStatus.GetCurrentAttackPower();
        _currentDefencePower = Player.playerStatus.GetCurrentDefencePower();
        playerAttackPowerText.text = MyBigIntegerMath.GetAbbreviationFromBigInteger(_currentAttackPower);
        playerDefencePowerText.text = MyBigIntegerMath.GetAbbreviationFromBigInteger(_currentDefencePower);
        startUI = true;
    }

    public void ChangeCurrentCombatPower(BigInteger newAttackPower, BigInteger newDefencePower)
    {
        if (!startUI)
        {
            return;
        }

        if (_activeCoroutine != null)
        {
            StopCoroutine(_activeCoroutine);
            _activeCoroutine = null;
        }


        // 초기 세팅
        combatPowerChangeUIObject.SetActive(false);
        if (_activeCoroutine == null)
        {
            _activeCoroutine = StartCoroutine(ActiveUI(newAttackPower, newDefencePower));
        }
    }

    private void SetChangeValueText(TextMeshProUGUI textComp, BigInteger newVal)
    {
        if (newVal == 0)
        {
            textComp.text = "-";
            textComp.color = Color.white;
        }
        else
        {
            textComp.text = MyBigIntegerMath.GetAbbreviationFromBigInteger(newVal);
            textComp.color = newVal > 0 ? Color.green : Color.red;
        }
    }

    IEnumerator ActiveUI(BigInteger newAttackPower, BigInteger newDefencePower)
    {
        yield return null;

        string newAttackPowerStr = MyBigIntegerMath.GetAbbreviationFromBigInteger(_currentAttackPower);
        string newDefencePowerStr = MyBigIntegerMath.GetAbbreviationFromBigInteger(_currentDefencePower);

        // 메인 UI
        playerAttackPowerText.text = newAttackPowerStr;
        playerDefencePowerText.text = newDefencePowerStr;

        // 변경알림 UI 값 변경
        BigInteger diffAttackVal = newAttackPower - _currentAttackPower;
        BigInteger diffDefenceVal = newDefencePower - _currentDefencePower;
        _currentAttackPower = newAttackPower;
        _currentDefencePower = newDefencePower;

        attackPowerText.text = newAttackPowerStr;
        defencePowerText.text = newDefencePowerStr;

        SetChangeValueText(attackPowerChangeText, diffAttackVal);
        SetChangeValueText(defencePowerChangeText, diffDefenceVal);
        
        combatPowerChangeUIObject.SetActive(true);
        _activeTime = Time.time;
        _activeCoroutine = null;
        if (_inactiveCoroutine == null)
        {
            _inactiveCoroutine = StartCoroutine(InactivateUI());
        }
        
    }

    IEnumerator InactivateUI()
    {
        while (true)
        {
            if (Time.time - _activeTime >= 2.0f)
            {
                combatPowerChangeUIObject.SetActive(false);
                _inactiveCoroutine = null;
                yield break;
            }

            yield return null;
        }
    }
}
