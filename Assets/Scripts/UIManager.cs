using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public float time = 8f;

    public int index = 0;

    public string[] messages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        messages = new[] { "Encuentra el mega pan, pero prr prr que no te pillen!",
            "Presiona 'F' para lanzar una distraccion",
            "Utiliza la rueda del raton para cambiar tu escala",
            "Adaptate al tamaño del boton para poder abrir la puerta"};
    }
    void Start()
    {
        Invoke("HideTargetObject", time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideTargetObject()
    {
        text.gameObject.SetActive(false);
    }

    void ShowTargetObject()
    {
        text.gameObject.SetActive(true);
    }

    public void ChangeText()
    {
        ShowTargetObject();
        index = (index + 1) % messages.Length; //me aseguro que no pasa los limites del array
        text.text = messages[index];
        Invoke("HideTargetObject", time);
    }
}
