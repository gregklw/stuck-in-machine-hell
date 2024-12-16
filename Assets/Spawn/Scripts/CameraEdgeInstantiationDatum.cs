using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class CameraEdgeInstantiationDatum
{
    [SerializeField] private AssetReferenceGameObject _assetReferencePrefab;
    [SerializeField] private int _amountToCreate;
    [SerializeField][Range(0, 20)] private float _edgeDistanceMinRange;
    [SerializeField][Range(0, 20)] private float _edgeDistanceMaxRange;

    public GameObject LoadedGameObject { get; set; }
    public AssetReferenceGameObject AssetReferencePrefab => _assetReferencePrefab;
    public int AmountToCreate => _amountToCreate;
    public float EdgeDistanceMinRange => _edgeDistanceMinRange;
    public float EdgeDistanceMaxRange => _edgeDistanceMaxRange;
}
