using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : Enemy
{
    [SerializeField][Range(0.2f, 1.0f)] private float _minimumDistance;
    private const float MinimumDistance = 0.2f;
    [SerializeField][Range(0, 5)] private float _baseMoveSpeed;
    public float BaseMoveSpeed
    {
        get => _baseMoveSpeed;
        set => _baseMoveSpeed = value;
    }
    private float _counter;
    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (!TargetPlayer) return;
        transform.up = GetDirectionVectorTowardsPlayer();
        //ThisRigidbody.MovePosition(BaseMoveSpeed * 0.001f * Time.fixedDeltaTime * transform.up);
        //transform.position += transform.up * BaseMoveSpeed * Time.fixedDeltaTime;
        if (Mathf.Abs(transform.position.x - TargetPlayer.transform.position.x) >= _minimumDistance)
            ThisRigidbody.velocity = BaseMoveSpeed * 40 * Time.fixedDeltaTime * (Vector2) transform.up;
        else
        {
            ThisRigidbody.velocity = Vector2.zero;
        }
    }
}
