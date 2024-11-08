using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame1 : MiniGameBase
{
    public int gridSizeX = 7;   // 그리드의 가로 크기
    public int gridSizeY = 7 + 1;   // 그리드의 세로 크기 + 최상단
    public GameObject tilePrefab; // 타일 프리팹
    public Transform gridParent;  // 타일이 배치될 부모 오브젝트
    public TileType[] possibleTileTypes; // 생성 가능한 타일의 종류들
    public int matchCount = 3;   // 병합을 위해 필요한 같은 타일의 개수

    private Tile[,] grid;       // 그리드에 배치된 타일 배열
    private List<Tile> tilesList = new List<Tile>(); // 순서대로 타일을 관리하는 배열
    private bool isProcessing;  // 병합 처리 중 여부
    private Tile selectedTile;  // 현재 선택된 타일
    public float slideDuration = 0.3f;
    private List<Tile> matchedTiles = new List<Tile>(); // 삭제할 타일들을 저장할 리스트

    private bool isOnGame = true;

    public float gameTime = 60.0f; // 기본 60초 제한시간
    private float tempTimer = 0.0f; // 현재 시간
    public int score = 0;  // 점수 -> 추후 재화와 비례 하도록
    private int scorePerTile = 10; // 타일 하나 당 점수

    public Slider timerSlider;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countdownText;

    protected override void StartGameLogic()
    {
        InitializeGrid();
    }

    protected override void EndGameLogic()
    {
        base.ClearGame();
        isOnGame = false;
        countdownText.gameObject.SetActive(true);
        countdownText.text = "TIME OVER!!";
        GameManager.isMiniGameActive = false;
        Debug.Log("Game 종료");
    }

    private void Start()
    {
        base.Initialize("MiniGame1", 0);
        StartCoroutine(CountdownAndStartGame());
    }

    private IEnumerator CountdownAndStartGame()
    {
        countdownText.gameObject.SetActive(true); // 카운트다운 텍스트 표시

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString(); // 텍스트 업데이트
            yield return new WaitForSeconds(1); // 1초 대기
        }

        countdownText.gameObject.SetActive(false); // 카운트다운 숨기기
        StartGameLogic(); // 게임 시작 로직 호출
        isOnGame = true;
        tempTimer = 0.0f;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
    }

    private void Update()
    {

        if (tempTimer < gameTime)
        {
            tempTimer += Time.deltaTime;
            timerSlider.value = tempTimer;
        }
        else
            EndGameLogic();

        if (Input.GetKeyDown(KeyCode.F5))
        {
            ResetGrid();
        }
    }

    public void ResetGrid()
    {
        foreach (var tile in tilesList)
        {
            if (tile != null)
            {
                Destroy(tile.gameObject);
            }
        }
        tilesList.Clear();
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        grid = new Tile[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                CreateTile(x, y);
            }
        }
        ShuffleTiles();
        AssignTileIndices(); // 타일 인덱스 초기 설정
    }

    private void CreateTile(int x, int y)
    {
        if (x < 0 || x >= gridSizeX || y < 0 || y >= gridSizeY)
            return;

        if (tilePrefab == null || gridParent == null)
            return;


        GameObject tileObj = Instantiate(tilePrefab, gridParent);
        Tile tile = tileObj.GetComponent<Tile>();

        if (tile == null)
            return;

        TileType randomType = possibleTileTypes[UnityEngine.Random.Range(0, possibleTileTypes.Length)];
        tile.Initialize(x, y, randomType);

        grid[x, y] = tile;
        tilesList.Add(tile);

        SetTilePosition(tile, x, y);
        tile.OnTileTouched += OnTileTouched;
        tile.OnTileDragged += (direction, startX, startY) => OnTileDragged(startX, startY, direction);
    }

    private void SetTilePosition(Tile tile, int x, int y)
    {
        RectTransform tileRect = tilePrefab.GetComponent<RectTransform>();
        float tileWidth = tileRect.rect.width;
        float tileHeight = tileRect.rect.height;

        float startX = -(gridSizeX - 1) * tileWidth / 2;
        float startY = -(gridSizeY - 1) * tileHeight / 2;

        Vector2 tilePosition = new Vector2(startX + x * tileWidth, startY + y * tileHeight);
        tile.transform.localPosition = tilePosition;
    }

    private void AssignTileIndices()
    {
        for (int i = 0; i < tilesList.Count; i++)
        {
            var tile = tilesList[i];
            tile.SetPosition(i % gridSizeX, i / gridSizeX); // 순서대로 재설정
            SetTilePosition(tile, tile.x, tile.y); // 위치 재배치
        }
        CheckAndRemoveMatches();
    }
    private void ShuffleTiles()
    {
        // 타일 목록 랜덤
        for (int i = 0; i < tilesList.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, tilesList.Count);
            SwapTilesInList(i, randomIndex);
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Tile tile = tilesList[x + y * gridSizeX];
                grid[x, y] = tile;
                tile.SetPosition(x, y);
                SetTilePosition(tile, x, y);
            }
        }
    }

    public void OnTileTouched(int x, int y)
    {
        if (isProcessing)
            return;

        selectedTile = grid[x, y];
    }

    private void OnTileDragged(int startX, int startY, Direction direction)
    {
        if (isProcessing || selectedTile == null)
            return;
        int targetX = startX;
        int targetY = startY;

        switch (direction)
        {
            case Direction.Up: targetY += 1; break;
            case Direction.Down: targetY -= 1; break;
            case Direction.Left: targetX -= 1; break;
            case Direction.Right: targetX += 1; break;
        }

        // 그리드 범위 확인
        if (targetX < 0 || targetX >= gridSizeX || targetY < 0 || targetY >= gridSizeY - 1)
            return;

        // 인접한 타일과 교환
        if (Mathf.Abs(startX - targetX) + Mathf.Abs(startY - targetY) == 1)
        {
            StartCoroutine(SwapTilesCoroutine(startX, startY, targetX, targetY));
            CheckAndRemoveMatches();
            selectedTile = null;
        }
    }

    private IEnumerator SwapTilesCoroutine(int x1, int y1, int x2, int y2)
    {
        isProcessing = true;

        Tile tile1 = grid[x1, y1];
        Tile tile2 = grid[x2, y2];

        if (tile1 == null || tile2 == null) yield break;

        Vector2 originalPos1 = tile1.transform.localPosition;
        Vector2 originalPos2 = tile2.transform.localPosition;

        // 슬라이드 애니메이션
        float elapsedTime = 0f;
        while (elapsedTime < slideDuration)
        {
            float t = elapsedTime / slideDuration;
            tile1.transform.localPosition = Vector2.Lerp(originalPos1, originalPos2, t);
            tile2.transform.localPosition = Vector2.Lerp(originalPos2, originalPos1, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 위치 교환
        grid[x1, y1] = tile2;
        grid[x2, y2] = tile1;

        tile1.SetPosition(x2, y2);
        tile2.SetPosition(x1, y1);

        SetTilePosition(tile1, x2, y2);
        SetTilePosition(tile2, x1, y1);

        bool isMatched = CheckAndRemoveMatches();

        // 매칭된 타일이 삭제된 경우 원래 위치로 돌아가지 않음
        if (isMatched)
        {

        }
        else
        {
            // 매칭된 타일이 없는 경우 원래 위치로 복귀
            elapsedTime = 0f;
            while (elapsedTime < slideDuration)
            {
                float t = elapsedTime / slideDuration;
                tile1.transform.localPosition = Vector2.Lerp(originalPos2, originalPos1, t);
                tile2.transform.localPosition = Vector2.Lerp(originalPos1, originalPos2, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 위치를 원래대로 복구
            grid[x1, y1] = tile1;
            grid[x2, y2] = tile2;

            tile1.SetPosition(x1, y1);
            tile2.SetPosition(x2, y2);

            SetTilePosition(tile1, x1, y1);
            SetTilePosition(tile2, x2, y2);
        }

        matchedTiles.Clear(); // 매칭 리스트 초기화
        isProcessing = false;
    }

    private void SwapTilesInList(int indexA, int indexB)
    {
        Tile temp = tilesList[indexA];
        tilesList[indexA] = tilesList[indexB];
        tilesList[indexB] = temp;
    }

    private bool CheckAndRemoveMatches()
    {
        matchedTiles.Clear();

        // 가로/세로로 3개 이상 인접한 같은 타입의 타일을 찾음
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY - 1; y++)
            {
                Tile currentTile = grid[x, y];

                if (currentTile == null) // currentTile이 null인지 확인
                    continue;

                // 가로 체크
                List<Tile> horizontalMatch = new List<Tile> { currentTile };
                for (int i = 1; x + i < gridSizeX; i++)
                {
                    Tile nextTile = grid[x + i, y];
                    if (nextTile != null && nextTile.tileType == currentTile.tileType)
                    {
                        horizontalMatch.Add(nextTile);
                    }
                    else break;
                }
                if (horizontalMatch.Count >= matchCount) // 필요한 매칭 개수 확인
                    matchedTiles.AddRange(horizontalMatch);

                // 세로 체크
                List<Tile> verticalMatch = new List<Tile> { currentTile };
                for (int i = 1; y + i < gridSizeY - 1; i++)
                {
                    Tile nextTile = grid[x, y + i];
                    if (nextTile != null && nextTile.tileType == currentTile.tileType)
                    {
                        verticalMatch.Add(nextTile);
                    }
                    else break;
                }
                if (verticalMatch.Count >= matchCount)
                    matchedTiles.AddRange(verticalMatch);
            }
        }

        // 2x2 모양 체크
        for (int x = 0; x < gridSizeX - 1; x++)
        {
            for (int y = 0; y < gridSizeY - 1; y++)
            {
                Tile tile1 = grid[x, y];
                Tile tile2 = grid[x + 1, y];
                Tile tile3 = grid[x, y + 1];
                Tile tile4 = grid[x + 1, y + 1];

                // null 체크 추가
                if (tile1 != null && tile2 != null && tile3 != null && tile4 != null &&
                    tile1.tileType == tile2.tileType &&
                    tile1.tileType == tile3.tileType &&
                    tile1.tileType == tile4.tileType)
                {
                    matchedTiles.Add(tile1);
                    matchedTiles.Add(tile2);
                    matchedTiles.Add(tile3);
                    matchedTiles.Add(tile4);
                }
            }
        }

        // 인접한 같은 타입 타일도 찾음
        List<Tile> additionalMatches = new List<Tile>();
        foreach (Tile tile in matchedTiles)
        {
            FindConnectedTiles(tile, tile.tileType, additionalMatches);
        }
        matchedTiles.AddRange(additionalMatches);

        // 중복 타일 제거
        matchedTiles = matchedTiles.Distinct().ToList();

        // 매칭된 타일이 없으면 false 반환
        if (matchedTiles.Count == 0)
            return false;

        // 삭제 수행
        foreach (Tile tile in matchedTiles)
        {
            if (tile != null) // null 검사
            {
                tile.SetMerged();             // 타일의 삭제 효과 처리 (타일 비활성화 등)
                grid[tile.x, tile.y] = null;  // 그리드에서 타일 제거
            }
        }
        score += matchedTiles.Count * scorePerTile; // 삭제된 타일 개수 당 점수 추가
        scoreText.text = "Score: " + score;

        StartCoroutine(WaitAndDropTiles()); // 애니메이션과 드롭 로직 시작
        matchedTiles.Clear();
        return true;
    }

    // 재귀적으로 인접한 같은 타입의 타일을 찾는 함수
    private void FindConnectedTiles(Tile tile, TileType type, List<Tile> connectedTiles)
    {
        if (tile == null || tile.tileType != type || connectedTiles.Contains(tile)) return;

        connectedTiles.Add(tile);

        int x = tile.x;
        int y = tile.y;

        // 인접한 4방향 검사
        if (x > 0) FindConnectedTiles(grid[x - 1, y], type, connectedTiles); // Left
        if (x < gridSizeX - 1) FindConnectedTiles(grid[x + 1, y], type, connectedTiles); // Right
        if (y > 0) FindConnectedTiles(grid[x, y - 1], type, connectedTiles); // Down
        if (y < gridSizeY - 2) // 최상단 줄을 제외하고 Up 방향 검사
            FindConnectedTiles(grid[x, y + 1], type, connectedTiles); // Up
    }

    private int movingTilesCount = 0; // 애니메이션 중인 타일 개수를 추적

    private IEnumerator WaitAndDropTiles()
    {
        yield return new WaitForSeconds(1f); // 병합 후 1초 대기

        for (int x = 0; x < gridSizeX; x++)
        {
            int emptyCount = 0; // 빈 타일 개수

            // 아래에서 위로 검사하면서 빈 타일을 확인
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y] == null)
                {
                    emptyCount++;
                }
                else if (emptyCount > 0)
                {
                    // 타일이 내려오도록 MoveDownByCells 호출
                    Tile tile = grid[x, y];
                    grid[x, y - emptyCount] = tile;
                    grid[x, y] = null;

                    // 코루틴 호출로 타일 이동 애니메이션 실행 및 이동 중인 타일 개수 증가
                    movingTilesCount++;
                    StartCoroutine(MoveTileWithTracking(tile, emptyCount));
                }
            }
        }

        // 모든 타일이 내려올 때까지 대기
        while (movingTilesCount > 0)
        {
            yield return null;
        }

        yield return StartCoroutine(SpawnNewTiles());
    }

    // 이동 중인 타일 개수를 추적하면서 타일 이동
    private IEnumerator MoveTileWithTracking(Tile tile, int cells)
    {
        yield return tile.MoveDownByCells(cells);
        movingTilesCount--; // 이동이 끝난 후 타일 개수 감소
    }


    // 삭제 후 새 타일을 생성하는 코루틴
    private IEnumerator SpawnNewTiles()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            int emptyCount = 0;
            for (int y = gridSizeY - 1; y >= 0; y--)
            {
                if (grid[x, y] == null)
                {
                    CreateTile(x, y);
                    emptyCount++;
                }
            }
        }

        CheckAndRemoveMatches(); // 매칭 체크

        yield return new WaitForSeconds(0.1f);
    }
}
