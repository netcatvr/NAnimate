
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AnimationExample : UdonSharpBehaviour
{
    public NAnimate nAnimateInstance;
    public Transform cube2;
    public Transform cube3;

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

            nAnimateInstance.Bounce(cube2, 3, new Vector3(1, 1, 1));
            nAnimateInstance.Bounce(cube3, 5, new Vector3(2,2,2));


        }
    }

}
