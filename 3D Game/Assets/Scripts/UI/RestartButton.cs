using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RestartButton : MonoBehaviour
{

    public Button restartButton;
    [SerializeField] private TextMeshProUGUI restartTMP;
    // Start is called before the first frame update
    void Awake()
    {
        restartButton.onClick.AddListener(Restart);
        //restartTMP = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        restartTMP.DOFade(0f, 0f).SetDelay(0.5f);
        restartTMP.DOFade(1f, 1f).SetDelay(1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }
}
