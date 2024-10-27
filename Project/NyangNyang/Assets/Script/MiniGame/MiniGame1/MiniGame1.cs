using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame1 : MonoBehaviour
{
    public int gridSizeX = 7;   // �׸����� ���� ũ��
    public int gridSizeY = 7;   // �׸����� ���� ũ��
    public GameObject tilePrefab; // Ÿ�� ������
    public Transform gridParent;  // Ÿ���� ��ġ�� �θ� ������Ʈ
    public TileType[] possibleTileTypes; // ���� ������ Ÿ���� ������
    public int matchCount = 3;   // ������ ���� �ʿ��� ���� Ÿ���� ����

    private Tile[,] grid;       // �׸��忡 ��ġ�� Ÿ�� �迭
    private List<Tile> tilesList = new List<Tile>(); // ������� Ÿ���� �����ϴ� �迭
    private bool isProcessing;  // ���� ó�� �� ����

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
        AssignTileIndices(); // Ÿ�� �ε��� �ʱ� ����
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

    // �־��� Ÿ���� �̿� Ÿ���� ��ȯ�ϴ� �޼���
    private IEnumerable<Tile> GetNeighbors(Tile tile)
    {
        int x = tile.x;
        int y = tile.y;

        // ���� �ϴܿ��� �����ϹǷ� �׸����� ��� ������ ����
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            if (x > 0) yield return grid[x - 1, y];      // ���� Ÿ��
            if (x < gridSizeX - 1) yield return grid[x + 1, y]; // ������ Ÿ��
            if (y > 0) yield return grid[x, y - 1];      // �Ʒ��� Ÿ��
            if (y < gridSizeY - 1) yield return grid[x, y + 1]; // ���� Ÿ��
        }
    }



    private IEnumerator MergeTiles(List<Tile> matchingTiles)
    {
        isProcessing = true;

        foreach (Tile tile in matchingTiles)
        {
            tile.SetMerged();
            yield return new WaitForSeconds(0.1f);
            tilesList.Remove(tile); // ����Ʈ���� Ÿ�� ����
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

        AssignTileIndices(); // Ÿ�� ���� �� �ε��� ���Ҵ�
        isProcessing = false;
    }

    private void DropTiles()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            int emptySlotY = -1;

            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y] == null) // �� ĭ �߰�
                {
                    if (emptySlotY == -1) // ù �� ĭ�� y ��ġ�� ����
                        emptySlotY = y;
                }
                else if (emptySlotY != -1) // �� ĭ�� �ְ� Ÿ���� �̵��� �� �ִ� ���
                {
                    // Ÿ���� �� ĭ ��ġ�� �̵�
                    grid[x, emptySlotY] = grid[x, y];
                    grid[x, emptySlotY].SetPosition(x, emptySlotY);
                    SetTilePosition(grid[x, emptySlotY], x, emptySlotY);

                    grid[x, y] = null; // ���� ��ġ�� �� ĭ���� ����
                    emptySlotY++; // ���� �� ĭ���� �̵�
                }
            }

            // �ֻ�ܿ� �� ĭ�� �ִ� ��쿡�� ���ο� Ÿ�� ����
            if (emptySlotY != -1)
            {
                for (int y = emptySlotY; y < gridSizeY; y++)
                {
                    // �� ���Ը� ä�쵵�� ���� �߰�
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
        // Ÿ���� ���� ũ�⸦ RectTransform���� ������
        RectTransform tileRect = tilePrefab.GetComponent<RectTransform>();
        float tileWidth = tileRect.rect.width;
        float tileHeight = tileRect.rect.height;

        // Ÿ�� ������ Ÿ���� �ʺ�� ���̷� ����
        return new Vector3(x * tileWidth, -y * tileHeight, 0);
    }


    // ��� Ÿ���� �ε����� �ٽ� �����ϴ� �޼���
    private void AssignTileIndices()
    {
        for (int i = 0; i < tilesList.Count; i++)
        {
            var tile = tilesList[i];
            tile.SetPosition(i % gridSizeX, i / gridSizeX); // ������� �缳��
            SetTilePosition(tile, tile.x, tile.y); // ��ġ ���ġ
        }
    }

    private void EndGameLogic()
    {
        Debug.Log("Game Over");
    }
}
