using System;
using Core.MonoBehaviours;
using UnityEngine;

[Serializable]
public class WeaponParameters
{
    public int[] DamageValues;
    public float[] ShotPauseValues;
    public float[] ProjectileSpeeds;
    public Transform ProjectileSource;
    public Bullet ProjectilePrefab;
}