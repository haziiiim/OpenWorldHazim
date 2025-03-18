using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
    public class CurvyTest : MonoBehaviour
    {
        public Rigidbody ballPrefab;
        public Transform launchPoint;
        public float heightAboveTarget = 12f;
        public Vector3 offsetLocal = new Vector3(25f, 3f, 10f);
        public float ballRadius;
        public Transform flag;
        public bool useCallback;
        public PEBTrajectoryPredictor predictor;
        public bool controlledByAnotherScript;

        Vector3 targetPoint;
        Rigidbody currentBall;
        AerodynamicMoveSolver amSolver;
        Camera cam;
        float amTimer;
        bool isLaunching;

        void Start()
        {
            cam = Camera.main;

            amSolver = new AerodynamicMoveSolver();
            // Step 0 (optional):
            // Add a custom callback that will be called when the projectile reaches target.
            amSolver.OnFinished = (ball) =>
            {
                if (!useCallback)
                    return;
                ball.angularVelocity = Vector3.zero;
                ball.velocity = new Vector3(0, 10, 0);
            };

            // Prediction:
            // Create a new instance of the ball at the launch point for the predictor to use.
            var b = Instantiate(ballPrefab, launchPoint.position, launchPoint.rotation);
            predictor.Simulatee = b;
            predictor.AMSolver = amSolver;
            b.gameObject.SetActive(false);
        }

        void Update()
        {
            var startPoint = launchPoint.position;
            var hit = default(RaycastHit);

            if (!controlledByAnotherScript)
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 200f))
                    targetPoint = hit.point + hit.normal * ballRadius;

            // Step 1:
            // Solve it (you can use any methods in Projectile class instead of only VelocityByHeight).
            var offY = new Vector3(0, offsetLocal.y, 0);
            var vRaw = Projectile.VelocityByHeight(startPoint, targetPoint + offY, heightAboveTarget);
            var vReal = amSolver.Solve(startPoint, targetPoint, offsetLocal, vRaw);
            amTimer = 0f;  // <-- Only needed if you use ^method 2

            // Prediction:
            // Set up and run PEBTrajectoryPredictor.
            predictor.LaunchVelocity = vReal + Projectile.VelocityCompensation;
            predictor.SimulateAndRender();

            isLaunching = isLaunching || (!controlledByAnotherScript && Input.GetMouseButtonDown(0));

            if (isLaunching)
            {
                flag.position = targetPoint;
                flag.rotation = Quaternion.LookRotation(hit.normal);

                // Launch
                currentBall = Instantiate(ballPrefab, startPoint, launchPoint.rotation);
                currentBall.AddForce(vReal + Projectile.VelocityCompensation, ForceMode.VelocityChange);

                // Step 2:
                // Set up execution of aerodynamic move.
                //
                // This is ^method 1, which adds AerodynamicMoveRuntime to each ball,
                // allowing unlimited number of objects doing aerodynamic move simultaneously.
                currentBall.TryGetComponent<AerodynamicMoveRuntime>(out var rt);
                rt ??= currentBall.gameObject.AddComponent<AerodynamicMoveRuntime>();
                rt.AcquireRuntimeParameters(amSolver, currentBall);
                rt.Run();

                isLaunching = false;
            }
        }

        // // This is ^method 2, only one object can do aerodynamic move at a time, but it is more lightweight
        // // than ^method 1 and is easier to embed into custom logic (PEBTrajectoryPredictor uses this).
        // // Uncomment this and comment out ^method 1 to use.
        // void FixedUpdate()
        // {
        //     if (currentBall != null)
        //     {
        //         amSolver.ApplyAcceleration(currentBall, ref amTimer);
        //     }
        // }

// ----------

        #region Curated control methods
        // Turn on controlledByAnotherScript to use the following methods.

        public void Aim(Vector3 value)
        {
            targetPoint = value;
        }

        public void Launch()
        {
            isLaunching = true;
        }

        #endregion
    }
}