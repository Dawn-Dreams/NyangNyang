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
        tile.OnTileTouched += () => OnTileTouched(x, y);
    }

    public void OnTileTouched(int x, int y)
    {
        if (isProcessing)
            return;

        List<Tile> matchingTiles = FindMatchingTiles(x, y);

        if (matchingTiles.Count >= matchCount)
        {
            StartCoroutine(MergeTiles(matchingTiles));
        }
    }

    private List<Tile> FindMatchingTiles(int startX, int startY)
    {
        List<Tile> matchingTiles = new List<Tile>();
        Tile startTile = grid[startX, startY];
        TileType startType = startTile.tileType;

        Queue<Tile> toCheck = new Queue<Tile>();
        HashSet<Tile> checkedTiles = new HashSet<Tile>();
        toCheck.Enqueue(startTile);
        checkedTiles.Add(startTile);

        while (toCheck.Count > 0)
        {
            Tile currentTile = toCheck.Dequeue();
            matchingTiles.Add(currentTile);

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

        // 좌측 하단에서 시작하므로 그리드의 경계 조건을 조정
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            if (x > 0) yield return grid[x - 1, y];      // 왼쪽 타일
            if (x < gridSizeX - 1) yield return grid[x + 1, y]; // 오른쪽 타일
            if (y > 0) yield return grid[x, y - 1];      // 아래쪽 타일
            if (y < gridSizeY - 1) yield return grid[x, y + 1]; // 위쪽 타일
        }
    }



    private IEnumerator MergeTiles(List<Tile> matchingTiles)
    {
        isProcessing = true;

        foreach (Tile tile in matchingTiles)
        {
            tile.SetMerged();
            yield return new WaitForSeconds(0.1f);
            tilesList.Remove(tile); // 리스트에서 타일 제거
            Destroy(tile.gameObject);
            grid[tile.x, tile.y] = null;
            Debug.Log("Delete : " + tile.x + ", " + tile.y);
        }

        yield return new WaitForSeconds(0.5f);

        DropTiles();

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y] == null)
                {
                    CreateTile(x, y);
                }
                else
                {
                    grid[x, y].SetPosition(x, y);
                    SetTilePosition(grid[x, y], x, y);
                }
            }
        }

        AssignTileIndices(); // 타일 병합 후 인덱스 재할당
        isProcessing = false;
    }

    private void DropTiles()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            int emptySlotY = -1;

            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y] == null) // 빈 칸 발견
                {
                    if (emptySlotY == -1) // 첫 빈 칸의 y 위치를 저장
                        emptySlotY = y;
                }
                else if (emptySlotY != -1) // 빈 칸이 있고 타일을 이동할 수 있는 경우
                {
                    // 타일을 빈 칸 위치로 이동
                    grid[x, emptySlotY] = grid[x, y];
                    grid[x, emptySlotY].SetPosition(x, emptySlotY);
                    SetTilePosition(grid[x, emptySlotY], x, emptySlotY);

                    grid[x, y] = null; // 원래 위치는 빈 칸으로 설정
                    emptySlotY++; // 다음 빈 칸으로 이동
                }
            }

            // 최상단에 빈 칸이 있는 경우에만 새로운 타일 생성
            if (emptySlotY != -1)
            {
                for (int y = emptySlotY; y < gridSizeY; y++)
                {
                    // 빈 슬롯만 채우도록 조건 추가
                    if (grid[x, y] == null)
                    {
                        CreateTile(x, y);
                    }
                }
            }
        }
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


    private Vector3 GetTilePosition(int x, int y)
    {
        // 타일의 실제 크기를 RectTransform에서 가져옴
        RectTransform tileRect = tilePrefab.GetComponent<RectTransform>();
        float tileWidth = tileRect.rect.width;
        float tileHeight = tileRect.rect.height;

        // 타일 간격을 타일의 너비와 높이로 설정
        return new Vector3(x * tileWidth, -y * tileHeight, 0);
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

    private void EndGameLogic()
    {
        Debug.Log("Game Over");
    }
}
