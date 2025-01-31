using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerParticleWeapon _startingWeaponPrefab;
    [SerializeField] private Transform _weaponsCollectionRoot;
    //consider replacing with HashSet
    private List<PlayerParticleWeapon> _weaponCollection;
    private BusEventBinding<WeaponPickupEventWrapper> _weaponPickupEventBinding;
    private BusEventBinding<WeaponStatModifierEventWrapper> _weaponStatModifierEventBinding;
    private PlayerParticleWeapon _currentWeapon;
    private Player _player;
    private int _weaponSelectionCounter;
    private Button _changeWeaponButton;


    private void Awake()
    {
        _weaponPickupEventBinding = new BusEventBinding<WeaponPickupEventWrapper>(AddNewPickedUpWeapon);
        _weaponStatModifierEventBinding = new BusEventBinding<WeaponStatModifierEventWrapper>(UpdateWeaponAttackSpeedValues);
        _player ??= FindObjectOfType<Player>();
        _weaponCollection = _weaponsCollectionRoot.GetComponentsInChildren<PlayerParticleWeapon>().ToList();
        _changeWeaponButton = GetComponent<Button>();
    }


    private void OnEnable()
    {
        _changeWeaponButton.onClick.AddListener(ChangeWeapon);
        EventBus<WeaponPickupEventWrapper>.Register(_weaponPickupEventBinding);
        EventBus<WeaponStatModifierEventWrapper>.Register(_weaponStatModifierEventBinding);
    }

    private void OnDisable()
    {
        _changeWeaponButton.onClick.RemoveListener(ChangeWeapon);
        EventBus<WeaponPickupEventWrapper>.Deregister(_weaponPickupEventBinding);
        EventBus<WeaponStatModifierEventWrapper>.Deregister(_weaponStatModifierEventBinding);
    }

    private void Start()
    {
        EventBus<WeaponStatModifierEventWrapper>.Raise(
            new WeaponStatModifierEventWrapper()
            {
                AttackSpeedValue = _player.BaseAttackSpeed
            }
        );

        UnlockWeapon(_startingWeaponPrefab);
    }

    private void UnlockWeapon(PlayerParticleWeapon weaponToBeUnlocked)
    {
        foreach (var weapon in _weaponCollection)
        {
            //Debug.Log(weapon + "/" + weaponToBeUnlocked);
            if (weapon.name.Equals(weaponToBeUnlocked.name))
            {
                if (!weapon.IsUnlocked)
                {
                    weapon.IsUnlocked = true;
                    _weaponSelectionCounter = _weaponCollection.Count - 1;
                    SetPlayerWeapon(weapon);
                    break;
                }
            }
        }
    }

    private void AddNewPickedUpWeapon(WeaponPickupEventWrapper playerWeaponChangeEvent)
    {
        PlayerParticleWeapon weaponPrefab = playerWeaponChangeEvent.WeaponPrefab;
        UnlockWeapon(weaponPrefab);
    }

    private void UpdateWeaponAttackSpeedValues(WeaponStatModifierEventWrapper weaponStatModifierEvent)
    {
        _weaponCollection.ForEach(weapon => weapon.SetAttackSpeed(weaponStatModifierEvent.AttackSpeedValue));
    }

    private void ChangeWeapon()
    {
        if (++_weaponSelectionCounter >= _weaponCollection.Count) _weaponSelectionCounter = 0;

        if (!_weaponCollection[_weaponSelectionCounter].IsUnlocked) ChangeWeapon();

        SetPlayerWeapon(_weaponCollection[_weaponSelectionCounter]);
    }

    private void SetPlayerWeapon(PlayerParticleWeapon weapon)
    {
        _currentWeapon?.StopGun();
        _currentWeapon = weapon;
        weapon.StartGun();
        _player.SetCurrentWeapon(weapon);
    }
}
