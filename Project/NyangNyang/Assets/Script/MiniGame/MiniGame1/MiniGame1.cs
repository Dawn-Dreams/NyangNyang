using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame1 : MonoBehaviour
{
    public int gridSizeX = 7;   // 그리드의 가로 크기
    public int gridSizeY = 7;   // 그리드의 세로 크기
    public GameObject tilePrefab; // 타일 프리팹
    public Transform gridParent;  // 타일이 배치될 부모 오브젝트
    public TileType[] possibleTileTypes; // 생성 가능한 타일의 종류들
    public int matchCount = 3;   // 병합을 위해 필요한 같은 타일의 개수

    private Tile[,] grid;       // 그리드에 배치된 타일 배열
    private List<Tile> tilesList = new List<Tile>(); // 순서대로 타일을 관리하는 배열
    private bool isProcessing;  // 병합 처리 중 여부
    private Tile selectedTile;  // 현재 선택된 타일
    public float slideDuration = 0.3f;

    private void Start()
    {
        StartGameLogic();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ResetGrid();
        }
    }

    private void ResetGrid()
    {
        foreach (var tile in tilesList)
        {
            Destroy(tile.gameObject);
        }

        tilesList.Clear();
        InitializeGrid();
    }

    private void StartGameLogic()
    {
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
        {
            Debug.LogError($"Attempted to create a tile at out-of-bounds position: ({x}, {y})");
            return;
        }

        if (tilePrefab == null || gridParent == null)
        {
            Debug.LogError("Tile prefab or Grid parent is not assigned.");
            return;
        }

        GameObject tileObj = Instantiate(tilePrefab, gridParent);
        Tile tile = tileObj.GetComponent<Tile>();

        if (tile == null)
        {
            Debug.LogError("The instantiated object does not have a Tile component.");
            return;
        }

        TileType randomType = possibleTileTypes[Random.Range(0, possibleTileTypes.Length)];
        tile.Initialize(x, y, randomType);

        grid[x, y] = tile;
        tilesList.Add(tile);

        SetTilePosition(tile, x, y);
        tile.OnTileDragged += (direction, startX, startY) => OnTileDragged(startX, startY, direction); // 드래그 이벤트 연결
    }

    public void OnTileTouched(int x, int y)
    {
        if (isProcessing)
            return;

        selectedTile = grid[x, y];
        // 추가적으로 타일 터치 시 처리할 로직 작성
        Debug.Log($"Tile ({x}, {y}) touched.");
    }

    private void OnTileDragged(int startX, int startY, Direction direction)
    {
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
        if (targetX < 0 || targetX >= gridSizeX || targetY < 0 || targetY >= gridSizeY)
            return;

        // 인접한 타일과 교환 시도
        if (Mathf.Abs(startX - targetX) + Mathf.Abs(startY - targetY) == 1)
        {
            StartCoroutine(SwapTilesCoroutine(startX, startY, targetX, targetY));
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

        Debug.Log($"Swapped tiles at ({x1}, {y1}) and ({x2}, {y2}).");

        isProcessing = false;
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

    // 모든 타일의 인덱스를 다시 설정하는 메서드
    private void AssignTileIndices()
    {
        for (int i = 0; i < tilesList.Count; i++)
        {
            var tile = tilesList[i];
            tile.SetPosition(i % gridSizeX, i / gridSizeX); // 순서대로 재설정
            SetTilePosition(tile, tile.x, tile.y); // 위치 재배치
        }
    }

    private void SwapTilesInList(int indexA, int indexB)
    {
        Tile temp = tilesList[indexA];
        tilesList[indexA] = tilesList[indexB];
        tilesList[indexB] = temp;
    }


    private void ShuffleTiles()
    {
        // 타일 목록 랜덤
        for (int i = 0; i < tilesList.Count; i++)
        {
            int randomIndex = Random.Range(i, tilesList.Count);
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


    private void EndGameLogic()
    {
        Debug.Log("Game Over");
    }
}
