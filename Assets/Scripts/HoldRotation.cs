using UnityEngine;

/// <summary>
/// HoldRotation - Define a rotação customizada de um objeto ao ser segurado
/// 
/// Como usar:
///   - Adicione este script no objeto pegável
///   - Configure a rotação desejada no Inspector
///   - O PlayerControllerPC vai usar essa rotação ao segurar o objeto
/// 
/// Exemplos usados no WoodShop-VR:
///   - Hammer_01: X -45, Y 90, Z 0 (martelo apontando para frente)
///   - Saw_01:    X 0,   Y 0,  Z 0 (serra na posição natural)
///   - Pregos e parafusos: X 0, Y 0, Z 0
/// 
/// Autor: Ricardo Augusto - WoodShop-VR
/// </summary>
public class HoldRotation : MonoBehaviour
{
    [Tooltip("Rotação do objeto enquanto está sendo segurado pelo player")]
    public Vector3 rotation = Vector3.zero;
}