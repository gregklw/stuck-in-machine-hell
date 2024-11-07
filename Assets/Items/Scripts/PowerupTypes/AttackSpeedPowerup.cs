using System.Diagnostics;

public class AttackSpeedPowerup : WeaponPowerup
{
    //can be replaced with flyweight
    public int AttackSpeedBoost;

    public override void ActivatePowerup(Player player)
    {
        player.BaseAttackSpeed += AttackSpeedBoost;
        EventBus<WeaponStatModifierEventWrapper>.Raise(
            new WeaponStatModifierEventWrapper()
            {
                AttackSpeedValue = player.BaseAttackSpeed
            }
        );
    }
}
