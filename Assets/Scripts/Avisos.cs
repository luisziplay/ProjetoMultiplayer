using UnityEngine;

public class Avisos : MonoBehaviour
{   
    
    [Header("Aviso do Jogo/Objeto")]
    [TextArea]
    [SerializeField] private string avisoTexto;
    [Header("Sprite para aparecer junto com o texto")]
    [SerializeField] private Sprite spriteAviso;
    [Header("Cor do aviso")]
    [SerializeField] private Color corAviso = Color.white;
    [Header("O Aviso é Temporário?")]
    [SerializeField] private bool avisoTemporario = false;


    public string AvisoTexto()
    {
       return avisoTexto;
    }

    public Sprite SpriteAviso()
    {
        return spriteAviso;
    }

    public Color CorAviso()
    {
        return corAviso;
    }

    public void DefineTroca(Sprite s, string t, Color c)
    {
        spriteAviso = s;
        avisoTexto = t;
        corAviso = c;
    }

    public bool AvisoTemporario()
    {
        return avisoTemporario;
    }

}
