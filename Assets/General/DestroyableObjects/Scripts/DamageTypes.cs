using UnityEngine;

public enum ArmorType
{
    Light,
    Normal,
    Heavy,
    Chromium
}

public enum ProjectileType
{
    Small,
    Medium,
    Large
}

public static class DamageTypesUtil
{
    private const float SmallColliderSize = 0.08f;
    private const float MediumColliderSize = 0.16f;
    private const float LargeColliderSize = 0.32f;

    public static void SetProjectileColliderSize(BoxCollider2D collider, ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.Small:
                collider.size = new Vector2(SmallColliderSize, SmallColliderSize);
                break;
            case ProjectileType.Medium:
                collider.size = new Vector2(MediumColliderSize, MediumColliderSize);
                break;
            case ProjectileType.Large:
                collider.size = new Vector2(LargeColliderSize, MediumColliderSize);
                break;
        }
    }
}
