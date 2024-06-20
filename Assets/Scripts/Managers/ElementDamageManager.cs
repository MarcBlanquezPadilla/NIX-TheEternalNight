using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDamageManager : MonoBehaviour {

    #region Instance

    private static ElementDamageManager _instance;
    public static ElementDamageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ElementDamageManager>();
                //DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    #endregion

    [Header("Table")]
    [Header("Ligth Row")]
    [SerializeField] private float light_Light = 1.0f;
    [SerializeField] private float light_Shadow = 2.0f;
    [SerializeField] private float light_Poison = 1.0f;
    [SerializeField] private float light_Blood = 0.5f;
    [Header("Shadow Row")]
    [SerializeField] private float shadow_Light = 0.5f;
    [SerializeField] private float shadow_Shadow = 1.0f;
    [SerializeField] private float shadow_Poison = 2.0f;
    [SerializeField] private float shadow_Blood = 1.0f;
    [Header("Poison Row")]
    [SerializeField] private float poison_Light = 1.0f;
    [SerializeField] private float poison_Shadow = 0.5f;
    [SerializeField] private float poison_Poison = 0f;
    [SerializeField] private float poison_Blood = 2.0f;
    [Header("Blood Row")]
    [SerializeField] private float blood_Light = 2.0f;
    [SerializeField] private float blood_Shadow = 1.0f;
    [SerializeField] private float blood_Poison = 0.5f;
    [SerializeField] private float blood_Blood = 1.5f;


    public Dictionary<string, Dictionary<string, float>> ElementTable = new Dictionary<string, Dictionary<string, float>>();
    
    private void Start() {

        Dictionary<string, float> DefaultRow = new Dictionary<string, float>();

        DefaultRow.Add("Default", 1);
        DefaultRow.Add("Light", 1);
        DefaultRow.Add("Shadow", 1);
        DefaultRow.Add("Poison", 1);
        DefaultRow.Add("Blood", 1);

        ElementTable.Add("Default", DefaultRow);

        Dictionary<string, float> LigthRow = new Dictionary<string, float>();

        LigthRow.Add("Default", 1);
        LigthRow.Add("Light", light_Light);
        LigthRow.Add("Shadow", light_Shadow);
        LigthRow.Add("Poison", light_Poison);
        LigthRow.Add("Blood", light_Blood);

        ElementTable.Add("Light", LigthRow);

        Dictionary<string, float> ShadowRow = new Dictionary<string, float>();

        ShadowRow.Add("Default", 1);
        ShadowRow.Add("Light", shadow_Light);
        ShadowRow.Add("Shadow", shadow_Shadow);
        ShadowRow.Add("Poison", shadow_Poison);
        ShadowRow.Add("Blood", shadow_Blood);

        ElementTable.Add("Shadow", ShadowRow);

        Dictionary<string, float> PoisonRow = new Dictionary<string, float>();

        PoisonRow.Add("Default", 1);
        PoisonRow.Add("Light", poison_Light);
        PoisonRow.Add("Shadow", poison_Shadow);
        PoisonRow.Add("Poison", poison_Poison);
        PoisonRow.Add("Blood", poison_Blood);

        ElementTable.Add("Poison", PoisonRow);

        Dictionary<string, float> BloodRow = new Dictionary<string, float>();

        BloodRow.Add("Default", 1);
        BloodRow.Add("Light", blood_Light);
        BloodRow.Add("Shadow", blood_Shadow);
        BloodRow.Add("Poison", blood_Poison);
        BloodRow.Add("Blood", blood_Blood);

        ElementTable.Add("Blood", BloodRow);

    }

    public float ReturnDamageMultiple(string who, string to) {

        return ElementTable[who][to];
    }
}
