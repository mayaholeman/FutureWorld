 using UnityEngine;
 
 public class Walls : MonoBehaviour {
 
     public Material _original;
     public Material _transparent;
     private GameObject player;
     private PlayerController pC;
     private Renderer rend;
 
     void Start()
     {
         player = GameObject.FindWithTag("Player");
         pC = player.GetComponent<PlayerController>();
         rend = this.GetComponent<Renderer>();
        //  rend.enabled = false;
       
     }
 
     void Update()
     {
         if ((transform.position - Camera.main.transform.position).sqrMagnitude < pC.Dist()) 
         {
             rend.material = _transparent;
         }
         else
         {
             rend.material = _original;
         }
     }
 }
