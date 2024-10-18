using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraEdgeInstantiationDatum
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _amountToCreate;
    [SerializeField][Range(0, 20)] private float _edgeDistanceMinRange;
    [SerializeField][Range(0, 20)] private float _edgeDistanceMaxRange;
    public GameObject Prefab => _prefab;
    public int AmountToCreate => _amountToCreate;
    public float EdgeDistanceMinRange => _edgeDistanceMinRange;
    public float EdgeDistanceMaxRange => _edgeDistanceMaxRange;
}
