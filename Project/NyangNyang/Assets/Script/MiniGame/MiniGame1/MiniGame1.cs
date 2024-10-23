using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame1 : MiniGameBase
{
    public int gridSizeX = 7;   // �׸����� ���� ũ��
    public int gridSizeY = 7;   // �׸����� ���� ũ��
    public GameObject tilePrefab; // Ÿ�� ������
    public Transform gridParent;  // Ÿ���� ��ġ�� �θ� ������Ʈ
    public TileType[] possibleTileTypes; // ���� ������ Ÿ���� ������
    public int matchCount = 3;   // ������ ���� �ʿ��� ���� Ÿ���� ����

    private Tile[,] grid;       // �׸��忡 ��ġ�� Ÿ�� �迭
    private bool isProcessing;  // ���� ó�� �� ����

    // ���� ���� �� ����
    protected override void StartGameLogic()
    {
        InitializeGrid();  // �׸��� �ʱ�ȭ
    }

    // �׸��� �ʱ�ȭ �޼���
    private void InitializeGrid()
    {
        grid = new Tile[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                CreateTile(x, y);  // �� ��ǥ�� Ÿ�� ����
            }
        }
    }

    // Ÿ�� ���� �޼���
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
        tile.Initialize(x, y, randomType);  // Ÿ���� �ʱ�ȭ�Ͽ� ��ǥ�� Ÿ���� �ο�
        grid[x, y] = tile;

        tile.OnTileTouched += () => OnTileTouched(x, y);
    }



    // Ÿ�� ��ġ �� ȣ��Ǵ� �޼���
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

    // ���� ������ Ÿ���� ã�� �޼���
    private List<Tile> FindMatchingTiles(int startX, int startY)
    {
        List<Tile> matchingTiles = new List<Tile>();
        Tile startTile = grid[startX, startY];
        TileType startType = startTile.tileType;

        // BFS�� ���� ������ Ÿ�� ã��
        Queue<Tile> toCheck = new Queue<Tile>();
        HashSet<Tile> checkedTiles = new HashSet<Tile>();
        toCheck.Enqueue(startTile);
        checkedTiles.Add(startTile);

        while (toCheck.Count > 0)
        {
            Tile currentTile = toCheck.Dequeue();
            matchingTiles.Add(currentTile);

            // �ֺ� Ÿ�� �˻�
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

        if (x > 0) yield return grid[x - 1, y];      // ���� Ÿ��
        if (x < gridSizeX - 1) yield return grid[x + 1, y]; // ������ Ÿ��
        if (y > 0) yield return grid[x, y - 1];      // �Ʒ��� Ÿ��
        if (y < gridSizeY - 1) yield return grid[x, y + 1]; // ���� Ÿ��
    }

    // Ÿ�� ���� ó�� �޼��� (�񵿱� ó��)
    private IEnumerator MergeTiles(List<Tile> matchingTiles)
    {
        isProcessing = true;

        // ���յ� Ÿ�� ���� �� ó��
        foreach (Tile tile in matchingTiles)
        {
            tile.SetMerged();  // Ÿ���� ���� ���·� ǥ��
            yield return new WaitForSeconds(0.1f);
            Destroy(tile.gameObject);
        }

        yield return new WaitForSeconds(0.5f);

        // �� ĭ�� �� Ÿ�� ����
        foreach (Tile tile in matchingTiles)
        {
            CreateTile(tile.x, tile.y);
        }

        isProcessing = false;
    }

    protected override void EndGameLogic()
    {
        // ���� ���� �� ó���� ����
        Debug.Log("MiniGame Ended");
    }
}
