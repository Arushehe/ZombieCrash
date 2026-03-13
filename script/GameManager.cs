using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float gameTimer = 60f;
    public int score = 0;

    public TMP_Text timerText;
    public TMP_Text scoreText;

    public GameObject zombiePrefab;
    public int minZombies = 10;

    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    public GameObject endScreen;
    public TMP_Text finalScoreText;

    private bool gameEnded = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        SpawnInitialZombies();
        UpdateUI();
    }

    void Update()
    {
        if (gameEnded) return;

        if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            timerText.text = "TIME: " + Mathf.CeilToInt(gameTimer);
        }

        if (gameTimer <= 0)
        {
            EndGame();
        }
    }

    void SpawnInitialZombies()
    {
        for (int i = 0; i < minZombies; i++)
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            0,
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );

        Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
    }

    public void ZombieHit()
    {
        score++;
        UpdateUI();
        StartCoroutine(RespawnZombie());
    }

    IEnumerator RespawnZombie()
    {
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        SpawnZombie();
    }

    void UpdateUI()
    {
        scoreText.text = "SCORE: " + score;
        timerText.text = "TIME: " + Mathf.CeilToInt(gameTimer);
    }

    void EndGame()
    {
        gameEnded = true;

        endScreen.SetActive(true);
        finalScoreText.text = "FINAL SCORE: " + score;

        Time.timeScale = 0;
    }
}