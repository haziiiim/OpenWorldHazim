using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    private StarterAssetsInputs starterAssetsInput;
    private ThirdPersonController thirdPersonController;
    [SerializeField] private float normalSensetivity;
    [SerializeField] private float aimSensetivity;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform bulletSpawningPosition;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInput = GetComponent<StarterAssetsInputs>();
    }


    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask)) ;
        debugTransform.position = raycastHit.point;
        mouseWorldPosition = raycastHit.point;


        if (starterAssetsInput.aim)
        {
            aimCamera.gameObject.SetActive(true);
            thirdPersonController.setSensetivity(aimSensetivity);
            thirdPersonController.setRotateOnMove(false);
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 AimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, AimDirection, Time.deltaTime*20.0f);
        }

       
        else
        {
            aimCamera.gameObject.SetActive(false);
            thirdPersonController.setSensetivity(normalSensetivity);
            thirdPersonController.setRotateOnMove(true);
        }

        if (starterAssetsInput.shoot)
        {

        }
       
        
    }

}
