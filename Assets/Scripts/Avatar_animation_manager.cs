using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar_animation_manager : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    private Vector3 positive_offset = new Vector3(1.0f, 0.0f, 0.0f);
    private Vector3 negative_offset = new Vector3(-1.0f, 0.0f, 0.0f);
    private Vector3 forward_highest_distance_for_avatar, backward_highest_distance_for_avatar;
    private Spawning_manager spawning_Manager;
    private bool avatar_rotated = false;
    [HideInInspector]
    public bool configuration_done = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        spawning_Manager = GameObject.Find("Spawner").GetComponent<Spawning_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (configuration_done == false && GetSpawner_configuration_done() == true)
        {
            forward_highest_distance_for_avatar = this.gameObject.transform.position + positive_offset;
            backward_highest_distance_for_avatar = this.gameObject.transform.position + negative_offset;
            configuration_done = true;
            animator.SetBool("IsWalking", true);
        }

        if ((forward_highest_distance_for_avatar.x - this.gameObject.transform.position.x) > 0.0f && avatar_rotated == false)
        {
            this.gameObject.transform.position += new Vector3(0.025f, 0.0f, 0.0f);
        }
        
        if ((this.gameObject.transform.position.x - backward_highest_distance_for_avatar.x) > 0.0f && avatar_rotated == true)
        {
            this.gameObject.transform.position -= new Vector3(0.025f, 0.0f, 0.0f);
        }

        if (((forward_highest_distance_for_avatar.x - this.gameObject.transform.position.x) <= 0.0f) || ((this.gameObject.transform.position.x - backward_highest_distance_for_avatar.x) <= 0.0f))
        {
            this.gameObject.transform.Rotate(0.0f, 180.0f, 0.0f);
            avatar_rotated = !avatar_rotated;
        }
    }

    private bool GetSpawner_configuration_done()
    {
        return spawning_Manager.spawner_configuration_done;
    }
}
