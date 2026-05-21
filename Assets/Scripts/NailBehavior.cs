using UnityEngine;

/// <summary>
/// NailBehavior - Comportamento do prego ao ser atingido pelo martelo
/// 
/// Como funciona:
///   - A cada frame verifica se o player está segurando o martelo
///   - Se o martelo estiver a menos de 0.5 unidades do prego, prega
///   - Ao pregar: desativa física, remove tag Pickup e desativa highlight
/// 
/// Requisitos:
///   - Martelo deve ter tag "Hammer"
///   - PlayerControllerPC deve estar na cena
/// 
/// Autor: Ricardo Augusto - WoodShop-VR
/// </summary>
public class NailBehavior : MonoBehaviour
{
    // Controla se o prego já foi pregado
    private bool pregado = false;

    // Rigidbody do prego
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Se já pregado, não faz nada
        if (pregado) return;

        // Busca o player na cena
        PlayerControllerPC player = FindObjectOfType<PlayerControllerPC>();
        if (player == null) return;

        // Verifica o objeto que o player está segurando
        GameObject objetoSegurando = player.GetHeldObject();
        if (objetoSegurando == null) return;

        // Só continua se for o martelo
        if (!objetoSegurando.CompareTag("Hammer")) return;

        // Calcula distância entre martelo e prego
        float distancia = Vector3.Distance(objetoSegurando.transform.position, transform.position);

        // Se estiver perto o suficiente, prega
        if (distancia < 0.5f)
            Pregar();
    }

    /// <summary>
    /// Prega o prego na superfície:
    /// - Desativa física (fica parado)
    /// - Remove tag Pickup (não pode mais ser pegado)
    /// - Desativa highlight
    /// </summary>
    void Pregar()
    {
        pregado = true;

        // Remove possibilidade de pegar
        gameObject.tag = "Untagged";

        // Cola no lugar — desativa física
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Desativa highlight
        HighlightOnLook h = GetComponent<HighlightOnLook>();
        if (h != null) h.enabled = false;

        Debug.Log(gameObject.name + " foi pregado!");
    }
}