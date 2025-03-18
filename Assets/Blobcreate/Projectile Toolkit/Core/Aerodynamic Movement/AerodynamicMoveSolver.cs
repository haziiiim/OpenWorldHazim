using System;
using UnityEngine;

namespace Blobcreate.ProjectileToolkit
{
    public class AerodynamicMoveSolver
    {
        private Vector3 offsetVector;
        private Vector3 vReal;
        private Vector3 acc;
        private float timeOfFlight;
        private bool isAccelerating;
        private Vector3 vStore;
        
        /// <summary>
        /// Custom callback that will be called when the projectile reaches target.
        /// Note that it is triggered after a pre-calculated time, not by collision.
        /// </summary>
        public Action<Rigidbody> OnFinished;

        /// <summary>
        /// The modified launch velocity that takes aerodynamic movement into account.
        /// The same value as the return value of Solve(...).
        /// </summary>
        public Vector3 SolvedVelocity => vReal;
        
        /// <summary>
        /// The continuous acceleration that is used to conduct aerodynamic movement.
        /// </summary>
        public Vector3 Acceleration => acc;

        /// <summary>
        /// The world space version of offsetLocal.
        /// It is used to get offset end point ( = end + OffsetVector).
        /// </summary>
        public Vector3 OffsetVector => offsetVector;

        /// <summary>
        /// The duration needed for the projectile to move to end point, the acceleration of
        /// aerodynamic move is applied during this period of time.
        /// </summary>
        public float TimeOfFlight => timeOfFlight;

        /// <summary>
        /// Computes the values required for aerodynamic movement.
        /// </summary>
        /// <param name="start">The launch point.</param>
        /// <param name="end">The target point.</param>
        /// <param name="offsetLocal">Of a aerodynamic movement, the max displacement along local space,
        /// the local space is formed by projecting "end - start" onto xz plane as forward vector.</param>
        /// <param name="v">The original launch velocity that makes the projectile move from start point
        /// to end point without taking aerodynamic move into account.</param>
        /// <returns>The modified launch velocity that takes aerodynamic move into account.</returns>
        public Vector3 Solve(Vector3 start, Vector3 end, Vector3 offsetLocal, Vector3 v)
        {
            // Gets the duration that the projectile will fly.
            Projectile.FlightTest(start, end, v, FlightTestMode.Horizontal, out timeOfFlight);
            
            var f = end - start;
            f.y = 0f;
            offsetVector = Quaternion.LookRotation(f) * offsetLocal;
            
            // Computes the continuous acceleration that will be applied to the Rigidbody.
            acc = AccelerationByTime(end + offsetVector, Vector3.zero, end, timeOfFlight);
            vReal = Projectile.VelocityByTime(start, end + offsetVector, timeOfFlight);
            return vReal;
        }
        
        // Much more complicated than I thought, several
        // tricks are involved to make the movement accurate.
        /// <summary>
        /// Executes the aerodynamic movement.
        /// </summary>
        /// <param name="rBody">The Rigidbody of the object to which the acceleration is applied.</param>
        /// <param name="timer">This is used as a runtime context, it records how much time has been passed
        /// since the projectile launch.</param>
        /// <returns>Whether or not the acceleration is applied. False means the procedure is finished.</returns>
        public bool ApplyAcceleration(Rigidbody rBody, ref float timer)
        {
            if (timer <= timeOfFlight && timer + Time.fixedDeltaTime > timeOfFlight)
            {
                var t = timeOfFlight - timer;
                vStore = rBody.velocity;
                rBody.velocity = vStore / Time.fixedDeltaTime * t;
                timer += Time.fixedDeltaTime;
                return true;
            }
            
            if (timer > timeOfFlight)
            {
                if (isAccelerating)
                {
                    rBody.velocity = vStore;
                    OnFinished?.Invoke(rBody);
                }

                isAccelerating = false;
                return false;
            }

            if (timer > 0f)
                rBody.AddForce(acc, ForceMode.Acceleration);
            else
                rBody.AddForce(0.5f * acc, ForceMode.Acceleration);

            timer += Time.fixedDeltaTime;
            isAccelerating = true;
            return true;
        }
        
        private Vector3 AccelerationByTime(Vector3 pointA, Vector3 velocityA, Vector3 pointB, float time)
        {
            var inverseTime = 1f / time;
            var dS = pointB - pointA;
            var vB = dS * (2f * inverseTime) - velocityA;
            
            return (vB - velocityA) * inverseTime;
        }
    }
}