using UnityEngine;

/// <summary>
/// HighlightOnLook - Destaca o objeto quando o player mira nele
/// 
/// Como funciona:
///   - Salva as cores originais de todos os Renderers do objeto e filhos
///   - Quando AtivarDestaque() é chamado, muda todas as cores para amarelo
///   - Quando DesativarDestaque() é chamado, restaura as cores originais
/// 
/// Usado pelo PlayerControllerPC via raycast a cada frame.
/// Autor: Ricardo Augusto - WoodShop-VR
/// </summary>
public class HighlightOnLook : MonoBehaviour
{
    [Tooltip("Cor que aparece quando o player mira no objeto")]
    public Color corDestaque = Color.yellow;

    // Todos os Renderers do objeto e seus filhos
    private Renderer[] renderers;

    // Cores originais salvas no Start
    private Color[] coresOriginais;

    void Start()
    {
        // Pega todos os Renderers do objeto E dos filhos
        renderers = GetComponentsInChildren<Renderer>();
        coresOriginais = new Color[renderers.Length];

        // Salva cada cor original
        for (int i = 0; i < renderers.Length; i++)
            coresOriginais[i] = renderers[i].material.color;
    }

    /// <summary>Ativa o highlight — muda cor para amarelo</summary>
    public void AtivarDestaque()
    {
        foreach (var r in renderers)
            r.material.color = corDestaque;
    }

    /// <summary>Desativa o highlight — restaura cor original</summary>
    public void DesativarDestaque()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = coresOriginais[i];
    }
}