using UnityEngine;
using TMPro;

/// <summary>
/// UIPickup - Exibe texto na tela quando o player mira em objeto pegável
/// 
/// Como funciona:
///   - A cada frame verifica se o player está mirando em objeto com tag Pickup
///   - Se sim, mostra o texto "Pressione E para pegar"
///   - Se não, esconde o texto
/// 
/// Configuração:
///   - Adicione este script no Canvas
///   - Arraste o TextoPickup (TextMeshPro) no campo Texto Pickup
/// 
/// Autor: Ricardo Augusto - WoodShop-VR
/// </summary>
public class UIPickup : MonoBehaviour
{
    [Tooltip("Arraste o objeto TextoPickup aqui no Inspector")]
    public TextMeshProUGUI textoPickup;

    // Referência ao player na cena
    private PlayerControllerPC player;

    void Start()
    {
        // Busca o player automaticamente
        player = FindObjectOfType<PlayerControllerPC>();

        // Começa com o texto escondido
        if (textoPickup != null)
            textoPickup.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null || textoPickup == null) return;

        // Mostra ou esconde o texto baseado na mira do player
        bool mirando = player.EstaMirandoPickup();
        textoPickup.gameObject.SetActive(mirando);
    }
}