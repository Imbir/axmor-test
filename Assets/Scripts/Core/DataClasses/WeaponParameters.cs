using System;
using Core.MonoBehaviours;
using UnityEngine;

[Serializable]
public class WeaponParameters
{
    public string[] Names;
    public int[] DamageValues;
    public float[] ShotPauseValues;
    public float[] ProjectileSpeeds;
    public Transform ProjectileSource;
    public Bullet ProjectilePrefab;
}