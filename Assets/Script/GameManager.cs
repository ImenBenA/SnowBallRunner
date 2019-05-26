using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { set; get; }

    private bool isPaused = false;
    public GameObject DarkAspect;
    public GameObject PauseMenu;
    public bool IsDead { set;get; }
    private bool isGameStarted = false;
    private PlayerController controller;
    //UI fields 
    public Text scoreText, coinText, modifierText, highScoreText;
    private float score, coinScore, modifierScore;
    private int lastScore;
    //death menu
    public Animator GameCanvas, MenuAnim;
    public Animator deathMenuAnim;
    public Text deadscoreText, deadcointText;
    
    void Awake()
    {
        Instance = this;
        highScoreText.text = "HighScore : " + PlayerPrefs.GetFloat("HighScore").ToString("0");
        modifierScore = 3;
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        modifierText.text = "Speed x" + modifierScore.ToString("0.0");
        coinText.text = "Gems : "+coinScore.ToString("0");
        scoreText.text ="Score : "+ score.ToString("0");
    }
    private void Update()
    {

        if (MobileInputs.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            controller.StartRunning();
            FindObjectOfType<CameraController>().IsMoving = true;
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            //GameCanvas.SetTrigger("Show");
            MenuAnim.SetTrigger("Hide");
        }
        if (isGameStarted && !IsDead && !isPaused)
        {
            PauseShow();
            score += (Time.deltaTime * modifierScore);
            if (lastScore != (int )score)
            {
                lastScore = (int)score;
                scoreText.text = "Score : "+score.ToString("0");
            }
        }
    }
    public void GetCoin()
    {
        coinScore++;
        coinText.text = "Gems : "+coinScore.ToString("0");
        score += 5; 
        scoreText.text = score.ToString("0");
    }
    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text ="Speed x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    public void OnPauseButton()
    {
        if (isPaused)
        {
            isPaused = false;
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            controller.StartRunning();
            DarkAspect.SetActive(false);
        }
        else
        {
            isPaused = true;
            FindObjectOfType<GlacierSpawner>().IsScrolling = false;
            controller.StopRunning();
            DarkAspect.SetActive(true);
        }
    }
    public void OnDeath()
    {
        HideShow();
        IsDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deadscoreText.text = "Score : "+score.ToString("0");
        deadcointText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        if (score > PlayerPrefs.GetFloat("HighScore")) {
              PlayerPrefs.SetFloat("HighScore", score);
              highScoreText.text = "New HighScore : " + score.ToString("0");
        } 
          
    }
    private void PauseShow()
    {
        if (!PauseMenu.active)
            PauseMenu.SetActive(true);
        scoreText.gameObject.SetActive(true);
        coinText.gameObject.SetActive(true);
        modifierText.gameObject.SetActive(true);

    }
    private void HideShow()
    {
        if (PauseMenu.active)
            PauseMenu.SetActive(false);
        scoreText.gameObject.SetActive(false);
        coinText.gameObject.SetActive(false);
        modifierText.gameObject.SetActive(false);
    }
}
