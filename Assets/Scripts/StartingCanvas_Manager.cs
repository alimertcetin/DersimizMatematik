using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartingCanvas_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Textler = new GameObject[5];
    [SerializeField]
    TMP_Text NextEnd_btn_Text = null;
    string orj_Text;

    [SerializeField]
    GameObject btn_Prev = null;

    int tracker = 0;
    float timer = 0;
    bool stopped = false;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 0.5 && !stopped)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            stopped = true;
        }
    }

    void Disable_Method()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    private void OnDisable()
    {
        Disable_Method();
    }

    private void OnDestroy()
    {
        Disable_Method();
    }

    private void Start()
    {
        orj_Text = NextEnd_btn_Text.text;
        btn_Prev.SetActive(false);
        foreach (var item in Textler)
        {
            item.SetActive(false);
        }
        Textler[0].SetActive(true);
    }

    //btn_Next
    public void btn_NEXT()
    {
        tracker++;
        if (tracker > 0)
        {
            btn_Prev.SetActive(true);
            if (tracker > Textler.Length - 1)
                Destroy(this.gameObject);
            else
            {
                foreach (var item in Textler)
                {
                    item.SetActive(false);
                }

                Textler[tracker].SetActive(true);
            }
        }

        if (tracker == Textler.Length - 1)
            NextEnd_btn_Text.text = "Son";
        else
            NextEnd_btn_Text.text = orj_Text;

    }

    //btn_Previous
    public void btn_Prev_Method()
    {
        tracker--;
        if (tracker < 0) tracker = 0;

        foreach (var item in Textler)
        {
            item.SetActive(false);
        }

        Textler[tracker].SetActive(true);

        if (tracker < Textler.Length - 1)
            NextEnd_btn_Text.text = orj_Text;

        if (tracker == 0) btn_Prev.SetActive(false);
    }
}
