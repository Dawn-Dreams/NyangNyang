using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MiniGameBase
{
    public TextMeshProUGUI scoreText;          // 점수를 표시할 UI 텍스트
    public TextMeshProUGUI timerText;          // 남은 시간을 표시할 UI 텍스트
    public Button scoreButton;      // 점수 추가 버튼
    public int targetScore = 10;    // 목표 점수
    public float timeLimit = 30f;   // 제한 시간

    private int currentScore = 0;   // 현재 점수
    private float remainingTime;    // 남은 시간


    // 초기화 로직
    void Start()
    {
        Initialize("미니게임 1", 0); // 미니게임 이름과 필요한 소탕권 인덱스 설정
        remainingTime = timeLimit;
        scoreButton.onClick.AddListener(OnScoreButtonClick);
        UpdateUI();
    }

    // 미니게임 시작 로직
    protected override void StartGameLogic()
    {
        Debug.Log("미니게임 1 시작");
        //GameManager.isMiniGameActive = true;
        remainingTime = timeLimit;
        currentScore = 0;
        UpdateUI();
    }

    // 매 프레임마다 시간 체크
    void Update()
    {
        if (GameManager.isMiniGameActive)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            // 시간이 다 되었을 때 게임 종료
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                EndGame();
            }
        }
    }

    // 점수 버튼 클릭 시 호출되는 함수
    private void OnScoreButtonClick()
    {
        if (GameManager.isMiniGameActive)
        {
            currentScore++;
            UpdateScoreUI();

            // 목표 점수에 도달했을 때 게임 클리어
            if (currentScore >= targetScore)
            {
                EndGame(true);
            }
        }
    }

    // 미니게임 종료 로직
    protected override void EndGameLogic()
    {
        // 클리어 여부에 따른 처리
        //if (currentScore >= targetScore) Debug.Log("게임 클리어! 보상 지급");
        //else Debug.Log("게임 실패. 다시 도전하세요.");
        
        GameManager.isMiniGameActive = false;
    }

    // 게임 종료 호출 (성공 여부를 파라미터로 받음)
    private void EndGame(bool isClear = false)
    {
        if (isClear)
        {
            ClearGame();
        }
        EndGameLogic();
    }

    // UI 업데이트 함수들
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
