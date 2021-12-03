using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button btnPlay;
    public GameObject panelLoading, panelCreditos;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void btnJogar() {
        panelLoading.SetActive(true);
        StartCoroutine(WaitMenu());
    }

    IEnumerator WaitMenu() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Game");
    }

    public void Exit() {
        Application.Quit();
    }

    public void Creditos() {
        if (!panelCreditos.activeInHierarchy) panelCreditos.SetActive(true);
        else panelCreditos.SetActive(false);
    }
}
