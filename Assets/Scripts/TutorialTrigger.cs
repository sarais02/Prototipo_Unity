using UnityEngine;
using DG.Tweening;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] UIManager ui;
    GameManager gm;
    private bool enter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enter = false;
        gm = GameManager.GetInstance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gm && gm.IsTutorial() && other.GetComponent<PlayerController>() && !enter) 
        { 
            enter = true;
            ui.ChangeText();
            //Si es el ultimo elemento activar la flecha
            if(ui.index == ui.messages.Length - 1)
            {
                SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
                sprite.enabled = true;
                sprite.gameObject.transform.DOPunchScale(new Vector3(0.8f, 0.8f, 1), 1f, 5);

                Invoke("DisableImage", ui.time);
            }
        }
        else if(gm && !gm.IsTutorial()) ui.HideTargetObject();
    }

    private void DisableImage()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}
