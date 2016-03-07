using UnityEngine;
using System.Collections;

public class HumanPlayerController : MonoBehaviour,IPlayerController {
   public Vector2 GetInput()
   {
       return new Vector2(0, Input.GetAxisRaw("Vertical"));
   }
}
