Read Me

# **Proyecto RTS — Patrones de Diseño en Unity**

## **Integrantes del grupo:** 
- Andy Rodríguez Herrera
- Sidney Rodríguez Carranza
- José Daniel Valerio Céspedes

## **Resumen de juego**
- El jugador y el enemigo cuentan con MainBase. Si tu base se destruye, pierdes; si destruyes la del enemigo, ganas.
- MainBase: produce Miner.
- GoldMine: contiene oro; los Miner extraen y depositan para ampliar la economía.
- MilitaryBuilding produce unidades de combate: melee, range, healer y tank (si hay oro suficiente).
- Las unidades atacan usando estrategias de combate (cuerpo a cuerpo, a distancia o curación).

## **Documentación de Patrones utilizados**
**Creacionales**
**1. Patrón Singleton**
Descripción general
El patrón Singleton garantiza que una clase tenga una única instancia global y proporcione un punto de acceso global a ella.
Aplicación en el proyecto
Se utiliza en UnitSelectorManager para gestionar:


Lista de todas las unidades en el juego.


Lista de unidades actualmente seleccionadas.


Funciones globales como selección múltiple o deselección.

Se usa en ResourceManager.cs para acceder a una única instancia de ResourceManager desde cualquier parte del juego mediante ResourceManager.Instance.
Beneficios
Evita tener múltiples instancias de un administrador central.


Permite acceder a la funcionalidad desde cualquier script con UnitSelectorManager.Instance.


Código relacionado
UnitSelectorManager.cs
ResourceManager.cs

**2. Patrón Factory Method**
Descripción general
El patrón Factory Method permite delegar la creación de objetos a una clase especializada (la “fábrica”), en lugar de instanciar directamente desde donde se necesita. Esto facilita extender y modificar la lógica de creación sin afectar al resto del sistema.
Aplicación en el proyecto
Se implementó para encapsular la creación de las distintas estructuras del juego (MainBase, ResourceRecolector, MilitaryBuilding), cada una con comportamiento propio.
Cuando el jugador elige construir un edificio, se llama a la fábrica:
StructureFactory.CreateStructure(ObjectData, posición)


Esta fábrica:
Instancia el prefab indicado en el ObjectData.
Activa el componente Constructable (para activar el obstáculo de navegación).
Verifica si el prefab implementa IStructure.
Si lo hace, llama a structure.Activate() para ejecutar la lógica de ese edificio.
Esto permite que cada edificio tenga su lógica interna encapsulada y que todo el sistema de construcción trabaje de forma genérica.



Beneficios
Centraliza la lógica de construcción: todo se hace en un solo lugar.


Permite agregar nuevos tipos de estructuras sin modificar otros sistemas.


Se combina con la interfaz IStructure para activar automáticamente la lógica interna del edificio (como producir mineros o entrenar soldados).


Reduce el acoplamiento entre UI, placement y lógica interna de los edificios.

Código relacionado
StructureFactory.cs


IStructure.cs


MainBase.cs


MilitaryBuilding.cs


ResourceRecolector.cs


ObjectPlacer.cs

**3. Patrón Prototype**
Descripción general
El Prototype permite crear nuevos objetos clonando instancias preconfiguradas (prototipos), en vez de construirlos desde cero. Así se evita lógica de construcción repetida y se facilita variar configuraciones por tipo.
Aplicación en el proyecto
Se usa para producir unidades desde MilitaryBuilding clonando prototipos (UnitPrototype como ScriptableObject) que ya conocen:
Su prefab base.
Sus requisitos de recursos (List<BuildRequirement>).


UnitFactory recibe los prototipos del edificio y clona el que se pida (p. ej., “Melee”), instanciándolo en el SpawnPointMilitary.

Componentes
UnitPrototype.cs (ScriptableObject)
 Define unitType, prefab, requirements y el método Clone(Transform spawnPoint) que instancia el prefab en la escena.


UnitFactory.cs
 Guarda un mapa unitType → UnitPrototype (vía Initialize(...)).
 Expone GetRequirements(string) y CreateUnit(string); esta última clona el prototipo correspondiente.


