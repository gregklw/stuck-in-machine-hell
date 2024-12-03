using UnityEngine;
/// <summary>
/// Used as a flyweight for projectile info.
/// </summary>
[CreateAssetMenu(fileName = "ProjectileSkinData", menuName = "ScriptableObjects/ProjectileSkinData")]
public class ProjectileSkinData : ScriptableObject
{
    public Sprite[] ProjectileSprites;
    public RuntimeAnimatorController ExplosionAnimationController;
    [Range(0.0f, 10.0f)] public float ProjectileSpeed;
    [Range(0.0f, 10.0f)] public float ProjectileLifeTime;
    public string[] CollisionTagsForHit;
    public ProjectileType ProjectileType;
}
