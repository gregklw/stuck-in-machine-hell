using UnityEngine;

[CreateAssetMenu(fileName = "NewExplosionVisuals", menuName = "ScriptableObjects/ExplosionVisuals")]
public class ExplosionVisuals : ScriptableObject
{
    public Sprite ExplosionSprite;
    public RuntimeAnimatorController ExplosionAnimationController;
}
