using UnityEngine;

public class UnderwaterPrototypeBootstrap : MonoBehaviour
{
    [SerializeField] private bool autoBuild = true;

    private void Start()
    {
        if (autoBuild)
        {
            EnsurePrototype();
        }
    }

    private void EnsurePrototype()
    {
        if (FindObjectOfType<GameManager>() != null && FindObjectOfType<PlayerController>() != null)
        {
            return;
        }

        GameObject root = new GameObject("Underwater Prototype Root");
        root.transform.position = Vector3.zero;

        GameObject playerObject = new GameObject("Diver");
        playerObject.transform.SetParent(root.transform, false);
        playerObject.transform.position = new Vector3(0f, 1f, 0f);

        CharacterController characterController = playerObject.AddComponent<CharacterController>();
        characterController.height = 1.8f;
        characterController.radius = 0.35f;
        characterController.center = new Vector3(0f, 0.9f, 0f);

        PlayerController playerController = playerObject.AddComponent<PlayerController>();
        playerObject.AddComponent<AfterimageRecorder>();

        GameObject diverBody = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        diverBody.name = "Diver Body";
        diverBody.transform.SetParent(playerObject.transform, false);
        diverBody.transform.localPosition = new Vector3(0f, 0.9f, 0f);
        diverBody.transform.localScale = new Vector3(0.8f, 0.9f, 0.8f);
        Destroy(diverBody.GetComponent<Collider>());

        GameObject helmet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        helmet.name = "Helmet";
        helmet.transform.SetParent(playerObject.transform, false);
        helmet.transform.localPosition = new Vector3(0f, 1.75f, 0f);
        helmet.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        Destroy(helmet.GetComponent<Collider>());

        GameObject tank = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        tank.name = "Oxygen Tank";
        tank.transform.SetParent(playerObject.transform, false);
        tank.transform.localPosition = new Vector3(-0.5f, 1.1f, 0f);
        tank.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        tank.transform.localScale = new Vector3(0.32f, 0.55f, 0.32f);
        Destroy(tank.GetComponent<Collider>());

        GameObject seaFloor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        seaFloor.name = "Sea Floor";
        seaFloor.transform.SetParent(root.transform, false);
        seaFloor.transform.position = new Vector3(0f, -3f, 0f);
        seaFloor.transform.localScale = new Vector3(30f, 1f, 30f);

        GameObject pillarA = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pillarA.name = "Ruin Pillar A";
        pillarA.transform.SetParent(root.transform, false);
        pillarA.transform.position = new Vector3(-7f, -0.5f, 4f);
        pillarA.transform.localScale = new Vector3(2f, 5f, 2f);

        GameObject pillarB = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pillarB.name = "Ruin Pillar B";
        pillarB.transform.SetParent(root.transform, false);
        pillarB.transform.position = new Vector3(7f, -0.5f, -4f);
        pillarB.transform.localScale = new Vector3(2f, 5f, 2f);

        GameObject pressurePlate = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pressurePlate.name = "Pressure Plate";
        pressurePlate.transform.SetParent(root.transform, false);
        pressurePlate.transform.position = new Vector3(0f, -1.3f, 10f);
        pressurePlate.transform.localScale = new Vector3(2.2f, 0.2f, 2.2f);
        BoxCollider plateCollider = pressurePlate.GetComponent<BoxCollider>();
        plateCollider.isTrigger = true;

        GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.name = "Locked Door";
        door.transform.SetParent(root.transform, false);
        door.transform.position = new Vector3(0f, 1.5f, 17f);
        door.transform.localScale = new Vector3(4f, 4.5f, 0.75f);

        DoorController doorController = door.AddComponent<DoorController>();
        doorController.Open();

        PressurePlate pressurePlateComponent = pressurePlate.AddComponent<PressurePlate>();
        pressurePlateComponent.SetLinkedDoor(doorController);

        GameManager gameManager = root.AddComponent<GameManager>();
        GameObject uiHost = new GameObject("UI Host");
        uiHost.transform.SetParent(root.transform, false);
        uiHost.AddComponent<UIManager>();

        // Keep all references linked for the game manager.
        gameManager.GetType().GetField("playerController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameManager, playerController);
        gameManager.GetType().GetField("afterimageRecorder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameManager, playerObject.GetComponent<AfterimageRecorder>());
        gameManager.GetType().GetField("uiManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameManager, uiHost.GetComponent<UIManager>());
        gameManager.GetType().GetField("doorController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameManager, doorController);
        gameManager.GetType().GetField("pressurePlate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(gameManager, pressurePlateComponent);
    }
}