MilitaryBuilding.cs
 En Awake() llama a unitFactory.Initialize(melee, range, healer, tank, spawnPoint) pasando sus prototipos y su spawn propio (hijo del prefab).


ResourceManager.cs (colabora)
 Verifica y consume los BuildRequirement antes de permitir la clonación.


MilitaryBuildingUI.cs (cliente de la fábrica)
 Al presionar un botón, consulta requisitos a la fábrica, pide al ResourceManager validar/consumir y finalmente invoca CreateUnit(unitType) (clonado).

Beneficios
Desacopla la creación de unidades de la UI y de la estructura: la lógica vive en los prototipos y la fábrica.


Consistencia: toda unidad del mismo tipo nace con la misma configuración del prototipo.


Extensibilidad: agregar un nuevo tipo de unidad es crear otro UnitPrototype y registrarlo en UnitFactory.Initialize(...) (sin tocar la UI ni el flujo de recursos).


Mantenimiento simple: cambios de balance (costos/prefabs) se hacen en el prototipo, no en el código.

Código relacionado
UnitPrototype.cs


UnitFactory.cs


MilitaryBuilding.cs


BuildRequirement.cs


ResourceManager.cs


MilitaryBuildingUI.cs (invoca a la fábrica; no crea unidades directamente)


**Estructurales**
**4. Patrón Facade**
Descripción general
El patrón Facade proporciona una interfaz unificada y simplificada para acceder a un subsistema complejo. En este caso, cada panel de estructura expone la misma interfaz para mostrarse/ocultarse.
Aplicación en el proyecto
Se usa para centralizar la activación de paneles de UI de estructuras. StructureUIManager decide qué panel mostrar y llama métodos uniformes en sus facades.
Componentes
IStructureUIFacade.cs → Interfaz fachada: ShowPanel(IStructure structure), HidePanel().


MainBaseUI.cs → Facade que muestra el panel de MainBase.


MilitaryBuildingUI.cs → Facade que muestra el panel de MilitaryBuilding.


StructureUIManager.cs → Punto de acceso único: recibe la selección (vía Observer) y llama a la facade correspondiente.

Beneficios
Centraliza y unifica el acceso a los paneles de UI.


Escalable: añadir otra estructura = nueva facade + entrada en el mapeo del manager.


Compatibilidad con Observer: el manager reacciona a la selección y muestra la facade correcta.

Código relacionado
IStructureUIFacade.cs


StructureUIManager.cs


MainBaseUI.cs


MilitaryBuildingUI.cs



**Comportamiento**
**5. Patrón Strategy**
Descripción general
El patrón Strategy define una familia de algoritmos, los encapsula y hace que sean intercambiables.
 El cliente (en este caso, las unidades) no necesita conocer la implementación concreta del algoritmo de ataque.
Aplicación en el proyecto
Cada unidad tiene un AttackController que usa una estrategia de ataque:


MeleeAttackStrategy → Ataque cuerpo a cuerpo con rango corto.


RangedAttackStrategy → Ataque a distancia con un rango mayor.


El tipo de ataque se asigna al inicio dependiendo de si la unidad es melee o rango.



Beneficios
Permite agregar nuevos tipos de ataques sin modificar el código principal de las unidades.


Facilita la configuración por prefab (cada unidad puede tener un rango y comportamiento de ataque diferente).


Reduce el acoplamiento entre la lógica de ataque y la lógica de movimiento/animación.


Código relacionado
IAttackStrategy.cs


MeleeAttackStrategy.cs


RangedAttackStrategy.cs


AttackController.cs

**6. Patrón State**
Descripción general
El patrón State permite que un objeto altere su comportamiento cuando cambia su estado interno.
 El objeto parece cambiar su clase porque cada estado se maneja mediante una clase diferente que encapsula su comportamiento.
Aplicación en el proyecto
Se implementó usando el sistema de Animator de Unity junto con scripts que heredan de StateMachineBehaviour.


