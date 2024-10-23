using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame1 : MiniGameBase
{
    public int gridSizeX = 7;   // 그리드의 가로 크기
    public int gridSizeY = 7;   // 그리드의 세로 크기
    public GameObject tilePrefab; // 타일 프리팹
    public Transform gridParent;  // 타일이 배치될 부모 오브젝트
    public TileType[] possibleTileTypes; // 생성 가능한 타일의 종류들
    public int matchCount = 3;   // 병합을 위해 필요한 같은 타일의 개수

    private Tile[,] grid;       // 그리드에 배치된 타일 배열
    private bool isProcessing;  // 병합 처리 중 여부

    // 게임 시작 시 로직
    protected override void StartGameLogic()
    {
        InitializeGrid();  // 그리드 초기화
    }

    // 그리드 초기화 메서드
    private void InitializeGrid()
    {
        grid = new Tile[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                CreateTile(x, y);  // 각 좌표에 타일 생성
            }
        }
    }

    // 타일 생성 메서드
    private void CreateTile(int x, int y)
    {
        if (tilePrefab == null)
        {
            Debug.LogError("Tile prefab is not assigned in the Inspector.");
            return;
        }

        if (gridParent == null)
        {
            Debug.LogError("Grid parent is not assigned in the Inspector.");
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
        tile.Initialize(x, y, randomType);  // 타일을 초기화하여 좌표와 타입을 부여
        grid[x, y] = tile;

        tile.OnTileTouched += () => OnTileTouched(x, y);
    }



    // 타일 터치 시 호출되는 메서드
    public void OnTileTouched(int x, int y)
    {
        if (isProcessing)
            return;

        Debug.Log($"Tile at {x},{y} touched");
        List<Tile> matchingTiles = FindMatchingTiles(x, y);

        if (matchingTiles.Count >= matchCount)
        {
            StartCoroutine(MergeTiles(matchingTiles));
        }
    }

    // 같은 종류의 타일을 찾는 메서드
    private List<Tile> FindMatchingTiles(int startX, int startY)
    {
        List<Tile> matchingTiles = new List<Tile>();
        Tile startTile = grid[startX, startY];
        TileType startType = startTile.tileType;

        // BFS로 같은 종류의 타일 찾기
        Queue<Tile> toCheck = new Queue<Tile>();
        HashSet<Tile> checkedTiles = new HashSet<Tile>();
        toCheck.Enqueue(startTile);
        checkedTiles.Add(startTile);

        while (toCheck.Count > 0)
        {
            Tile currentTile = toCheck.Dequeue();
            matchingTiles.Add(currentTile);

            // 주변 타일 검사
            foreach (Tile neighbor in GetNeighbors(currentTile))
            {
                if (!checkedTiles.Contains(neighbor) && neighbor.tileType == startType)
                {
                    toCheck.Enqueue(neighbor);
                    checkedTiles.Add(neighbor);
                }
            }
        }

        return matchingTiles;
    }

    // 주어진 타일의 이웃 타일을 반환하는 메서드
    private IEnumerable<Tile> GetNeighbors(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        if (x > 0) yield return grid[x - 1, y];      // 왼쪽 타일
        if (x < gridSizeX - 1) yield return grid[x + 1, y]; // 오른쪽 타일
        if (y > 0) yield return grid[x, y - 1];      // 아래쪽 타일
        if (y < gridSizeY - 1) yield return grid[x, y + 1]; // 위쪽 타일
    }

    // 타일 병합 처리 메서드 (비동기 처리)
    private IEnumerator MergeTiles(List<Tile> matchingTiles)
    {
        isProcessing = true;

        // 병합된 타일 제거 및 처리
        foreach (Tile tile in matchingTiles)
        {
            tile.SetMerged();  // 타일을 병합 상태로 표시
            yield return new WaitForSeconds(0.1f);
            Destroy(tile.gameObject);
        }

        yield return new WaitForSeconds(0.5f);

        // 빈 칸에 새 타일 생성
        foreach (Tile tile in matchingTiles)
        {
            CreateTile(tile.x, tile.y);
        }

        isProcessing = false;
    }

    protected override void EndGameLogic()
    {
        // 게임 종료 시 처리할 로직
        Debug.Log("MiniGame Ended");
    }
}
