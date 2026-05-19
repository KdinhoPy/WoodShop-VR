using UnityEngine;

/// <summary>
/// PickupSetup v2 - Configura objeto como pegável SEM CAIR no chão
/// 
/// VERSÃO ATUALIZADA: deixa o objeto em modo "kinematic" inicialmente
/// (parado no ar/lugar), só ativando física quando é solto.
/// 
/// Como usa:
///   1. Adiciona este script no objeto que quer que seja pegável
///   2. Ao iniciar o jogo, o objeto fica parado no lugar
///   3. Quando o player pega (tecla E), ele segue a câmera
///   4. Quando o player solta, ele cai normalmente (física ativa)
/// 
/// Autor: Ricardo - Projeto WoodShop-VR
/// </summary>
public class PickupSetup : MonoBehaviour
{
    [Header("Configurações Físicas (após soltar)")]
    [Tooltip("Massa do objeto em kg")]
    public float mass = 1f;

    [Tooltip("Resistência ao movimento (0 = nenhuma, 5 = muita)")]
    public float drag = 0.5f;

    [Header("Comportamento Inicial")]
    [Tooltip("Se marcado, objeto fica parado no ar até ser pego (não cai)")]
    public bool startStatic = true;

    void Start()
    {
        SetupAsPickup();
    }

    /// <summary>
    /// Configura todos os componentes necessários para ser pegável
    /// </summary>
    void SetupAsPickup()
    {
        // 1. Define a tag como "Pickup"
        try
        {
            gameObject.tag = "Pickup";
        }
        catch (UnityException)
        {
            Debug.LogWarning("Tag 'Pickup' não existe! Crie em Edit > Project Settings > Tags and Layers");
        }

        // 2. Garante que tem um Collider
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            // Tenta adicionar MeshCollider (precisa ser Convex pra ter Rigidbody)
            MeshFilter mf = GetComponent<MeshFilter>();
            if (mf != null && mf.sharedMesh != null)
            {
                MeshCollider meshCol = gameObject.AddComponent<MeshCollider>();
                meshCol.convex = true;
                Debug.Log("MeshCollider (convex) adicionado em: " + gameObject.name);
            }
            else
            {
                // Se não tem mesh próprio, adiciona BoxCollider como fallback
                gameObject.AddComponent<BoxCollider>();
                Debug.Log("BoxCollider adicionado em: " + gameObject.name);
            }
        }

        // 3. Garante que tem um Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            Debug.Log("Rigidbody adicionado em: " + gameObject.name);
        }

        // 4. Aplica configurações
        rb.mass = mass;
        rb.linearDamping = drag;
        rb.useGravity = !startStatic;       // Sem gravidade se for estático
        rb.isKinematic = startStatic;       // Kinematic = não é afetado por física
    }
}
