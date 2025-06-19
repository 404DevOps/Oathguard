using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponCollection", menuName = "Weapon/WeaponCollection")]
public class WeaponCollection : ScriptableObject
    {
        [SerializeField] private List<WeaponSet> _weaponSets;

        private static WeaponCollection _instance;

        public static WeaponCollection Instance()
        {
            if (_instance == null)
            {
                _instance = Resources.Load("GameConfig/" + typeof(WeaponCollection).Name) as WeaponCollection;
            }
            return _instance;
        }

    public List<WeaponSet> GetAllWeapons()
    {
        return _weaponSets;
    }
}

