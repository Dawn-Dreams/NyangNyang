using UnityEngine;
using TMPro;

public class MiniGame1 : MiniGameBase
{
    public int width = 8;               // ������ �ʺ�
    public int height = 8;              // ������ ����
    public GameObject[] tiles;          // ����� Ÿ�ϵ�
    public TextMeshProUGUI scoreText;   // ���� UI
    public TextMeshProUGUI timerText;   // Ÿ�̸� UI

    private GameObject[,] grid;         // ���� ����
    private int score = 0;              // ����
    private float timeLimit = 60f;      // ���� �ð�
    private float remainingTime;        // ���� �ð�

    void Start()
    {
        Initialize("��ġ-3 �̴ϰ���", 1); // �̴ϰ��� �̸��� ���� Ƽ�� �ε���
        grid = new GameObject[width, height];
        GenerateBoard();
        remainingTime = timeLimit;
        UpdateUI();
    }

    // �̴ϰ��� ���� ����
    protected override void StartGameLogic()
    {
        Debug.Log("��ġ-3 �̴ϰ��� ����");
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

    // ���� ���� Ÿ���� �������� ��ġ
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

    // ��ġ-3 �˻� ����
    private void CheckForMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = grid[x, y];

                // ���� ��Ī �˻�
                if (x < width - 2 && grid[x + 1, y].tag == tile.tag && grid[x + 2, y].tag == tile.tag)
                {
                    Destroy(grid[x, y]);
                    Destroy(grid[x + 1, y]);
                    Destroy(grid[x + 2, y]);
                    score += 10;
                }

                // ���� ��Ī �˻�
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

    // �� ������ ä���
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

    // �̴ϰ��� ���� ����
    protected override void EndGameLogic()
    {
        Debug.Log("��ġ-3 �̴ϰ��� ����");
        GameManager.isMiniGameActive = false;
    }

    // UI ������Ʈ �Լ���
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
