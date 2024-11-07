using UnityEngine;
/// <summary>
/// Used as a flyweight for projectile info.
/// </summary>
[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ObjectSkinData : ScriptableObject
{
    public Sprite[] ProjectileSprites;
    public Sprite ExplosionSprite;
    public RuntimeAnimatorController ExplosionAnimationController;
    [Range(0.0f, 10.0f)] public float ProjectileSpeed;
    [Range(0.0f, 10.0f)] public float ProjectileLifeTime;
    public string[] CollisionTagsForHit;
    public ProjectileType ProjectileType;
}
