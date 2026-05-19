 WOODSHOP-VR — README
   Projeto Final: Meu Primeiro Ambiente VR
   Curso Web 3.0 | Residência em TIC 29
________________________________________________

PROJETO: WoodShop-VR
AUTOR:   Ricardo
DATA:    19/05/2026
ENGINE:  Unity 6.4 (6000.4.7f1)
PLATAFORMA ALVO: Meta Quest (Android via OpenXR)
________________________________________________

SOBRE O PROJETO
_______________________________________________

WoodShop-VR e um ambiente imersivo de marcenaria virtual
desenvolvido como ferramenta de treino de coordenacao
motora fina e grossa. O usuario pode explorar uma oficina
de marcenaria construida dentro de uma casa de madeira,
interagindo com ferramentas como martelo, machado, serra
eletrica e serrote.

A proposta tem aplicacoes em terapia ocupacional
(reabilitacao neurologica pos-AVC), educacao tecnica
(seguranca no uso de ferramentas) e lazer.

________________________________________________

REQUISITOS PARA ABRIR O PROJETO
________________________________________________

Unity Hub instalado
- Unity Editor 6.4 (6000.4.7f1) ou superior
- Modulos: Android Build Support (com OpenJDK e SDK/NDK)
- Sistema operacional: Windows 10/11
- RAM minima recomendada: 8 GB (16 GB ideal)
________________________________________________

COMO ABRIR
________________________________________________

1. Abrir o Unity Hub
2. Clicar em "Open" -> "Add project from disk"
3. Selecionar a pasta WoodShop-VR
4. Aguardar importacao inicial (pode demorar 3-5 minutos)
5. Abrir a cena: Assets > Scenes > SampleScene

________________________________________________

CONTROLES — MODO PC (Editor)
________________________________________________

W / A / S / D ........ Andar (frente, esquerda, tras, direita)
SHIFT ................ Correr (velocidade dobrada)
ESPACO ............... Pular
BOTAO DIREITO MOUSE .. Olhar ao redor (segura e arrasta)
E .................... Pegar / soltar objeto

________________________________________________

OBJETOS PEGAVEIS:
________________________________________________

- Martelo (Hammer_01)
- Machado (Hatchet)
- Serrote (Saw_01)
- Tábuas Unidas (WoodPlanks)
- Tábuas Negras(DarkPlank)
- Toras (Log)

________________________________________________

CONTROLES — MODO VR (Meta Quest)
________________________________________________

Utiliza os controles padrao do template VR oficial da
Unity (XR Interaction Toolkit + XR Hands). Compativel
com Meta Quest 2, 3 e Pro.
________________________________________________

ESTRUTURA DO PROJETO
________________________________________________
Assets/
  Marcenaria/   .... Modelos 3D importados (.glb)
  Scripts/      .... PlayerControllerPC.cs, PickupSetup.cs
  Scenes/       .... SampleScene.unity
  Settings/     .... Configuracoes URP
  XR/, XRI/     .... Pacotes OpenXR e XR Interaction Toolkit
  TextMesh Pro/ .... Fontes e UI

ProjectSettings/    .... Player Settings, XR, Tags
Packages/           .... manifest.json

________________________________________________
SCRIPTS CUSTOMIZADOS
________________________________________________
PlayerControllerPC.cs
  Controla o player no modo PC/Editor. Usa Rigidbody
  para fisica (colide com paredes/chao). Inclui sistema
  de pickup com raycast.

PickupSetup.cs
  Configura automaticamente qualquer objeto como pegavel
  (tag Pickup + Collider + Rigidbody). Modo startStatic
  mantem objetos imoveis ate serem pegados.

________________________________________________
PACOTES XR INSTALADOS
________________________________________________
- OpenXR Plugin (1.16.1)
- Unity OpenXR Meta (2.5.0)
- XR Interaction Toolkit (3.4.1)
- XR Hands (1.7.3)
- XR Plugin Management (4.5.4)

________________________________________________
CONFIGURACAO DE BUILD (Meta Quest)
________________________________________________
- Plataforma: Android
- Package Name: com.ricardo.woodshopvr
- Min API Level: Android 10 (API 29)
- Scripting Backend: IL2CPP
- Target Architecture: ARM64
- Graphics API: Vulkan
- Texture Compression: ASTC

________________________________________________
ASSETS — CREDITOS
________________________________________________
Modelos 3D obtidos em plataformas gratuitas com licenca
Creative Commons:
- Poly Pizza (poly.pizza) — varios autores
- Sketchfab (sketchfab.com) — varios autores

________________________________________________
CONTATO
________________________________________________
Aluno: Ricardo
GitHub: KdinhoPy
Curso: Web 3.0 — Residencia em TIC 29