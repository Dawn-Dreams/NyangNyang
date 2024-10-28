using UnityEngine;

[CreateAssetMenu(fileName = "NewTileType", menuName = "MiniGame/TileType", order = 1)]
public class TileType : ScriptableObject
{
    public string typeName;    // Ÿ�� Ÿ���� �̸�
    public Sprite tileSprite;  // Ÿ���� ��������Ʈ (���)
    public Color tileColor;    // Ÿ���� ���� (������)
}
