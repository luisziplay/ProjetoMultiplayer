using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SistemaInterativo : MonoBehaviour
{
    
    [Header("Objeto do Canvas que o Icone")]
    [SerializeField] private Image spriteInterface;
    [Header("Objeto do Canvas que o Texto")]
    [SerializeField] private TextMeshProUGUI textoAviso;
    [SerializeField] private float tempoExibir;

    private void Start()
    {
        ProcuraReferencias();
    }

    void Update()
    {
        ProcuraReferencias();
    }

    private void ProcuraReferencias()
    {
        if (spriteInterface == null || textoAviso == null)
        {
            spriteInterface = GameObject.Find("spriteInterface").GetComponent<Image>();
            spriteInterface.enabled = false;
            textoAviso = GameObject.Find("textoAviso").GetComponent<TextMeshProUGUI>();
            textoAviso.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Avisos>(out Avisos a))
        {
            StartCoroutine(ExibirAviso(a.SpriteAviso(), a.AvisoTexto(), a.CorAviso()));
            
            if(a.AvisoTemporario())
            {
                StartCoroutine(TimerAvisoTemporario(other.gameObject));
            }
        }
    }

    IEnumerator TimerAvisoTemporario(GameObject g)
    {
        yield return new WaitForSeconds(tempoExibir);
        Destroy(g);
    }

    IEnumerator ExibirAviso(Sprite s, string t, Color c)
    {
        //Ativando os objetos
        spriteInterface.enabled = true;
        textoAviso.enabled = true;

        //Passando a sprite e definindo a cor do ícone
        spriteInterface.sprite = s;
        spriteInterface.color = c;

        //Passando a sprite e definindo a cor do texto
        textoAviso.text = t;
        textoAviso.color = c;

        //Exibe o aviso por x segundos
        yield return new WaitForSeconds(tempoExibir);

        //Desativando os objetos
        spriteInterface.enabled = false;
        textoAviso.enabled = false;
    }
}
