using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;


    [SerializeField] 
    TMP_Text kalanZamanTxt;

    [SerializeField]
    TMP_Text skorTxt;

    public int kalanZaman = 5;

    [SerializeField]
    GameObject turSonucPanel;

    [HideInInspector]
    public int gecerliPuan;

    [SerializeField]
    GameObject pausePanel;

    [SerializeField] TMP_Text scoreDisplayText;
    [SerializeField] TMP_Text HighScoreDisplayText;







    public bool turBittimi;

    Board board;
    public string anamenu;
    float geciciSkor;

    private void Awake()
    {
        Instance = this;

        board = Object.FindObjectOfType<Board>();
    }
    private void Start()
    {
        turBittimi = false;
        StartCoroutine(GeriSayRouitine());
        

        scoreDisplay();
        HighScoreDisplay();
        

       


    }

    private void Update()
    {
        geciciSkor = gecerliPuan;

        if (PlayerPrefs.GetInt("highScore") == 0)
        {
            PlayerPrefs.SetInt("highScore", (int)gecerliPuan);
        }
        else if (gecerliPuan > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", (int)gecerliPuan);
            HighScoreDisplayText.text = "High Score: " + PlayerPrefs.GetInt("highScore").ToString();
        }
        else
        {
            geciciSkor = gecerliPuan;
        }
    }

    IEnumerator GeriSayRouitine()
    {
        while(kalanZaman>0)
        {
            yield return new WaitForSeconds(1f);

            kalanZamanTxt.text = kalanZaman.ToString() + " s";
            kalanZaman--;


            if(kalanZaman <=0)
            {
                soundManager.Instance.OyunBittiSesiCikar();
                turBittimi = true;
                turSonucPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void PuaniArtirFNC(int gelenPuan)
    {
        gecerliPuan += gelenPuan;
        skorTxt.text = gecerliPuan.ToString() + " Puan";
        scoreDisplayText.text = "Your score : " + (int)gecerliPuan;
        



    }

    public void karistirFNC()
    {
        board.BoardKaristirFNC();
    }

    public void OyunuDurdurAc()
    {
        if(!pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }


    public void AnaMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(anamenu);
    }
    public void OyunaDon()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }

    public void scoreDisplay()
    {
        scoreDisplayText.text = "Your score : " + (int)gecerliPuan;
    }

    public void HighScoreDisplay()
    {
        HighScoreDisplayText.text = "High Score: " + PlayerPrefs.GetInt("highScore").ToString();
    }




}
