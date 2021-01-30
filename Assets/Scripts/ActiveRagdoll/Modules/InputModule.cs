using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ActiveRagdoll {
    // Author: Sergio Abreu García | https://sergioabreu.me

    /// <summary> Tells the ActiveRagdoll what it should do. Input can be external (like the
    /// one from the player or from another script) and internal (kind of like sensors, such as
    /// detecting if it's on floor). </summary>
    public class InputModule : Module {
        public event Action<Vector2> onMove;
        public event Action<Vector2> onAim;
        public event Action<float> onLeftArm;
        public event Action<float> onRightArm;
        public event Action<bool> onFloorChanged;
        // ---------- INTERNAL INPUT ----------

        [Header("Floot")]
        [SerializeField] float floorDetectionDistance = 0.3f;
        [SerializeField] float maxFloorSlope = 60;

        private bool _isOnFloor = true;
        public bool IsOnFloor => _isOnFloor;

        Rigidbody _rightFoot = default;
        Rigidbody _leftFoot = default;

        void OnMove(InputValue value) => onMove?.Invoke(value.Get<Vector2>());
        void OnAim(InputValue value) => onAim?.Invoke(value.Get<Vector2>());
        void OnLeftArm(InputValue value) => onLeftArm?.Invoke(value.Get<float>());
        void OnRightArm(InputValue value) => onRightArm?.Invoke(value.Get<float>());

        void Start() 
        {
            _rightFoot = _activeRagdoll.GetPhysicalBone(HumanBodyBones.RightFoot).GetComponent<Rigidbody>();
            _leftFoot = _activeRagdoll.GetPhysicalBone(HumanBodyBones.LeftFoot).GetComponent<Rigidbody>();
        }

        void Update() 
        {
            bool lastIsOnFloor = _isOnFloor;

            _isOnFloor = CheckRigidbodyOnFloor(_rightFoot, out var _) || CheckRigidbodyOnFloor(_leftFoot, out _);

            if (_isOnFloor != lastIsOnFloor)
                onFloorChanged?.Invoke(_isOnFloor);
        }


        /// <summary>
        /// Checks whether the given rigidbody is on floor
        /// </summary>
        /// <param name="bodyPart">Part of the body to check</param
        /// <returns> True if the Rigidbody is on floor </returns>
        public bool CheckRigidbodyOnFloor(Rigidbody bodyPart, out Vector3 normal) 
        {
            // Raycast
            Ray ray = new Ray(bodyPart.position, Vector3.down);
            bool onFloor = Physics.Raycast(ray, out RaycastHit info, floorDetectionDistance, ~(1 << bodyPart.gameObject.layer));

            // Additional checks
            onFloor = onFloor && Vector3.Angle(info.normal, Vector3.up) <= maxFloorSlope;

            if (onFloor && info.collider.gameObject.TryGetComponent<Floor>(out Floor floor))
                onFloor = floor.isFloor;

            normal = info.normal;
            return onFloor;
        }
    }
} // namespace ActiveRagdoll