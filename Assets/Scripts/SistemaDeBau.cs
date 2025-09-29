using System.Collections;
using UnityEngine;

public class SistemaDeBau : MonoBehaviour
{
    [SerializeField] private int numeroBau;
    [SerializeField] private bool bauTrancado = false;
    [Header("Caso Tracada, defina o sprite de aviso")]
    [SerializeField] private Sprite spriteAvisoBau;
    [Header("Quantidade Ouro")]
    [SerializeField] private int quantidadeOuro = 0;
    [SerializeField] private GameObject prefabOuro;
    [SerializeField] private GameObject pontoRef;
    [SerializeField] private float tempoGerarOuro = 0.5f;
    private Animator animator;
    private Avisos avisoBau;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (bauTrancado)
        {
            avisoBau = GetComponent<Avisos>();
        }

    }
    public void AbrirPorta(int nChave = 0)
    {
        if (nChave == 0 && !bauTrancado)
        {
            StartCoroutine(GerarOuro());
            animator.SetTrigger("Abrir");
        }
        else if (nChave == numeroBau && bauTrancado)
        {
            StartCoroutine(GerarOuro());
            animator.SetTrigger("Abrir");
            bauTrancado = false;
            avisoBau.DefineTroca(spriteAvisoBau, "Destrancado", Color.green);
        }
    }

    public bool EstaTrancada()
    {
        return bauTrancado;
    }

    IEnumerator GerarOuro()
    {
        for (int i = 0; i < quantidadeOuro; i++)
        {
            Instantiate(prefabOuro, pontoRef.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(tempoGerarOuro);
        }
    }
}
