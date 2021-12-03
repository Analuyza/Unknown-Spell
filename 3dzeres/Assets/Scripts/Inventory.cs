using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    public static Inventory inst;
    public bool[] isFull, isSelect;
    public GameObject[] slots;
    public Button btnUseInv;
    public GameObject panelInventory, goBtnSlot;
    public GameObject chaveLivro;
    bool jaAbriuLivro = false;
    public int selected = 0;

    private void Start()
    {
        inst = this;
    }

    private void Update()
    {
        if (selected >= 2) btnUseInv.interactable = true;
        else btnUseInv.interactable = false;
    }

    public void OpenInventario() {
        if (panelInventory.activeInHierarchy) panelInventory.SetActive(false);
        else panelInventory.SetActive(true);
    }
    
    public void CloseInventario() {
        if (panelInventory.activeInHierarchy) panelInventory.SetActive(false);
        
        else if (Player.player.pagLivro.activeInHierarchy) {
            if (!Player.player.puzzleUnlock) {
                Player.player.pagLivro.SetActive(false);
                Player.player.canWalk = true;
                Player.player.puzzleUnlock = true;
            }
            else {
                Player.player.pagLivro.SetActive(false);
                Player.player.puzzle.SetActive(true);
                AudioSource[] musicasAmbient;
                musicasAmbient = GameObject.Find("GameManager").GetComponentsInChildren<AudioSource>();
                foreach (AudioSource audio in musicasAmbient) audio.mute = true;
                StartCoroutine(WaitPuzzle());
            }            
        }
    }

    public void UsarItens() {
        panelInventory.SetActive(false);
        if (!jaAbriuLivro) {
            Player.player.canWalk = false;
            chaveLivro.SetActive(true);
            for (int i = 0; i < 2; i++) {
                isFull[i] = false;
                Destroy(slots[i].transform.GetChild(0).gameObject);
            }
            StartCoroutine(WaitChaveLivro());
            jaAbriuLivro = true;
        }
    }

    IEnumerator WaitChaveLivro() {
        yield return new WaitForSeconds(5.5f);
        Inventory.inst.chaveLivro.SetActive(false);
        Player.player.pagLivro.SetActive(true);
    }

    IEnumerator WaitPuzzle() {
        yield return new WaitForSeconds(6);
        CameraFollow.inst.puzzleStarted2 = true;
        Player.player.doingPuzzle = true;
        Player.player.canWalk = true;
    }
}
