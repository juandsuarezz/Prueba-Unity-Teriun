using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public bool canPlay, zombieAppear;
    public int coins, coinsCount, score, highscore;
    public PlayerController player;
    public GameObject coin, startPanel, gameOverPanel, gamePanel, zombie, mainPlayer;
    public Text monedas, puntaje, record, final;

    
    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("newhighscore", 0);
        Time.timeScale = 0;
        zombieAppear = true;
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gamePanel.SetActive(false);
        coinsCount = 0;
        coins = 0;
        canPlay = true;
        player = FindObjectOfType<PlayerController>();
        InstantiateZombie();
        InstantiateZombie();
    }

    // Update is called once per frame
    void Update()
    {
        if(canPlay)
        {
            if(coinsCount < 5)
            {
                var newCoin = Instantiate(coin, new Vector3(Random.Range(-60f, 60f), 5.38f, Random.Range(-21f, 54f)), Quaternion.identity);
                newCoin.transform.parent = transform.Find("Coins");
                coinsCount += 1;
            }

            if(zombieAppear)
            {
                InstantiateZombie();
                zombieAppear = false;
                StartCoroutine("AppearZombie");
            }
        }

        monedas.text = "Items: " + coins.ToString();
        puntaje.text = "Puntaje: " + score.ToString();

    }

    IEnumerator AppearZombie()
    {
        yield return new WaitForSeconds(2.4f);
        zombieAppear = true;

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        final.text = "Puntaje Final: " + score.ToString();
        Time.timeScale = 0;

        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("newhighscore", highscore);
        }

        record.text = "Record: " + highscore.ToString();

    }

    public void InstantiateZombie()
    {
        var newZombie = Instantiate(zombie, new Vector3(Random.Range(-60f, 60f), 10f, Random.Range(-21f, 54f)), Quaternion.identity);
        newZombie.transform.parent = transform.Find("Enemies");
    }

}
