using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.InputModule;

public class Spawning_manager : MonoBehaviour, IInputClickHandler
{
    public GameObject AvatarPrefab;
    [HideInInspector]
    public GameObject avatar;
    private Vector3 temp, avatarBasePosition, ten_meters_from_Device;
    [HideInInspector]
    public int location = 0;
    [HideInInspector]
    public bool spawner_configuration_done = false;
    private bool placing = true;
    private Avatar_animation_manager animation_Manager;

    // Start is called before the first frame update
    void Start()
    {
        ten_meters_from_Device = new Vector3(0.0f, 0.0f, 10.0f);
        InputManager.Instance.PushFallbackInputHandler(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (placing)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
                30.0f, SpatialMappingManager.Instance.LayerMask))
            {
                // Move this object's parent object to
                // where the raycast hit the Spatial Mapping mesh.
                this.gameObject.transform.position = hitInfo.point;

                // Rotate this object's parent object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;
                this.gameObject.transform.rotation = toQuat;
            }
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        placing = false;

        if (spawner_configuration_done == false)
        {
            avatar = Instantiate(AvatarPrefab, this.gameObject.transform.position, Quaternion.identity);
            temp = ten_meters_from_Device + new Vector3(-this.gameObject.transform.position.x, 0.0f, -this.gameObject.transform.position.z);
            avatar.transform.position += temp;
            avatarBasePosition = avatar.transform.position;
            avatar.AddComponent<Avatar_animation_manager>();
            animation_Manager = avatar.GetComponent<Avatar_animation_manager>();
            avatar.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            spawner_configuration_done = true;
            Destroy(SpatialMappingManager.Instance.gameObject);
        }
        else
        {
            if (location == 0)
            {
                int[] choices = new int[] { 1, 2 };
                Next_Location_Calculation(choices);
            }
            else if (location == 1)
            {
                int[] choices = new int[] { 0, 2 };
                Next_Location_Calculation(choices);
            }
            else if (location == 2)
            {
                int[] choices = new int[] { 0, 1 };
                Next_Location_Calculation(choices);
            }
        }
        // throw new System.NotImplementedException();
    }

    void Next_Location_Calculation(int[] choices_array)
    {
        int[] choices = choices_array;
        int howManyChoices = choices.Length;
        int RandomIndex = Random.Range(0, howManyChoices);
        int next_location = choices[RandomIndex];
        location = next_location;
        animation_Manager.animator.SetBool("IsWalking", false);
        animation_Manager.configuration_done = false;

        avatar.transform.position = avatarBasePosition;
        if (location == 1)
            avatar.transform.position = avatarBasePosition + new Vector3(0.0f, 0.0f, 15.0f);
        else if (location == 2)
            avatar.transform.position = avatarBasePosition + new Vector3(0.0f, 0.0f, 30.0f);
        else if (location == 0)
            avatar.transform.position = avatarBasePosition;
    }
}
