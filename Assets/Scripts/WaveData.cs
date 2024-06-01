using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData", order = 1)]
public class WaveData : ScriptableObject
{
    public EnemyWave[] waves;
}

[System.Serializable]
public class EnemyWave
{
    public EnemySpawn[] enemies;
}

[System.Serializable]
public class EnemySpawn
{
    public GameObject enemyPrefab;
    public Vector2 spawnPosition;
    public float spawnDelay;
}
