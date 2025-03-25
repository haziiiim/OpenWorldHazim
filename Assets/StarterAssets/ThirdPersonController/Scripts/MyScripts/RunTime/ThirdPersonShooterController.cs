using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using Unity.VisualScripting;
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
    [SerializeField] private Transform spawningBulletPosition;
    [SerializeField] private Animator animator; 

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInput = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }



        if (starterAssetsInput.aim)
        {
            aimCamera.gameObject.SetActive(true);
            thirdPersonController.setSensetivity(aimSensetivity);
            thirdPersonController.setRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime*10f));
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
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime*20f));
        }

        if (starterAssetsInput.shoot)
        {
            Debug.Log("spawn a bullet");
            Vector3 aimDir = (mouseWorldPosition - spawningBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile, spawningBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInput.shoot = false;


        }
       
        
    }

}
