using UnityEngine;

/// <summary>
/// PlayerControllerPC v4 - Controle completo do player no PC/Editor
/// 
/// Funcionalidades:
///   - Movimentação WASD com física (colide com paredes)
///   - Mouse direito para olhar ao redor
///   - Shift para correr, Espaço para pular
///   - Tecla E para pegar/soltar objetos com tag "Pickup"
///   - Highlight amarelo ao mirar em objetos pegáveis
///   - Rotação customizada ao segurar (via HoldRotation)
///   - UI "Pressione E" aparece ao mirar em objeto pegável
/// 
/// Autor: Ricardo Augusto - WoodShop-VR
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerControllerPC : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Velocidade base de caminhada")]
    public float moveSpeed = 5f;

    [Tooltip("Multiplicador de velocidade ao segurar Shift")]
    public float runMultiplier = 2f;

    [Tooltip("Força aplicada ao pular")]
    public float jumpForce = 5f;

    [Tooltip("Sensibilidade do mouse ao olhar")]
    public float mouseSensitivity = 2f;

    [Header("Referências")]
    [Tooltip("Transform da câmera filho — arraste no Inspector")]
    public Transform playerCamera;

    [Header("Interação")]
    [Tooltip("Distância máxima para pegar objetos")]
    public float pickupDistance = 3f;

    [Tooltip("Distância em que o objeto fica enquanto carregado")]
    public float holdDistance = 1.5f;

    // Componentes internos
    private Rigidbody rb;
    private float rotationX = 0f;
    private GameObject heldObject = null;
    private Rigidbody heldObjectRb = null;
    private bool isGrounded = false;
    private HighlightOnLook currentHighlight = null;
    private Quaternion heldObjectOriginalRotation;

    /// <summary>Retorna o objeto atualmente segurado pelo player</summary>
    public GameObject GetHeldObject() { return heldObject; }

    /// <summary>Retorna true se o player está mirando em um objeto com tag Pickup</summary>
    public bool EstaMirandoPickup()
    {
        if (playerCamera == null) return false;
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickupDistance))
            return hit.collider.CompareTag("Pickup");
        return false;
    }

    void Awake()
    {
        // Configura o Rigidbody para não tombar
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
    }

    void Start()
    {
        // Verifica se a câmera foi atribuída no Inspector
        if (playerCamera == null)
        {
            Debug.LogError("PlayerControllerPC: Player Camera não foi atribuída!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        HandleLook();        // rotação com mouse
        HandlePickupInput(); // tecla E
        HandleJump();        // tecla Espaço
        UpdateHeldObject();  // mantém objeto na frente
        HandleHighlight();   // destaca objeto na mira
    }

    void FixedUpdate()
    {
        HandleMovement();  // movimento físico
        CheckGrounded();   // verifica se está no chão
    }

    /// <summary>
    /// Destaca o objeto pegável que está na mira do player.
    /// Usa GetComponentInChildren e GetComponentInParent para
    /// funcionar com objetos que têm sub-meshes filhos.
    /// </summary>
    void HandleHighlight()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            // Busca HighlightOnLook no objeto, filhos ou pai
            HighlightOnLook h = hit.collider.GetComponent<HighlightOnLook>();
            if (h == null) h = hit.collider.GetComponentInChildren<HighlightOnLook>();
            if (h == null) h = hit.collider.GetComponentInParent<HighlightOnLook>();

            if (h != null)
            {
                // Desativa highlight anterior se mudou de objeto
                if (currentHighlight != null && currentHighlight != h)
                    currentHighlight.DesativarDestaque();
                currentHighlight = h;
                currentHighlight.AtivarDestaque();
            }
            else
            {
                // Nenhum highlight encontrado — desativa o anterior
                if (currentHighlight != null)
                {
                    currentHighlight.DesativarDestaque();
                    currentHighlight = null;
                }
            }
        }
        else
        {
            // Raycast não acertou nada — desativa highlight
            if (currentHighlight != null)
            {
                currentHighlight.DesativarDestaque();
                currentHighlight = null;
            }
        }
    }

    /// <summary>
    /// Movimentação com física usando Rigidbody.linearVelocity.
    /// Preserva velocidade Y para que gravidade e pulo funcionem.
    /// </summary>
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S

        // Calcula direção baseada na orientação do player
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;
        Vector3 desiredVelocity = (forward + right).normalized;

        // Aplica velocidade (Shift = correr)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * runMultiplier : moveSpeed;
        Vector3 velocity = desiredVelocity * currentSpeed;
        velocity.y = rb.linearVelocity.y; // preserva gravidade/pulo
        rb.linearVelocity = velocity;
    }

    /// <summary>Pulo — só funciona se estiver no chão</summary>
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    /// <summary>Detecta chão com raycast curto para baixo</summary>
    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    /// <summary>
    /// Rotação com mouse — segurar botão direito.
    /// Corpo gira horizontalmente, câmera gira verticalmente (clamp ±90°).
    /// </summary>
    void HandleLook()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            transform.Rotate(Vector3.up * mouseX);
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);
            playerCamera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }
    }

    /// <summary>Detecta tecla E para pegar ou soltar objeto</summary>
    void HandlePickupInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryPickup();
            else
                Drop();
        }
    }

    /// <summary>
    /// Mantém o objeto segurado na frente da câmera.
    /// Usa HoldRotation se disponível, senão mantém rotação original.
    /// </summary>
    void UpdateHeldObject()
    {
        if (heldObject != null)
        {
            // Move suavemente para frente da câmera
            Vector3 targetPos = playerCamera.position + playerCamera.forward * holdDistance;
            heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, targetPos, 15f * Time.deltaTime);

            // Aplica rotação customizada ou original
            HoldRotation holdRot = heldObject.GetComponent<HoldRotation>();
            if (holdRot != null)
                heldObject.transform.rotation = Quaternion.Euler(holdRot.rotation);
            else
                heldObject.transform.rotation = heldObjectOriginalRotation;
        }
    }

    /// <summary>
    /// Tenta pegar objeto na mira via raycast.
    /// Só pega objetos com tag "Pickup".
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
                heldObjectOriginalRotation = heldObject.transform.rotation;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                // Desativa física enquanto carrega
                if (heldObjectRb != null)
                {
                    heldObjectRb.isKinematic = true;
                    heldObjectRb.useGravity = false;
                }
                Debug.Log("Pegou: " + heldObject.name);
            }
        }
    }

    /// <summary>Solta o objeto e reativa a física</summary>
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

    /// <summary>Desenha o raio de pickup no Editor (linha verde)</summary>
    void OnDrawGizmosSelected()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerCamera.position, playerCamera.position + playerCamera.forward * pickupDistance);
        }
    }
}