using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    public List<FogWall> fogWalls;
    public UIBossHealthBar bosshealthBar;
    public EnemyBossManager bossManager;

    public bool bossFightIsActive;
    public bool bossHasBeenAwakened;
    public bool bossHasBeenDefeated;

    private void Awake()
    {
        bosshealthBar = FindObjectOfType<UIBossHealthBar>();
    }

    public void ActivateBossFight()
    {
        bossFightIsActive = true;
        bossHasBeenAwakened = true;
        bosshealthBar.SetUIHealthBarToArctive();

        foreach(var fogWall in fogWalls)
        {
            fogWall.ActivateFogWall();
        }
    }

    public void BossHasBeenDefeated()
    {
        bossHasBeenDefeated = true;
        bossFightIsActive = false;

        foreach (var fogWall in fogWalls)
        {
            fogWall.DeactiveFogWall();
        }

    }
}
