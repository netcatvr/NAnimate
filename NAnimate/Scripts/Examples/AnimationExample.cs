
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AnimationExample : UdonSharpBehaviour
{
    public NAnimate nAnimateInstance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Bounce (For quick and dirty cartoony effects)
            nAnimateInstance.Bounce(gameObject.transform, 0.1f, new Vector3(1,1,1));

            //Position Interpolation
            nAnimateInstance.AnimatePosition(gameObject.transform, gameObject.transform.position + new Vector3(1, 1, 1), 3, EaseType.EaseInExpo);
            
            //Rotation Interpolation
            nAnimateInstance.AnimateRotation(gameObject.transform, new Quaternion(1, 1, 1, 1), 5, EaseType.BounceOut);
        }
    }

}
