using UnityEngine;

public class SpawnManager : MonoBehaviour
{
//******* Public Values Zone ******//
    public GameObject[]     enemies;
    public GameObject       airBalloon;
    public GameObject       bombBalloon;
    public GameObject       barbedBalloon;
    public GameObject       bonusBalloon;
    public float            inStartSecScoreBl, spawnSpeedScoreBl;
    public float            inStartSecAirBl, spawnSpeedAirBl;
    public float            inStartBombBl, spawnSpeedBombBl;
    public float            inStartBarbedBl, spawnSpeedBarbedBl;
    public float            inStartBonusBl, spawnSpeedBonusBl;

    //******* Private Values Zone ******//
    private float           zEnemySpawn = 1f;
    private float           yStatic = 17;

    void Start()
    {
        InvokeRepeating("ScoreBalloonSpawn", inStartSecScoreBl, spawnSpeedScoreBl);
        InvokeRepeating("AirBalloonSpawn", inStartSecAirBl, spawnSpeedAirBl);
        InvokeRepeating("BombBalloonSpawn", inStartBombBl, spawnSpeedBombBl);
        InvokeRepeating("BarbedBalloonSpawn", inStartBarbedBl, spawnSpeedBarbedBl);
        InvokeRepeating("BonusBalloonSpawn", inStartBonusBl, spawnSpeedBonusBl);
    }

    void BarbedBalloonSpawn() 
    {
        float randomScale = Random.Range(2, 4);
        float randomX = Random.Range(-3, 3);
        Vector3 randomScaleVec = new Vector3(randomScale, randomScale, randomScale);
        Vector3 randomSpawn = new Vector3(randomX, -yStatic, -zEnemySpawn);
        barbedBalloon.transform.localScale = randomScaleVec;
        Instantiate(barbedBalloon, randomSpawn, barbedBalloon.transform.rotation);
    }

    void BombBalloonSpawn() 
    {
        float randomScale = Random.Range(2, 3);
        float randomX = Random.Range(-2, 2);
        Vector3 randomScaleVec = new Vector3(randomScale, randomScale, randomScale);
        Vector3 randomSpawn = new Vector3(randomX, -yStatic, -zEnemySpawn);
        bombBalloon.transform.localScale = randomScaleVec;
        Instantiate(bombBalloon, randomSpawn, bombBalloon.transform.rotation);
    }

    void AirBalloonSpawn() 
    {
        float randomScale = Random.Range(2, 3);
        float randomX = Random.Range(-3, 3);
        Vector3 randomScaleVec = new Vector3(randomScale, randomScale, randomScale);
        Vector3 randomSpawn = new Vector3(randomX, -yStatic, -zEnemySpawn);
        airBalloon.transform.localScale = randomScaleVec;
        Instantiate(airBalloon, randomSpawn, airBalloon.transform.rotation);
    }

    void ScoreBalloonSpawn()
    {
       float randomX = Random.Range(-3, 3);
       int enemyRandom = Random.Range(0, enemies.Length);
       Vector3 randomSpawn = new Vector3(randomX, -yStatic, -zEnemySpawn);
       Instantiate(enemies[enemyRandom], randomSpawn, enemies[enemyRandom].transform.rotation);
    }

    void BonusBalloonSpawn()
    {
        float randomScale = Random.Range(2, 3);
        float randomX = Random.Range(-2, 2);
        Vector3 randomScaleVec = new Vector3(randomScale, randomScale, randomScale);
        Vector3 randomSpawn = new Vector3(randomX, -yStatic, -zEnemySpawn);
        bonusBalloon.transform.localScale = randomScaleVec;
        Instantiate(bonusBalloon, randomSpawn, bonusBalloon.transform.rotation);
    }
}
