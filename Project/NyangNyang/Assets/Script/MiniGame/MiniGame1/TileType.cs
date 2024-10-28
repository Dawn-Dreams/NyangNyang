using UnityEngine;

[CreateAssetMenu(fileName = "NewTileType", menuName = "MiniGame/TileType", order = 1)]
public class TileType : ScriptableObject
{
    public string typeName;    // 타일 타입의 이름
    public Sprite tileSprite;  // 타일의 스프라이트 (모양)
    public Color tileColor;    // 타일의 색상 (선택적)
}
