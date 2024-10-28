namespace APIGameServer.DTO;

public class RankingData
{
    public int rank {  get; set; }  
    public int uid {  get; set; }   
    public string nickname {  get; set; }
    public int score {  get; set; }
}
public class ResponseRanking
{
    public ErrorCode ErrorCode { get; set; }
    public List<RankingData> RankingData { get; set; }
}