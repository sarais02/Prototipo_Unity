using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject menuPanel;
    GameManager gm;
    public float time = 6.5f;

    public int index = 0;

    public string[] messages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        messages = new[] { "Encuentra el mega pan, pero prr prr que no te pillen!",
            "Presiona 'F' para lanzar una distraccion",
            "Utiliza la rueda del raton para cambiar tu escala y esconderte mejor",
            "Adaptate a la escala del boton para poder abrir la puerta"};
    }
    void Start()
    {
        gm = GameManager.GetInstance();
        Invoke("HideTargetObject", time);
        HideTargetObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideTargetObject()
    {
        text.gameObject.SetActive(false);
    }
    public void ShowTargetObject()
    {
        if(gm.IsTutorial()) text.gameObject.SetActive(true);
        menuPanel.gameObject.SetActive(false);
    }

    public void ChangeText()
    {
        ShowTargetObject();
        index = (index + 1) % messages.Length; //me aseguro que no pasa los limites del array
        text.text = messages[index];
        Invoke("HideTargetObject", time);
    }
}
