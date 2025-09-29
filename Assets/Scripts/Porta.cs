using UnityEngine;

public class Porta : MonoBehaviour
{
    [SerializeField] private int numeroPorta;
    [SerializeField] private bool portaTrancada = false;
    [Header("Caso Tracada, defina o sprite de aviso")]
    [SerializeField] private Sprite spriteAvisoPorta;
    private Animator animator;
    private Avisos avisoPorta;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if(portaTrancada)
        {
            avisoPorta = GetComponent<Avisos>();
        }
        
    }
    public void AbrirPorta(int nChave = 0)
    {
        if(nChave == 0 && !portaTrancada)
        {
            animator.SetTrigger("Abrir");
        }
        else if(nChave == numeroPorta && portaTrancada)
        {
            animator.SetTrigger("Abrir");
            portaTrancada = false;
            avisoPorta.DefineTroca(spriteAvisoPorta, "Destrancado", Color.green);
        }
    }

    public bool EstaTrancada()
    {
        return portaTrancada;
    }

}
