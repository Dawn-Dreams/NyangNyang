using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MiniGameBase
{
    public TextMeshProUGUI scoreText;          // ������ ǥ���� UI �ؽ�Ʈ
    public TextMeshProUGUI timerText;          // ���� �ð��� ǥ���� UI �ؽ�Ʈ
    public Button scoreButton;      // ���� �߰� ��ư
    public int targetScore = 10;    // ��ǥ ����
    public float timeLimit = 30f;   // ���� �ð�

    private int currentScore = 0;   // ���� ����
    private float remainingTime;    // ���� �ð�


    // �ʱ�ȭ ����
    void Start()
    {
        Initialize("�̴ϰ��� 1", 0); // �̴ϰ��� �̸��� �ʿ��� ������ �ε��� ����
        remainingTime = timeLimit;
        scoreButton.onClick.AddListener(OnScoreButtonClick);
        UpdateUI();
    }

    // �̴ϰ��� ���� ����
    protected override void StartGameLogic()
    {
        Debug.Log("�̴ϰ��� 1 ����");
        //GameManager.isMiniGameActive = true;
        remainingTime = timeLimit;
        currentScore = 0;
        UpdateUI();
    }

    // �� �����Ӹ��� �ð� üũ
    void Update()
    {
        if (GameManager.isMiniGameActive)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            // �ð��� �� �Ǿ��� �� ���� ����
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                EndGame();
            }
        }
    }

    // ���� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    private void OnScoreButtonClick()
    {
        if (GameManager.isMiniGameActive)
        {
            currentScore++;
            UpdateScoreUI();

            // ��ǥ ������ �������� �� ���� Ŭ����
            if (currentScore >= targetScore)
            {
                EndGame(true);
            }
        }
    }

    // �̴ϰ��� ���� ����
    protected override void EndGameLogic()
    {
        // Ŭ���� ���ο� ���� ó��
        //if (currentScore >= targetScore) Debug.Log("���� Ŭ����! ���� ����");
        //else Debug.Log("���� ����. �ٽ� �����ϼ���.");
        
        GameManager.isMiniGameActive = false;
    }

    // ���� ���� ȣ�� (���� ���θ� �Ķ���ͷ� ����)
    private void EndGame(bool isClear = false)
    {
        if (isClear)
        {
            ClearGame();
        }
        EndGameLogic();
    }

    // UI ������Ʈ �Լ���
    private void UpdateUI()
    {
        UpdateScoreUI();
        UpdateTimerUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = $"Score: {currentScore}/{targetScore}";
    }

    private void UpdateTimerUI()
    {
        timerText.text = $"Time: {remainingTime:F1} s";
    }
}
