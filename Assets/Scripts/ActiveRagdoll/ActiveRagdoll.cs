using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ActiveRagdoll {
    // Author: Sergio Abreu García | https://sergioabreu.me

    public class ActiveRagdoll : MonoBehaviour {
        [Header("General")]
        [SerializeField] private int _solverIterations = 13;
        [SerializeField] private int _velSolverIterations = 13;
        [SerializeField] private float _maxAngularVelocity = 50;

        [Header("Body")]
        [SerializeField] private Transform _animatedTorso;
        [SerializeField] private Rigidbody _physicalTorso;

        [Header("Animators")]
        [SerializeField] private Animator _animatedAnimator;
        [SerializeField] private Animator _physicalAnimator;

        [SerializeField] BodyPart _headNeck = default;
        [SerializeField] BodyPart _torso = default;
        [SerializeField] BodyPart _leftArm = default;
        [SerializeField] BodyPart _rightArm = default;
        [SerializeField] BodyPart _leftLeg = default;
        [SerializeField] BodyPart _rightLeg = default;

        private static uint _idCount = 0;

        /// <summary> The unique ID of this Active Ragdoll instance. </summary>
        public uint ID { get; private set; }

        public Transform AnimatedTorso => _animatedTorso;
        public Rigidbody PhysicalTorso => _physicalTorso;

        public int SolverIterations => _solverIterations;
        public int VelSolverIterations => _velSolverIterations;
        public float MaxAngularVelocity => _maxAngularVelocity;

        public Transform[] AnimatedBones { get; private set; }
        public ConfigurableJoint[] Joints { get; private set; }
        public Rigidbody[] Rigidbodies { get; private set; }
        public Animator AnimatedAnimator => _animatedAnimator;

        public BodyPart HeadNeck => _headNeck;
        public BodyPart Torso => _torso;
        public BodyPart LeftArm => _leftArm;
        public BodyPart RightArm => _rightArm;
        public BodyPart LeftLeg => _leftLeg;
        public BodyPart RightLeg => _rightLeg;

        public event Action<Collision> onCollisionEnter;
        public event Action<Collision> onCollisionExit;
        public event Action<Collision> onCollisionStay;

        public AnimatorHelper AnimatorHelper { get; private set; }
        /// <summary> Whether to constantly set the rotation of the Animated Body to the Physical Body's.</summary>
        public bool SyncTorsoPositions { get; set; } = true;
        public bool SyncTorsoRotations { get; set; } = true;

        public void ChildCollisionEnter(Collision collision) => onCollisionEnter?.Invoke(collision);
        public void ChildCollisionExit(Collision collision) => onCollisionExit?.Invoke(collision);
        public void ChildCollisionStay(Collision collision) => onCollisionStay?.Invoke(collision);

        private void Awake() {
            ID = _idCount++;
            AnimatedBones = _animatedTorso.GetComponentsInChildren<Transform>();
            Joints = _physicalTorso.GetComponentsInChildren<ConfigurableJoint>();
            Rigidbodies = _physicalTorso.GetComponentsInChildren<Rigidbody>();

            foreach(var rb in Rigidbodies)
            {
                rb.gameObject.AddComponent<CollisionForwarder>().Initialize(this);

            }

            foreach (Rigidbody rb in Rigidbodies) {
                rb.solverIterations = _solverIterations;
                rb.solverVelocityIterations = _velSolverIterations;
                rb.maxAngularVelocity = _maxAngularVelocity;
            }

            _headNeck.Init();
            _torso.Init();
            _leftArm.Init();
            _rightArm.Init();
            _leftLeg.Init();
            _rightLeg.Init();

            AnimatorHelper = _animatedAnimator.gameObject.AddComponent<AnimatorHelper>();
        }

        private void FixedUpdate() {
            SyncAnimatedBody();
        }

        /// <summary> Updates the rotation and position of the animated body's root
        /// to match the ones of the physical.</summary>
        private void SyncAnimatedBody() {
            // This is needed for the IK movements to be synchronized between
            // the animated and physical bodies. e.g. when looking at something,
            // if the animated and physical bodies are not in the same spot and
            // with the same orientation, the head movement calculated by the IK
            // for the animated body will be different from the one the physical body
            // needs to look at the same thing, so they will look at totally different places.
            if (SyncTorsoPositions)
                _animatedAnimator.transform.position = _physicalTorso.position
                                + (_animatedAnimator.transform.position - _animatedTorso.position);
            if (SyncTorsoRotations)
                _animatedAnimator.transform.rotation = _physicalTorso.rotation;
        }


        // ------------------- GETTERS & SETTERS -------------------

        /// <summary> Gets the transform of the given ANIMATED BODY'S BONE </summary>
        /// <param name="bone">Bone you want the transform of</param>
        /// <returns>The transform of the given ANIMATED bone</returns>
        public Transform GetAnimatedBone(HumanBodyBones bone) {
            return _animatedAnimator.GetBoneTransform(bone);
        }

        /// <summary> Gets the transform of the given PHYSICAL BODY'S BONE </summary>
        /// <param name="bone">Bone you want the transform of</param>
        /// <returns>The transform of the given PHYSICAL bone</returns>
        public Transform GetPhysicalBone(HumanBodyBones bone) {
            return _physicalAnimator.GetBoneTransform(bone);
        }
    }
} // namespace ActiveRagdoll