namespace APIGameServer.Models;


public class PlayerStatusData
{
    public int uid { get; set; }
    public int hp { get; set; }
    public int mp { get; set; }
    public int attack_power { get; set; }
    public int def { get; set; }
    public int heal_hp_persec { get; set; }
    public int heal_mp_persec { get; set; }
    public float crit_percent {  get; set; }
    public float attack_speed { get; set; }

}
public class PlayerStatusLevelData
{
    public int uid { get; set; }
    public int hp_lv { get; set; }
    public int mp_vl { get; set; }
    public int str_vl { get; set; }
    public int heal_hp_lv { get; set; }
    public int heal_mp_lv { get; set; }
    public int crit_lv { get; set; }
    public int attack_speed_lv { get; set; }
    public int gold_acq_lv { get; set; }
    public int exp_acq_lv { get; set; }

}
public class PlayerGoodsData
{
    public int uid { get; set; }
    public int gold { get; set; }
    public int jewerly { get; set; }
    public int ticket { get; set; }


}



