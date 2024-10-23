using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Weapon _startingWeaponPrefab;
    //consider replacing with HashSet
    private List<Weapon> _weaponCollection;

    private BusEventBinding<WeaponPickupEventWrapper> _weaponPickupEventBinding;
    private Weapon _currentWeapon;
    private Player _player;
    private int _weaponSelectionCounter;
    private Button _changeWeaponButton;

    private void Awake()
    {
        _weaponPickupEventBinding = new BusEventBinding<WeaponPickupEventWrapper>(AddNewPickedUpWeapon);
        _player ??= FindObjectOfType<Player>();
        Weapon startingWeapon = Instantiate(_startingWeaponPrefab);
        _weaponCollection = new List<Weapon> { startingWeapon };
        SetPlayerWeapon(startingWeapon);
        _changeWeaponButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _changeWeaponButton.onClick.AddListener(ChangeWeapon);
        EventBus<WeaponPickupEventWrapper>.Register(_weaponPickupEventBinding);
    }

    private void OnDisable()
    {
        _changeWeaponButton.onClick.RemoveListener(ChangeWeapon);
        EventBus<WeaponPickupEventWrapper>.Deregister(_weaponPickupEventBinding);
    }

    private void AddNewPickedUpWeapon(WeaponPickupEventWrapper playerWeaponChangeEvent)
    {
        Weapon weaponPrefab = playerWeaponChangeEvent.WeaponPrefab;
        foreach (var collectedWeapon in _weaponCollection)
        {
            //Debug.Log(collectedWeapon.GetType());
            if (collectedWeapon.GetType() == weaponPrefab.GetType())
            {
                //consider destroying weapon argument or prevent instantiation
                return;
            }
        }
        Weapon weapon = Instantiate(weaponPrefab, transform);
        _weaponCollection.Add(weapon);
        _weaponSelectionCounter = _weaponCollection.Count - 1;
        SetPlayerWeapon(weapon);
    }

    private void ChangeWeapon()
    {
        if (++_weaponSelectionCounter >= _weaponCollection.Count) _weaponSelectionCounter = 0;
        SetPlayerWeapon(_weaponCollection[_weaponSelectionCounter]);
    }

    private void SetPlayerWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
        _player.CurrentWeapon = _currentWeapon;
    }

}
