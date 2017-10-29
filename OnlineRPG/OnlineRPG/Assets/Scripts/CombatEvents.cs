using UnityEngine;

public class CombatEvents : MonoBehaviour
{
    public delegate void OnEnemyDeathDelegate(IEnemy enemy);
    public static OnEnemyDeathDelegate OnEnemyDeath;
}