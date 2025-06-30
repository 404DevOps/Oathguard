using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OathCollection", menuName = "Config/OathCollection")]
public class OathCollection : ScriptableObject
    {
        [SerializeField] private List<OathAura> _oathAuras;

        private static OathCollection _instance;

        public static OathCollection Instance()
        {
            if (_instance == null)
            {
                _instance = Resources.Load("GameConfig/" + typeof(OathCollection).Name) as OathCollection;
            }
            return _instance;
        }

    public List<OathAura> GetAllOaths()
    {
        return _oathAuras;
    }

    public List<OathAura> GetRandomOths(int count)
    {
        if (_oathAuras == null || _oathAuras.Count == 0 || count <= 0)
            return new List<OathAura>();

        int maxCount = Mathf.Min(count, _oathAuras.Count);
        List<OathAura> shuffled = new List<OathAura>(_oathAuras);

        if (maxCount == 0)
        {
            Debug.Log("No Oaths configured");
            return new();
        }

        // fisher-yates shuffle
        for (int i = 0; i < shuffled.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        return shuffled.GetRange(0, maxCount);
    }
}

