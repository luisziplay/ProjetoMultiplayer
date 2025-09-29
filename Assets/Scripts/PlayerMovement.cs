using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float inputH;
    private float inputV;
    private Animator animator;
    private bool estaNoChao = true;
    private float velocidadeAtual;
    private bool contato = false;
    private bool parado = false;
    private bool morrer = true;
    private SistemaDeVida sVida;
    private SistemaInterativo sInterativo;
    private Vector3 anguloRotacao = new Vector3(0, 90, 0);
    private bool temChave = false;
    private int numeroChave = 0;
    [SerializeField] private float velocidadeAndar;
    [SerializeField] private float velocidadeCorrer;
    [SerializeField] private float forcaPulo;
    [SerializeField] private GameObject magiaPreFab;
    [SerializeField] private GameObject quebraPreFab;
    [SerializeField] private GameObject miraMagia;
    [SerializeField] private int forcaArremeco;
    [SerializeField] private CinemachineCamera cineCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        sVida = GetComponent<SistemaDeVida>();
        sInterativo = GetComponent<SistemaInterativo>();
        velocidadeAtual = velocidadeAndar;
        ProcuraReferencias();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            contato = true;
        }

        if (sVida.EstaVivo() && !parado)
        {
            Pular();
            Magia();
        }
        else if (!sVida.EstaVivo() && morrer)
        {
            Morrer();
        }

        ProcuraReferencias();
    }

    void FixedUpdate()
    {
        if (sVida.EstaVivo() && !parado)
        {
            Andar();
            //Girar();
            Correr();
        }
    }

    private void ProcuraReferencias()
    {
        if(cineCamera == null)
        {
            transform.position = GameObject.Find("StartPoint").transform.position;
            cineCamera = GameObject.Find("CinemachineCamera").GetComponent<CinemachineCamera>();
            cineCamera.Follow = this.gameObject.transform;
        }
    }
    private void Andar()
    {
        float inputH = Input.GetAxis("Horizontal");

        // Movimento apenas no eixo X (side scroller 3D)
        Vector3 moveDirection = new Vector3(inputH, 0, 0);

        if (moveDirection != Vector3.zero)
        {
            // Aplica movimento
            Vector3 moveForward = rb.position + moveDirection * velocidadeAtual * Time.deltaTime;
            rb.MovePosition(moveForward);

            // Faz o personagem virar para a direção do movimento
            Quaternion novaRotacao = Quaternion.LookRotation(moveDirection);
            transform.rotation = novaRotacao;
        }

        // Animações
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Andar", true);
            animator.SetBool("AndarTras", false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("AndarTras", true);
            animator.SetBool("Andar", false);
        }
        else
        {
            animator.SetBool("AndarTras", false);
            animator.SetBool("Andar", false);
        }
    }

    private void Girar()
    {
        inputH = Input.GetAxis("Horizontal");
        Quaternion deltaRotation =
            Quaternion.Euler(anguloRotacao * inputH * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        if (Input.GetKey(KeyCode.A) ||
                    Input.GetKey(KeyCode.D) ||
                        Input.GetKey(KeyCode.LeftArrow) ||
                            Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("Andar", true);
        }
    }

    private void Pular()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
            animator.SetTrigger("Pular");
        }
    }

    private void Correr()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            velocidadeAtual = velocidadeCorrer;
            animator.SetBool("Correr", true);
        }
        else
        {
            velocidadeAtual = velocidadeAndar;
            animator.SetBool("Correr", false);
        }
    }

    private void Morrer()
    {
        morrer = false;
        animator.SetBool("EstaVivo", false);
        animator.SetTrigger("Morrer");
        rb.Sleep();
    }

    private void Interagir()
    {
        StartCoroutine(Parado());
        animator.SetTrigger("Interagir");
    }

    private void Pegar()
    {
        StartCoroutine(Parado());
        animator.SetTrigger("Pegar");
    }

    IEnumerator Parado()
    {
        parado = true;
        yield return new WaitForSeconds(1.5f);
        parado = false;
    }

    private int Atacar()
    {
        animator.SetTrigger("Atacar");
        Instantiate(quebraPreFab , miraMagia.transform.position, miraMagia.transform.rotation);
        contato = false; 
        return 10;
    }

    private void Magia()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (sVida.GetMana() > 0)
            {
                StartCoroutine(LancarMagia());
                animator.SetTrigger("Magia");
            }
        }
    }

    IEnumerator LancarMagia()
    {

        yield return new WaitForSeconds(0.5f);
        GameObject magia = Instantiate(magiaPreFab, miraMagia.transform.position, miraMagia.transform.rotation);
        //magia.transform.rotation *= Quaternion.Euler(0, -90, 0); //Machado
        //magia.transform.rotation *= Quaternion.Euler(90, 0, 180); //Flecha
        Rigidbody rbMagia = magia.GetComponentInChildren<Rigidbody>();
        rbMagia.AddForce(miraMagia.transform.forward * forcaArremeco, ForceMode.Impulse);
        sVida.UsarMana();
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            estaNoChao = true;
            animator.SetBool("EstaNoChao", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            estaNoChao = false;
            animator.SetBool("EstaNoChao", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Mana") && Input.GetKey(KeyCode.E))
        {
            Pegar();
            sVida.CargaMana(50);
            Destroy(other.gameObject);
        }
        else if(other.CompareTag("Vida") && Input.GetKey(KeyCode.E))
        {
            Pegar();
            sVida.CargaVida(50);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Porta") && Input.GetKey(KeyCode.E))
        {
            if (other.gameObject.GetComponent<Porta>().EstaTrancada())
            {
                Interagir();
                other.gameObject.GetComponent<Porta>().AbrirPorta(numeroChave);
            }
            else if (!other.gameObject.GetComponent<Porta>().EstaTrancada())
            {
                Interagir();
                other.gameObject.GetComponent<Porta>().AbrirPorta();
            }
        }
        else if (other.CompareTag("Bau") && Input.GetKey(KeyCode.E))
        {
            if (other.gameObject.GetComponent<SistemaDeBau>().EstaTrancada())
            {
                Interagir();
                other.gameObject.GetComponent<SistemaDeBau>().AbrirPorta(numeroChave);
            }
            else if (!other.gameObject.GetComponent<SistemaDeBau>().EstaTrancada())
            {
                Interagir();
                other.gameObject.GetComponent<SistemaDeBau>().AbrirPorta();
            }
        }
        else if (other.CompareTag("Chave") && Input.GetKey(KeyCode.E))
        {
            Pegar();
            temChave = true;
            numeroChave = other.gameObject.GetComponent<Chave>().NumeroPorta();
            other.gameObject.GetComponent<Chave>().PegarChave();
        }

        if (other.gameObject.CompareTag("Quebra"))
        {
            if(contato)
            {
               other.gameObject.GetComponent<ObjetoQuebra>().Quebrar(Atacar()); 
            }
        }
    }

}
