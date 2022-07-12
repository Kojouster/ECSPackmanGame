using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.SceneManagement;
using Unity.Entities;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score,lives, level;
    public TextMeshProUGUI pelletUI, scoreTxt, livesTxt;
    public GameObject  WinUI, LoseUI, gameUI;


    // loading this instance before game starts
    public void Awake()
    {
        Instance = this; 
    }
    
    public void Reset()
    {
      
       
        score = 0;
        lives = 3;

      
    }

    // Losing life method
    public void LoseLife()
    {

      
        lives--;
        livesTxt.text = "Lives : " + lives;

        if (lives < 0)
        {
            
            Lose();
        
        }
    
    }
    //In game music and ui
    public void InGame()
    {
        SwitchUI(gameUI);
        AudioManager.inst.PlayMusicRequests("game");

    }

    public void Win()
    {
        SwitchUI(WinUI);
       // AudioManager.inst.PlayMusicRequests("win");
       
    }

    // Lose condition
    public void Lose()
    {
       
        SwitchUI(LoseUI);
        AudioManager.inst.PlayMusicRequests("lose");

    }

    // Made for convinience for switching different UI states
    public void SwitchUI(GameObject newUI)
    {

        WinUI.SetActive(false);
        LoseUI.SetActive(false);
        gameUI.SetActive(false);

        newUI.SetActive(true);

    }

    // Adding Score
    public void AddScore(int points)
    {
        score += points;
        scoreTxt.text = "Score : " + score;
    
    }


    public void SetPelletCount(int pelletCount )
    {
        // if there is a winUi within the scene it will switch to the in game UI
        if (WinUI.activeInHierarchy)
        {
            InGame();
        }

      


      

        pelletUI.text = "Pellets: " + pelletCount;

        if (pelletCount == 0)
            Win();
          


    }
    /*
    public void LoadNextlevel()
    {
      
        LoadLevel(level + 1);
        InGame();
       


    }
   
    public void LoadLevel(int nextLevel)
    {
        if (nextLevel > 3)
        {
           
            
            return;
            
        }
       
        UnLoadLevel();
        level = nextLevel;
        SceneManager.LoadScene ("Level" + level);
        


    }
    public void UnLoadLevel()
    {
        // accessing our default word that i made in the inspectors
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        foreach (Entity e in em.GetAllEntities())
            em.DestroyEntity(e);

        if (SceneManager.GetSceneByName("Level" + level).isLoaded)
            SceneManager.UnloadSceneAsync("Level" + level);

    }*/

    public void LoadLevel1()
    {
       
        SceneManager.LoadScene("Level1");
        Reset();
    }

    public void LoadLevel2()
    {

        SceneManager.LoadScene("Level2");
        Reset();
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
        Reset();
    }
    public void ReturnToMenu()
    {
        // not really efficient way between switching neetween scenes as scenes work different in ecs 
        // scene has to be unloded before loading next level otherwise levels will stuck on top of each other
        var em = World.DefaultGameObjectInjectionWorld.EntityManager; 
        foreach (Entity e in em.GetAllEntities())
            em.DestroyEntity(e);
        SceneManager.LoadScene("Level 0");
        SceneManager.UnloadSceneAsync("Level1");

    }
    // Same technique was applied for other levels, in the future I am planning to make levels switch automatically,this is kind of an easy way to get around the problem with switching scenes
    public void ReturnToMenuFromlevel2()
    {

        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        foreach (Entity e in em.GetAllEntities())
            em.DestroyEntity(e);
        SceneManager.LoadScene("Level 0");
        SceneManager.UnloadSceneAsync("Level2");
    }

    public void ReturnToMenuFromLevel3()
    {

        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        foreach (Entity e in em.GetAllEntities())
            em.DestroyEntity(e);
        SceneManager.LoadScene("Level 0");
        SceneManager.UnloadSceneAsync("Level3");

    }

    // Restarting the current level
    public void RestartLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
