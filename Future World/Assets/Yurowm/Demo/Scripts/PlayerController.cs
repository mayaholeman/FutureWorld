using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{

    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;

    private Actions actions;
    private Animator animator;

    void Awake()
    {
        actions = GetComponent<Actions>();
        animator = GetComponent<Animator>();
        if (arsenal.Length > 0)
            SetArsenal(arsenal[0].name);
    }

    public void SetArsenal(string name)
    {
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == name)
            {
                if (rightGunBone.childCount > 0)
                    Destroy(rightGunBone.GetChild(0).gameObject);
                if (leftGunBone.childCount > 0)
                    Destroy(leftGunBone.GetChild(0).gameObject);
                if (hand.rightGun != null)
                {
                    GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
                    newRightGun.transform.parent = rightGunBone;
                    newRightGun.transform.localPosition = Vector3.zero;
                    newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                if (hand.leftGun != null)
                {
                    GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                    newLeftGun.transform.parent = leftGunBone;
                    newLeftGun.transform.localPosition = Vector3.zero;
                    newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                animator.runtimeAnimatorController = hand.controller;
                return;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            actions.Walk();
        }
        else
        {
            actions.Stay();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            actions.Sitting();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            actions.GetUp();
        }
    }


    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
    }
}
