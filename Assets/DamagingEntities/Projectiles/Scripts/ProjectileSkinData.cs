using UnityEngine;
using UnityEngine.AddressableAssets;
/// <summary>
/// Used as a flyweight for projectile info.
/// </summary>
[CreateAssetMenu(fileName = "ProjectileSkinData", menuName = "ScriptableObjects/ProjectileSkinData")]
public class ProjectileSkinData : ScriptableObject
{
    public Sprite[] ProjectileSprites;
    public RuntimeAnimatorController ExplosionAnimationController;

    [Header("Death Explosion Variables")]
    public Material DeathExplosionMaterial;
    public int SpriteSheetRows;
    public int SpriteSheetColumns;

    [Header("Projectile Stats")]
    [Range(0.0f, 10.0f)] public float ProjectileSpeed;
    [Range(0.0f, 10.0f)] public float ProjectileLifeTime;
    public string[] CollisionTagsForHit;
    public ProjectileType ProjectileType;
}
