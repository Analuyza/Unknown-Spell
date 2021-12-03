using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public Text txtInicial;
    string text;
    public Puzzles puzzle;
    public VideoPlayer final;

    void Start()
    {
        inst = this;
        
        text = "O pequeno alquimista esta em busca do conhecimento para aprender um feitico novo, porem, os antigos alquimistas esconderam esse conhecimento dentro de um livro magico, escondido em uma biblioteca e selado por uma fechadura dourada\nDizem que a chave para destravar a fechadura esta em um bosque misterioso...\nAjude o pequeno alquimista a aprender o feitico novo!";

        StartCoroutine(EffectTypewriter(text));
        StartCoroutine(WaitToCloseTextInitial());
    }

    public void Menu() {
        SceneManager.LoadScene("Menu");
    }

    public void Sair() {
        Application.Quit();
    }

    IEnumerator EffectTypewriter(string text) {
        foreach (char character in text.ToCharArray()) {
            txtInicial.text += character;
            yield return new WaitForSeconds(.02f);
        }
    }
    IEnumerator WaitToCloseTextInitial() {
        yield return new WaitForSeconds(20);
        Destroy(txtInicial.transform.parent.gameObject);
    }
}
