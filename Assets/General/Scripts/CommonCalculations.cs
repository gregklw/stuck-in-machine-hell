using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class CommonCalculations
{
    public static float CalculateAttackSpeed(float attackSpeed)
    {
        return Mathf.Clamp(120.0f / attackSpeed, 0.0001f, Mathf.Infinity);
    }

    public static float CalculateArmorAmplify(float inputDamage, ArmorType armorType, ProjectileType projectileType)
    {
        if (armorType == ArmorType.Light && projectileType == ProjectileType.Large
            || armorType == ArmorType.Normal && projectileType == ProjectileType.Small
            || armorType == ArmorType.Heavy && projectileType == ProjectileType.Medium
            || armorType == ArmorType.Chromium)
        {
            return inputDamage / 5;
        }
        else if (armorType == ArmorType.Light && projectileType == ProjectileType.Small
            || armorType == ArmorType.Normal && projectileType == ProjectileType.Medium
            || armorType == ArmorType.Heavy && projectileType == ProjectileType.Large)
        {
            return inputDamage / 2;
        }

        return inputDamage;
    }

    public static Vector3 RotateTowardsUp(Vector3 start, float angle)
    {
        Vector3 axis = Vector3.Cross(start, Vector3.right);
        if (axis == Vector3.zero) axis = Vector3.forward;
        return Quaternion.AngleAxis(angle, axis) * start;
    }
}
