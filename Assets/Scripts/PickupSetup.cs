using UnityEngine;

/// <summary>
/// PickupSetup - Configura automaticamente um objeto como pegável
/// 
/// O que este script faz ao iniciar:
///   - Adiciona tag "Pickup" ao objeto
///   - Adiciona Box Collider se não tiver
///   - Adiciona Rigidbody com massa e drag configurados
///   - Se startStatic = true, deixa o objeto parado até ser pegado
/// 
/// Usado em: ferramentas, tábuas, toras, pregos e parafusos
/// Autor: Ricardo Augusto - WoodShop-VR
/// </summary>
public class PickupSetup : MonoBehaviour
{
    [Header("Configurações Físicas (após soltar)")]
    [Tooltip("Massa do objeto ao cair")]
    public float Mass = 1f;

    [Tooltip("Resistência do ar — reduz quicar ao cair")]
    public float Drag = 0.5f;

    [Header("Comportamento Inicial")]
    [Tooltip("Se true, objeto fica parado até ser pegado pela primeira vez")]
    public bool startStatic = true;

    void Awake()
    {
        // Garante a tag Pickup
        gameObject.tag = "Pickup";

        // Adiciona Box Collider se não tiver nenhum
        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();

        // Adiciona Rigidbody se não tiver
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        // Configura massa e drag
        rb.mass = Mass;
        rb.linearDamping = Drag;

        // Se startStatic, deixa kinematic até ser pegado
        if (startStatic)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Debug.Log("PickupSetup aplicado em: " + gameObject.name);
    }
}