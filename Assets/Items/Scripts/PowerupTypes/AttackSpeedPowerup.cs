public class AttackSpeedPowerup : WeaponPowerup
{
    //can be replaced with flyweight
    public int AttackSpeedBoost;

    public override void ActivatePowerup(Player player)
    {
        player.BaseAttackSpeed += AttackSpeedBoost;
    }
}
