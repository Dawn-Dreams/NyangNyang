using UnityEngine;
using TMPro;

public class MiniGame1 : MiniGameBase
{
    public int width = 8;               // 보드의 너비
    public int height = 8;              // 보드의 높이
    public GameObject[] tiles;          // 사용할 타일들
    public TextMeshProUGUI scoreText;   // 점수 UI
    public TextMeshProUGUI timerText;   // 타이머 UI

    private GameObject[,] grid;         // 게임 보드
    private int score = 0;              // 점수
    private float timeLimit = 60f;      // 제한 시간
    private float remainingTime;        // 남은 시간

    void Start()
    {
        Initialize("매치-3 미니게임", 1); // 미니게임 이름과 보상 티켓 인덱스
        grid = new GameObject[width, height];
        GenerateBoard();
        remainingTime = timeLimit;
        UpdateUI();
    }

    // 미니게임 시작 로직
    protected override void StartGameLogic()
    {
        Debug.Log("매치-3 미니게임 시작");
        remainingTime = timeLimit;
        score = 0;
        UpdateUI();
    }

    void Update()
    {
        if (GameManager.isMiniGameActive)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                EndGameLogic();
            }
        }
    }

    // 보드 위에 타일을 랜덤으로 배치
    private void GenerateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y);
                int randomTile = Random.Range(0, tiles.Length);
                GameObject tile = Instantiate(tiles[randomTile], position, Quaternion.identity);
                grid[x, y] = tile;
            }
        }
    }

    // 매치-3 검사 로직
    private void CheckForMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = grid[x, y];

                // 가로 매칭 검사
                if (x < width - 2 && grid[x + 1, y].tag == tile.tag && grid[x + 2, y].tag == tile.tag)
                {
                    Destroy(grid[x, y]);
                    Destroy(grid[x + 1, y]);
                    Destroy(grid[x + 2, y]);
                    score += 10;
                }

                // 세로 매칭 검사
                if (y < height - 2 && grid[x, y + 1].tag == tile.tag && grid[x, y + 2].tag == tile.tag)
                {
                    Destroy(grid[x, y]);
                    Destroy(grid[x, y + 1]);
                    Destroy(grid[x, y + 2]);
                    score += 10;
                }
            }
        }
        UpdateScoreUI();
        FillEmptySpaces();
    }

    // 빈 공간을 채우기
    private void FillEmptySpaces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    Vector2 position = new Vector2(x, y);
                    int randomTile = Random.Range(0, tiles.Length);
                    GameObject tile = Instantiate(tiles[randomTile], position, Quaternion.identity);
                    grid[x, y] = tile;
                }
            }
        }
    }

    // 미니게임 종료 로직
    protected override void EndGameLogic()
    {
        Debug.Log("매치-3 미니게임 종료");
        GameManager.isMiniGameActive = false;
    }

    // UI 업데이트 함수들
    private void UpdateUI()
    {
        UpdateScoreUI();
        UpdateTimerUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = $"Score: {score}";
    }

    private void UpdateTimerUI()
    {
        timerText.text = $"Time: {remainingTime:F1} s";
    }
}
