using UnityEngine;

public class BackgroundColorChange : MonoBehaviour
{
    public SpriteRenderer background;  // Referência ao SpriteRenderer do background
    public Color defaultColor = Color.white;  // Cor padrão do background
    public Color activeColor = Color.red;     // Cor quando o Player entra no background

    private void Start()
    {
        // Certifica-se de que o background começa com a cor padrão
        if (background != null)
        {
            background.color = defaultColor;
        }
        else
        {
            // Debug.LogError("Background não atribuído no Inspector.");
        }

        // Verifica se o Player já está no background ao iniciar o jogo
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                background.color = activeColor;
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o Player entrou no background
        if (other.CompareTag("Player"))
        {
            // Muda a cor do background para a cor ativa
            if (background != null)
            {
                background.color = activeColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Verifica se o Player saiu do background
        if (other.CompareTag("Player"))
        {
            // Muda a cor do background de volta para a cor padrão
            if (background != null)
            {
                background.color = defaultColor;
            }
        }
    }
}
