using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SistemaDeVida : MonoBehaviour
{
    [SerializeField] private int vida = 100;
    [SerializeField] private int mana = 100;
    [SerializeField] private Slider manaIndicador;
    [SerializeField] private Slider vidaIndicador;
    private bool estaVivo = true;
    private bool levarDano = true;
    private PlayerMovement pMove;
    private bool podeRecarregarMana = true;

    void Start()
    {
        ProcuraReferencias();
    }

    void Update()
    {
        ProcuraReferencias();
    }

    private void ProcuraReferencias()
    {
        if (manaIndicador == null)
        {
            manaIndicador = GameObject.Find("Mana").GetComponent<Slider>();
            manaIndicador.maxValue = 100;
            manaIndicador.value = mana;
        }

        if (vidaIndicador == null)
        {
            vidaIndicador = GameObject.Find("Vida").GetComponent<Slider>();
            vidaIndicador.maxValue = 100;
            vidaIndicador.value = vida;
        }

        if (pMove == null)
        {
            pMove = GetComponent<PlayerMovement>();
        }
    }

    public bool EstaVivo()
    {
        return estaVivo;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fatal") && estaVivo && levarDano)
        {
            StartCoroutine(LevarDano(10));
        }
    }

    public void TomarDano(int dano)
    {
        if (estaVivo && levarDano)
        {
            StartCoroutine(LevarDano(dano));
        }
    }

    IEnumerator LevarDano(int dano)
    {
        levarDano = false;

        if (vida > 0)
        {
            pMove.Hit(); // Chama o método Hit do PlayerMovement para executar a animação de dano
            vida -= dano;
            vidaIndicador.value = vida;
            VerificarVida();
            yield return new WaitForSeconds(0.5f);
            levarDano = true;
        }
    }

    private void VerificarVida()
    {
        if(vida <= 0)
        {
            vida = 0;
            estaVivo = false;
        }
    }

    public void UsarMana()
    {
        mana -= 10;
        manaIndicador.value = mana;
        if(podeRecarregarMana)
        {
            StartCoroutine("RecarregaMana");
        }
    }

    public void CargaMana(int carga)
    {
        mana += carga;
        manaIndicador.value = mana;
        if (mana > 100)
        {
            mana = 100;
            manaIndicador.value = vida;
        }
    }

    public void CargaVida(int carga)
    {
        vida += carga;
        vidaIndicador.value = vida;
        if (vida > 100)
        {
            vida = 100;
            vidaIndicador.value = vida;
        }
    }

    public int GetMana()
    {
        return mana;
    }

    IEnumerable RecarregaMana()
    {
        podeRecarregarMana = false;
        while (mana < 100)
        {
            mana += 5;
            manaIndicador.value = mana;
            yield return new WaitForSeconds(0.1f);
        }
        mana = 100;
        podeRecarregarMana = true;
    }
}
