using System.Collections;
using UnityEngine;

public interface IParticleSystemHelper
{
    IEnumerator DetachAndDestroyWhenParticlesDead();
}
