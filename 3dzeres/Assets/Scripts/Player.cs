using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player player;
    private Inventory inventory;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer = false;
    public float playerSpeed, jumpHeight, gravityValue;
    public Animator animator;
    public bool canWalk = true;
    public AudioClip chave, livro, passosBibliot;
    public AudioSource passos;
    public bool inArea = false, puzzleUnlock = false, doingPuzzle = false;
    string area;
    public bool inBibliot = false;
    GameObject goObjectToPick;
    public GameObject puzzle, pagLivro, instanceBibliot;

    void Start()
    {
        player = this;
        controller = GetComponent<CharacterController>();
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
    }

    void Update()
    {
        if (canWalk) Movement();
        // Pegar objetos
        if (inArea && Input.GetKeyUp(KeyCode.E)) {
            if (area == "chave") {
                PegaChave();
                inArea = false;
                for (int i = 0; i < inventory.slots.Length; i++) {
                    if (!inventory.isFull[i]) {
                        inventory.isFull[i] = true;
                        GameObject btnItem = Instantiate(inventory.goBtnSlot, inventory.slots[i].transform, false);
                        btnItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + goObjectToPick.name);
                        btnItem.gameObject.name = "btn" + goObjectToPick.name;
                        break;
                    }
                }
            }
            else if (area == "livro") {
                PegaLivro();
                inArea = false;
                for (int i = 0; i < inventory.slots.Length; i++) {
                    if (!inventory.isFull[i]) {
                        inventory.isFull[i] = true;
                        GameObject btnItem = Instantiate(inventory.goBtnSlot, inventory.slots[i].transform, false);
                        btnItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/" + goObjectToPick.name);
                        btnItem.gameObject.name = "btn" + goObjectToPick.name;
                        break;
                    }
                }
            }
            else if (area == "quadro" && puzzleUnlock) {
                inArea = false;
                canWalk = false;
                animator.SetBool("Movement", false);
                pagLivro.GetComponentInChildren<Text>().text = "Traca-me com destreza em torno de vos e reteras do orvalho o condao de enfeiticar qualquer alma ou coracao.";
                pagLivro.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        goObjectToPick = other.gameObject;
        if (other.tag == "Collectable") {
            inArea = true;
            if (other.name == "Chave") area = "chave";
            else if (other.name == "Livro") area = "livro";
            else if (other.name == "Quadro") area = "quadro";            
        }
        #region Puzzle
        else if (other.tag == "Puzzle") {
            Debug.Log("ooin");
            canWalk = false;
            StartCoroutine(WaitToWalk());
            transform.localPosition = puzzle.transform.Find("RestartPoint").gameObject.transform.position;
            Puzzles.inst.started = false;
            Puzzles.inst.middlePuzzle = false;
        }
        else if (other.name == "StartPoint") {
            if (!Puzzles.inst.started) Puzzles.inst.started = true;
            else if (Puzzles.inst.started && Puzzles.inst.middlePuzzle) {
                Puzzles.inst.finished = true;
                // Debug.Log("conseguiu amore");
                StartCoroutine(WaitFinal());
            }
        }
        else if (other.name == "MiddlePuzzle") Puzzles.inst.middlePuzzle = true;
        #endregion
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Collectable") {
            inArea = false;
        }
    }

    void Movement() {
        if(animator.GetBool("Movement")) passos.Play();
        else passos.Stop();
        if (inBibliot) passos.clip = passosBibliot;
        if (inBibliot && !doingPuzzle) {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0) {
                playerVelocity.y = 0f;
                animator.SetBool("Movement", false);
                // passos.Stop();
            }

            Vector3 move = new Vector3(Input.GetAxis("Horizontal") * -1, 0, Input.GetAxis("Vertical") * -1);
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero) {
                gameObject.transform.forward = move;
                animator.SetBool("Movement", true);
                // passos.Play();
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
        else {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0) {
                playerVelocity.y = 0f;
                animator.SetBool("Movement", false);
                // passos.Stop();
            }

            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero) {
                gameObject.transform.forward = move;
                animator.SetBool("Movement", true);
                // passos.Play();
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
        if (inBibliot && doingPuzzle) {
            playerSpeed = 7;
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0) {
                playerVelocity.y = 0f;
                animator.SetBool("Movement", false);
                // passos.Stop();
            }

            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero) {
                gameObject.transform.forward = move;
                animator.SetBool("Movement", true);
                // passos.Play();
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }

    void PegaChave() {
        canWalk = false;
        animator.SetBool("Movement", false);
        animator.SetBool("PegandoChave", true);
        StartCoroutine(WaitPeekKey());
        GameObject.Find("GameManager").GetComponent<AudioSource>().PlayOneShot(chave);
    }

    void PegaLivro() {
        canWalk = false;
        animator.SetBool("Movement", false);
        animator.SetBool("PegandoLivro", true);
        StartCoroutine(WaitPeekBook());
        GameObject.Find("GameManager").GetComponent<AudioSource>().PlayOneShot(livro);
    }

    IEnumerator WaitToWalk() {
        animator.SetBool("Movement", false);
        yield return new WaitForSeconds(1);
        canWalk = true;
    }
    IEnumerator WaitPeekKey() {
        StartCoroutine(WaitToDestroy(2));
        yield return new WaitForSeconds(3);
        Destroy(goObjectToPick.gameObject);
        animator.SetBool("PegandoChave", false);
        transform.localPosition = instanceBibliot.transform.position;
        inBibliot = true;
        canWalk = true;
    }
    IEnumerator WaitPeekBook() {
        StartCoroutine(WaitToDestroy(2));
        yield return new WaitForSeconds(2);
        animator.SetBool("PegandoLivro", false);
        canWalk = true;
    }
    IEnumerator WaitToDestroy(float secs) {
        yield return new WaitForSeconds(secs);
        Destroy(goObjectToPick.gameObject);
    }
    IEnumerator WaitFinal() {
        canWalk = false;
        yield return new WaitForSeconds(1f);
        GameManager.inst.final.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.7f);
        GameManager.inst.final.Stop();
        yield return new WaitForSeconds(1.2f);
        GameManager.inst.final.transform.Find("Text").gameObject.SetActive(true);
        GameManager.inst.final.transform.Find("btnMenu").gameObject.SetActive(true);
        GameManager.inst.final.transform.Find("btnSair").gameObject.SetActive(true);
    }
}