Cada comportamiento de la unidad se encapsula en un estado:

UnitIdleState → Estado de reposo.


UnitFollowState → Estado de seguimiento al objetivo.


UnitAttackState → Estado de ataque.

Implementación en miner.cs:  
Interfaz IMinerState que define los métodos Enter(), Update() y Exit()
Estados concretos: MinerIdleState y MinerMiningState
Clase Miner que mantiene el estado actual y permite cambiarlo con ChangeState()
Cada estado encapsula un comportamiento específico:
MinerIdleState: Comportamiento cuando está buscando minas
MinerMiningState: Comportamiento cuando está extrayendo oro

Beneficios
Se evita tener una clase con múltiples condicionales (if/else o switch) para controlar cada comportamiento.
Permite agregar nuevos estados sin modificar los existentes.
Se integra naturalmente con el sistema de animaciones de Unity.
Facilita la adición de nuevos estados 
Encapsula la lógica de cada comportamiento de manera independiente

Código relacionado
UnitIdleState.cs


UnitFollowState.cs


UnitAttackState.cs

Miner.cs

**7. Patrón Observer**
Descripción general
El patrón Observer permite que un objeto (sujeto) notifique automáticamente a otros objetos (observadores) cuando su estado cambia, sin que exista un acoplamiento fuerte entre ellos.
Aplicación en el proyecto
Se implementó para notificar la selección de estructuras a los paneles de UI correspondientes.
 Cuando el jugador selecciona una estructura (ej: MainBase) con S + clic izquierdo, se notifica a todos los observadores registrados.
Componentes involucrados:
StructureSelectionNotifier.cs → Sujeto (Subject).
 Guarda una lista de observadores y los notifica cuando una estructura es seleccionada.


StructureUIManager.cs → Observador que gestiona la lógica para mostrar el panel correspondiente a la estructura seleccionada.


MainBaseUI.cs → Observador que muestra el panel de mineros si la estructura seleccionada es una MainBase.


Esto permite que múltiples sistemas se suscriban a los eventos de selección sin que el selector conozca sus detalles.
Implementación en GoldMine.cs: 
Interfaces IGoldMineObserver y IGoldMineSubject
GoldMine implementa IGoldMineSubject con métodos AddObserver(), RemoveObserver() y NotifyObservers()
Miner implementa IGoldMineObserver con método OnGoldChanged()
Notificación automática cuando cambia el oro en la mina.

Beneficios
Desacopla la lógica de selección de la lógica de UI.


Permite múltiples observadores (más paneles de UI en el futuro).


Facilita pruebas y mantenimiento, ya que los módulos están desacoplados.
Permite a los mineros reaccionar a cambios en las minas (como agotamiento de recursos)
Facilita la adición de nuevos observadores (como UI que muestre estado de minas)

Código relacionado
StructureSelectionNotifier.cs


StructureUIManager.cs


MainBaseUI.cs


StructureSelector.cs


IStructureSelectionObserver.cs
GoldMine.cs

**8. Patrón Command**
Descripción general
Encapsula una solicitud como un objeto, permitiendo parametrizar clientes con diferentes solicitudes y soportar operaciones reversibles, busca además cumplir con separación adecuada de responsabilidades y se pueden parametrizar de forma más efectiva estas solicitudes
Aplicación en el proyecto
En nuestro proyecto ResourceManager,  convierte cada operación económica en un objeto independiente que puede ejecutarse y deshacerse. Esto es importante para plantear futuras implementaciones de operaciones de salvado de progreso del juego y tambien es vital para manejar múltiples clases que dependen de la cantidad de oro para hacer construcciones , unidades o generar reservas de oro por parte de los mineros. Además gestiona la misma logica del lado del bando enemigo

Componentes:

AgregarOroJugador:  
Encapsula la operación de añadir oro al jugador

Mantiene referencias al ResourceManager y la cantidad de oro
Ejecutar() llama a AgregarOroJugadorDirecto()
Deshacer() llama a GastarOroJugadorDirecto()

GastarOroJugador:  
Encapsula la operación de gastar oro del jugador
Ejecutar() verifica si hay suficientes recursos

