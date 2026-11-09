using UnityEngine;
using static PlayerStats;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Datos del Objeto")]
    public string itemName;
    public string description;
    public string negativeDescription;
    public bool isBig;
    public Sprite icon;

    [Header("Efecto en las Estadísticas")]
    public StatType targetStat;
    public float modifierAmount;
}
