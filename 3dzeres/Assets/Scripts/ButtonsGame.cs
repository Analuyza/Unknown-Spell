using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsGame : MonoBehaviour
{
    Inventory inventory;

    void Start()
    {
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
    }

    public void SelectSlot() {
        if (gameObject.transform.parent.name.Contains("Slot")) {
            for (int i = 0; i < inventory.slots.Length; i++) {
                if (inventory.isFull[i] && inventory.slots[i].name == transform.parent.name) {
                    if (!inventory.isSelect[i]) {
                        inventory.isSelect[i] = true;
                        Color myColor = new Color();
                        ColorUtility.TryParseHtmlString("#70E708", out myColor);
                        gameObject.transform.parent.gameObject.GetComponent<Image>().color = myColor;
                        inventory.selected++;
                        break;
                    }
                    else {
                        inventory.isSelect[i] = false;
                        Color myColor = new Color();
                        ColorUtility.TryParseHtmlString("#FFFFFF", out myColor);
                        gameObject.transform.parent.gameObject.GetComponent<Image>().color = myColor;
                        inventory.selected--;
                        break;
                    }
                }
            }
        }
    }
}