AgregarOroEnemigo:  
Encapsula la operación de añadir oro al enemigo
Similar a AgregarOroJugador pero para recursos enemigos

La clase HistorialRecursos mantiene un registro de los comandos ejecutados:  
Usa una Stack<IResourceCommand> para almacenar el historial
Método Ejecutar() que ejecuta un comando y lo guarda si fue exitoso
Método Deshacer() que recupera el último comando ejecutado


Beneficios

Operaciones reversibles:  Permite deshacer transacciones económicas (crítico para juegos de estrategia)
El método UndoLastOperation() puede revertir errores tácticos
Abstracción y encapsulamiento:  Separa la solicitud (añadir/gastar oro) de su implementación,cada operación económica tiene su propia clase
Mantenibilidad mejorada:  Facilita añadir nuevas operaciones económicas (ej: transferir recursos, aplicar impuestos,aplicar mejoras a edificios)
Las operaciones complejas pueden componerse de comandos simples
Registro de transacciones:  El historial de comandos sirve como registro de auditoría, util para debugging y análisis del balance económico
Flexibilidad en la interfaz:  Los métodos públicos como AddPlayerGold() ocultan la complejidad
Permite cambiar la implementación interna sin afectar las clases cliente
Interoperabilidad con otros sistemas: La clase RegisterGoldExtraction() demuestra cómo el sistema interactúa con mineros
Los comandos pueden responder a eventos externos (como agotamiento de minas)

Código relacionado
ResourceManager.cs



## **División de responsabilidades a la hora de programar**
**Sistema económico:**
Recolecta un recurso (oro): Andy
Usa ese recurso para construir estructuras o entrenar unidades: Andy y Sidney
**Estructuras:**
Base principal (produce unidades o recolectores): Sidney
Recolector de recursos: Andy
Edificio militar (entrena unidades): Sidney
**Unidades**
Unidades melee, ranged, tanque, soporte: José
Cada unidad con comportamientos específicos: José
**Combate en tiempo real:**
Unidades se mueven, detectan enemigos y atacan automáticamente: José
**Control de jugador:**
Selección de múltiples unidades: José
Órdenes: mover, patrullar, atacar, construir: José
**IA enemiga**
Se expande, produce unidades y ataca en oleadas crecientes: Andy y José
Detecta amenazas y responde con ataques o defensas: Andy y José



## Cómo jugar (controles)
Cámara
Mover el mouse a los bordes de la pantalla.


Selección de unidades
Seleccionar una unidad: click izquierdo sobre la unidad.
Selección múltiple: click izquierdo y arrastrar para dibujar un rectángulo.
Limpiar selección: Esc.


Órdenes a unidades
Mover / Interactuar: click derecho sobre el terreno (mover).
Atacar: click derecho sobre una unidad/estructura enemiga.
Miner: con el minero seleccionado, click derecho sobre una GoldMine para comenzar a extraer (el depósito en la MainBase es automático cuando está lleno).
Healer: seleccionar al healer con click izquierdo, colocar el mouse encima y presionar la tecla Q para curar.


Estructuras y producción
Abrir menú de estructuras: botón de UI “Structures” que tiene el símbolo “+”
Construir MilitaryBuilding: en el panel, elige MilitaryBuilding, coloca en el grid con click izquierdo (requiere oro).
Seleccionar una estructura: poner el mouse sobre la estructura y presionar la Tecla S y el click al mismo tiempo.
Producir desde MilitaryBuilding: en su panel pulsa los botones Melee / Range / Healer / Tank (consume oro).
Producir desde MainBase: botón Produce Miner para crear mineros.


Condiciones de victoria/derrota
Ganas al destruir la MainBase enemiga.
Pierdes si destruyen tu MainBase.


Enemigo
El enemigo genera unidades (mediante EnemySpawner) y estas se mueven/atacan automáticamente (EnemyAutoMovement).
También puede minar oro con sus Miner y construir/producir usando los mismos sistemas (usa su propios recursos).
