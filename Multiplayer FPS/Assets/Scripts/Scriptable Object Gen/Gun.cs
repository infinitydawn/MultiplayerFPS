using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public float aimSpeed;
    public string Name;
    public float fireRate;
    public GameObject prefab;
}
