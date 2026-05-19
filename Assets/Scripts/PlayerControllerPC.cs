using UnityEngine;

/// <summary>
/// PlayerControllerPC v2 - Movimentação First Person com FÍSICA (colide com paredes)
/// 
/// VERSÃO ATUALIZADA: usa Rigidbody.linearVelocity ao invés de Transform.position,
/// permitindo que o player colida com paredes, chão, e outros objetos sólidos.
/// 
/// Requisitos no GameObject (já configurado pelo Ricardo):
///   - Capsule Collider (Height 1.8, Radius 0.3)
///   - Rigidbody (Mass 1, Use Gravity, Freeze Rotation X/Y/Z)
///   - Camera filho (Position 0, 1.7, 0 relativa - altura dos olhos)
/// 
/// Controles:
///   - WASD: andar
///   - Mouse (botão direito + arrastar): olhar ao redor
///   - Shift: correr
///   - Espaço: pular
///   - E: pegar/soltar objeto
/// 
/// Autor: Ricardo - Projeto WoodShop-VR
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerControllerPC : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Velocidade base de andar")]
    public float moveSpeed = 5f;

    [Tooltip("Multiplicador ao segurar Shift")]
    public float runMultiplier = 2f;

    [Tooltip("Força do pulo")]
    public float jumpForce = 5f;

    [Tooltip("Sensibilidade do mouse")]
    public float mouseSensitivity = 2f;

    [Header("Referências")]
    [Tooltip("Camera filho - arraste no Inspector")]
    public Transform playerCamera;

    [Header("Interação")]
    [Tooltip("Distância máxima para pegar objetos")]
    public float pickupDistance = 3f;

    [Tooltip("Distância em que o objeto fica enquanto carregado")]
    public float holdDistance = 1.5f;

    // Variáveis internas
    private Rigidbody rb;
    private float rotationX = 0f;
    private GameObject heldObject = null;
    private Rigidbody heldObjectRb = null;
    private bool isGrounded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Garante que o Rigidbody está configurado certo
        rb.freezeRotation = true;
        rb.useGravity = true;
    }

    void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogError("PlayerControllerPC: Player Camera não foi atribuída!");
            enabled = false;
            return;
        }

        Debug.Log("PlayerControllerPC v2 ativo (com física). WASD para andar, mouse direito para olhar, E para pegar, Espaço para pular.");
    }

    void Update()
    {
        HandleLook();
        HandlePickupInput();
        HandleJump();
        UpdateHeldObject();
    }

    void FixedUpdate()
    {
        HandleMovement();
        CheckGrounded();
    }

    /// <summary>
    /// Movimento com física - usa velocity ao invés de transform.position
    /// </summary>
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calcula velocidade desejada baseada na orientação do player
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;
        Vector3 desiredVelocity = (forward + right).normalized;

        // Aplica velocidade (mantém Y atual pra gravidade funcionar)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * runMultiplier : moveSpeed;
        Vector3 velocity = desiredVelocity * currentSpeed;
        velocity.y = rb.linearVelocity.y; // preserva velocidade vertical (gravidade/pulo)

        rb.linearVelocity = velocity;
    }

    /// <summary>
    /// Pulo - só funciona se estiver no chão
    /// </summary>
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    /// <summary>
    /// Detecta se o player está no chão usando raycast curto pra baixo
    /// </summary>
    void CheckGrounded()
    {
        // Raycast 1.1 unidades pra baixo a partir do centro
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    /// <summary>
    /// Rotação com mouse - segurar botão direito
    /// </summary>
    void HandleLook()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Rotação horizontal (corpo inteiro)
            transform.Rotate(Vector3.up * mouseX);

            // Rotação vertical (só camera)
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);
            playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }
    }

    /// <summary>
    /// Detecta tecla E pra pegar/soltar
    /// </summary>
    void HandlePickupInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                Drop();
            }
        }
    }

    /// <summary>
    /// Mantém o objeto carregado na frente do player
    /// </summary>
    void UpdateHeldObject()
    {
        if (heldObject != null)
        {
            Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, targetPos, 15f * Time.deltaTime);
            heldObject.transform.rotation = Quaternion.Lerp(heldObject.transform.rotation, playerCamera.rotation, 8f * Time.deltaTime);
        }
    }

    /// <summary>
    /// Tenta pegar objeto na frente
    /// </summary>
    void TryPickup()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRb != null)
                {
                    heldObjectRb.isKinematic = true;
                    heldObjectRb.useGravity = false;
                }

                Debug.Log("Pegou: " + heldObject.name);
            }
            else
            {
                Debug.Log("Objeto sem tag 'Pickup': " + hit.collider.name);
            }
        }
    }

    /// <summary>
    /// Solta o objeto
    /// </summary>
    void Drop()
    {
        if (heldObject != null)
        {
            if (heldObjectRb != null)
            {
                heldObjectRb.isKinematic = false;
                heldObjectRb.useGravity = true;
            }

            Debug.Log("Soltou: " + heldObject.name);
            heldObject = null;
            heldObjectRb = null;
        }
    }

    /// <summary>
    /// Visualiza o raio de pickup no Editor
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerCamera.position, playerCamera.position + playerCamera.forward * pickupDistance);
        }
    }
}
