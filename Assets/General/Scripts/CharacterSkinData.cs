using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSkinData", menuName = "ScriptableObjects/CharacterSkinData")]
public class CharacterSkinData : ScriptableObject
{
    public Sprite CharacterSprite;
    public RuntimeAnimatorController DeathExplosionAnimation;
}
