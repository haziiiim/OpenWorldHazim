using System;
using UnityEngine;

namespace Blobcreate.ProjectileToolkit
{
    public class AerodynamicMoveRuntime : MonoBehaviour
    {
        private Vector3 acc;
        private bool isRunning;
        private float timeOfFlight;
        private float timer;
        private Rigidbody ball;
        private bool isAccelerating;
        private Vector3 vStore;
        
        private Action<Rigidbody> OnFinished;

        /// <summary>
        /// The duration needed for the projectile to move to end point, the acceleration of aerodynamic
        /// move is applied during this period of time.
        /// </summary>
        public float TimeOfFlight => timeOfFlight;
        
        /// <summary>
        /// How much time has been passed since the projectile launch.
        /// </summary>
        public float Timer => timer;
        
        /// <summary>
        /// Acquires the values from the AerodynamicMoveSolver instance called solver. A valid instance
        /// should have been called Solve(...) beforehand.
        /// </summary>
        /// <param name="solver">The AerodynamicMoveSolver instance to acquire values from.</param>
        /// <param name="rBody">The Rigidbody of the object to which the acceleration is applied.</param>
        public void AcquireRuntimeParameters(AerodynamicMoveSolver solver, Rigidbody rBody)
        {
            acc = solver.Acceleration;
            timeOfFlight = solver.TimeOfFlight;
            OnFinished = solver.OnFinished;
            ball = rBody;
        }

        /// <summary>
        /// Starts running the AerodynamicMoveRuntime.
        /// </summary>
        public void Run()
        {
            isRunning = true;
        }

        /// <summary>
        /// Pauses the AerodynamicMoveRuntime. Can be useful for implementing replay and handling collisions.
        /// </summary>
        public void Pause()
        {
            isRunning = false;
        }
        
        /// <summary>
        /// Pauses the AerodynamicMoveRuntime and reset the internal timer, i.e., stop. Can be useful for
        /// implementing replay and handling collisions.
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            timer = 0f;
        }

        protected virtual void FixedUpdate()
        {
            if (isRunning)
                ApplyAcceleration();
        }

        private bool ApplyAcceleration()
        {
            if (timer <= timeOfFlight && timer + Time.fixedDeltaTime > timeOfFlight)
            {
                var t = timeOfFlight - timer;
                vStore = ball.velocity;
                ball.velocity = vStore / Time.fixedDeltaTime * t;
                timer += Time.fixedDeltaTime;
                return true;
            }
            
            if (timer > timeOfFlight)
            {
                if (isAccelerating)
                {
                    ball.velocity = vStore;
                    OnFinished?.Invoke(ball);
                }

                isAccelerating = false;
                return false;
            }

            if (timer > 0f)
                ball.AddForce(acc, ForceMode.Acceleration);
            else
                ball.AddForce(0.5f * acc, ForceMode.Acceleration);

            timer += Time.fixedDeltaTime;
            isAccelerating = true;
            return true;
        }
    }
}