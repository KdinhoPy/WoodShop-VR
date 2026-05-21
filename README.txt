# WoodShop-VR

Marcenaria Virtual para Treino de Coordenação Motora
Web 3.0 | Residência em TIC 29 — Unidade 1, Capítulo 3

---

NOME COMPLETO DO ALUNO

Ricardo Augusto

---

APRESENTANDO O PROJETO

O WoodShop-VR é uma experiência em Realidade Virtual desenvolvida na
Unity 6.4 com suporte ao Meta Quest via OpenXR, simulando uma marcenaria
interativa para treino de coordenação motora.

O ambiente é composto por uma marcenaria rústica dentro de uma casa de
madeira, contendo bancadas, ferramentas e materiais de trabalho.

O usuário pode:
- Pegar ferramentas com a tecla E (martelo, machado, serra, serrote)
- Ver o highlight amarelo ao mirar em objetos pegáveis
- Ver a UI "Pressione E para pegar" ao mirar em objetos
- Manipular materiais (toras, tábuas, pregos, parafusos)
- Pregar pregos aproximando o martelo deles

Mais de 30 objetos 3D na cena, organizados em 7 categorias interativas.

---

CONTEXTO E OBJETIVOS

O WoodShop-VR foi criado para resolver um problema real: a falta de
espaços seguros para praticar manipulação de ferramentas.

No contexto do Metaverso, o projeto tem três aplicações:

- Terapia Ocupacional: reabilitação pós-AVC com exercícios de
  coordenação motora fina e grossa
- Educação Técnica: prática de manuseio de ferramentas em cursos
  de marcenaria
- Lazer: experiência imersiva de marcenaria sem risco de acidentes

Em VR, o usuário pratica gestos reais em ambiente seguro, sem risco
de acidentes ou desperdício de material.

---

PROCESSO DE CRIAÇÃO E DIFICULDADES

Como desenvolvi o projeto:

1. Setup do ambiente — Unity Hub, Unity 6.4, Git e GitHub Desktop
2. Configuração XR — OpenXR + Meta Quest feature group
3. Build Android — ARM64, IL2CPP, Vulkan e API 29 para Meta Quest
4. Montagem da cena — assets do Poly Pizza e Sketchfab (Creative Commons)
5. Scripts de interação — 6 scripts C# customizados desenvolvidos:
   - PlayerControllerPC.cs: movimentação, câmera e sistema de pickup
   - HighlightOnLook.cs: destaque amarelo ao mirar em objetos
   - HoldRotation.cs: rotação customizada por ferramenta
   - PickupSetup.cs: configuração automática de objetos pegáveis
   - NailBehavior.cs: sistema de pregar com martelo
   - UIPickup.cs: UI "Pressione E para pegar"

Maiores desafios e soluções:

PROBLEMA: Player atravessava paredes
SOLUÇÃO: Reescrevi o script para usar Rigidbody.linearVelocity
em vez de Transform.position, permitindo colisão física real.

PROBLEMA: Tábuas empurravam o player
SOLUÇÃO: Criei uma Layer Pickup e configurei a Layer Collision Matrix
para ignorar colisão entre objetos pegáveis e o player.

PROBLEMA: Highlight não funcionava em objetos com sub-meshes
SOLUÇÃO: Usei GetComponentsInChildren para capturar todos os
Renderers do objeto e seus filhos.

PROBLEMA: Serra ficava com rotação errada ao segurar
SOLUÇÃO: Criei o script HoldRotation.cs com rotação customizada
por objeto, aplicada apenas ao segurar.

PROBLEMA: Deleção acidental de elementos do template VR
SOLUÇÃO: Aprendi a usar o ícone de olho na Hierarchy para ocultar
em vez de deletar.

---

CONFIGURAÇÃO TÉCNICA

- Unity: 6.4 (6000.4.7f1)
- Pipeline: URP (Universal Render Pipeline)
- XR: OpenXR + Unity OpenXR Meta 2.5.0
- Build: Android / Meta Quest (ARM64, IL2CPP, Vulkan, API 29)
- Hardware: Acer Nitro V15, 16 GB RAM, GPU NVIDIA 8 GB VRAM

---

Projeto desenvolvido para a Residência em TIC 29 — Web 3.0