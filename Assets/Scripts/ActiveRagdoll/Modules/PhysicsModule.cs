﻿using UnityEngine;

namespace ActiveRagdoll
{
    // Author: Sergio Abreu García | https://sergioabreu.me

    public class PhysicsModule : Module {
        // --- BALANCE ---

        public enum BalanceMode {
            UprightTorque,
            ManualTorque,
            StabalizerJoint,
            FreezeRotations,
            None,
        }

        [Header("General")]
        [SerializeField] private BalanceMode _balanceMode = BalanceMode.StabalizerJoint;
        public BalanceMode Mode => _balanceMode;
        public float customTorsoAngularDrag = 0.05f;

        [Header("Upright Torque")]
        public float uprightTorque = 10000;
        [Tooltip("Defines how much torque percent is applied given the inclination angle percent [0, 1]")]
        public AnimationCurve uprightTorqueFunction;
        public float rotationTorque = 500;

        [Header("Manual Torque")]
        public float manualTorque = 500;
        public float maxManualRotSpeed = 5;

        private Vector2 _torqueInput;

        [Header("Stabilizer Joint")]
        [SerializeField] private JointDriveConfig _stabilizerJointDrive;
        public JointDriveConfig StabilizerJointDrive {
            get { return _stabilizerJointDrive; }
            set { if (_stabilizerJoint != null)
                    _stabilizerJoint.angularXDrive = _stabilizerJoint.angularXDrive = (JointDrive)value;
                }
        }

        private GameObject _stabilizerGameobject;
        private Rigidbody _stabilizerRigidbody;
        private ConfigurableJoint _stabilizerJoint;

        [Header("Freeze Rotations")]
        [SerializeField] private float freezeRotationSpeed = 5;

        // --- ROTATION ---

        public Vector3 TargetDirection { get; set; }
        private Quaternion _targetRotation;



        private void Start() {
            UpdateTargetRotation();
            InitializeStabilizerJoint();
            StartBalance();
        }

        /// <summary> Creates the stabilizer GameObject with a Rigidbody and a ConfigurableJoint,
        /// and connects this last one to the torso. </summary>
        private void InitializeStabilizerJoint() {
            _stabilizerGameobject = new GameObject("Stabilizer", typeof(Rigidbody), typeof(ConfigurableJoint));
            _stabilizerGameobject.transform.parent = _activeRagdoll.PhysicalTorso.transform.parent;
            _stabilizerGameobject.transform.rotation = _activeRagdoll.PhysicalTorso.rotation;

            _stabilizerJoint = _stabilizerGameobject.GetComponent<ConfigurableJoint>();
            _stabilizerRigidbody = _stabilizerGameobject.GetComponent<Rigidbody>();
            _stabilizerRigidbody.isKinematic = true;

            var joint = _stabilizerGameobject.GetComponent<ConfigurableJoint>();
            joint.connectedBody = _activeRagdoll.PhysicalTorso;
        }

        private void FixedUpdate() {
            UpdateTargetRotation();
            ApplyCustomDrag();

            Debug.Log(_balanceMode);

            switch (_balanceMode) {
                case BalanceMode.UprightTorque:
                    var balancePercent = Vector3.Angle(_activeRagdoll.PhysicalTorso.transform.up,
                                                         Vector3.up) / 180;
                    balancePercent = uprightTorqueFunction.Evaluate(balancePercent);
                    var rot = Quaternion.FromToRotation(_activeRagdoll.PhysicalTorso.transform.up,
                                                         Vector3.up).normalized;

                    _activeRagdoll.PhysicalTorso.AddTorque(new Vector3(rot.x, rot.y, rot.z)
                                                                * uprightTorque * balancePercent);

                    var directionAnglePercent = Vector3.SignedAngle(_activeRagdoll.PhysicalTorso.transform.forward,
                                        TargetDirection, Vector3.up) / 180;
                    _activeRagdoll.PhysicalTorso.AddRelativeTorque(0, directionAnglePercent * rotationTorque, 0);
                    break;

                case BalanceMode.FreezeRotations:
                    var smoothedRot = Quaternion.Lerp(_activeRagdoll.PhysicalTorso.rotation,
                                       _targetRotation, Time.fixedDeltaTime * freezeRotationSpeed);
                    _activeRagdoll.PhysicalTorso.MoveRotation(smoothedRot);

                    break;

                case BalanceMode.StabalizerJoint:
                    // Move stabilizer to player torso (useless, but improves clarity)
                    _stabilizerRigidbody.MovePosition(_activeRagdoll.PhysicalTorso.position);
                    _stabilizerRigidbody.MoveRotation(_targetRotation);
                    break;

                case BalanceMode.ManualTorque:
                    if (_activeRagdoll.PhysicalTorso.angularVelocity.magnitude < maxManualRotSpeed) {
                        var force = _torqueInput * manualTorque;
                        _activeRagdoll.PhysicalTorso.AddRelativeTorque(force.y, 0, force.x);
                    }

                    break;

                default: break;
            }
        }

        private void UpdateTargetRotation() {
            if (TargetDirection != Vector3.zero)
                _targetRotation = Quaternion.LookRotation(TargetDirection, Vector3.up);
            else
                _targetRotation = Quaternion.identity;
        }

        private void ApplyCustomDrag() {
            var angVel = _activeRagdoll.PhysicalTorso.angularVelocity;
            angVel -= (Mathf.Pow(angVel.magnitude, 2) * customTorsoAngularDrag) * angVel.normalized;
            _activeRagdoll.PhysicalTorso.angularVelocity = angVel;
        }

        public void SetBalanceMode(BalanceMode balanceMode) {
            StopBalance();
            _balanceMode = balanceMode;
            StartBalance();
        }

        /// <summary> Starts to balance depending on the balance mode selected </summary>
        private void StartBalance() {
            switch (_balanceMode) {
                case BalanceMode.UprightTorque:
                    break;

                case BalanceMode.FreezeRotations:
                    _activeRagdoll.PhysicalTorso.constraints = RigidbodyConstraints.FreezeRotation;
                    break;

                case BalanceMode.StabalizerJoint:
                    var jointDrive = (JointDrive) _stabilizerJointDrive;
                    _stabilizerJoint.angularXDrive = _stabilizerJoint.angularYZDrive = jointDrive;
                    break;

                case BalanceMode.ManualTorque:
                    break;

                default: break;
            }
        }

        /// <summary> Cleans up everything that was being used for the current balance mode. </summary>
        private void StopBalance() {
            switch (_balanceMode) {
                case BalanceMode.UprightTorque:
                    break;

                case BalanceMode.FreezeRotations:
                    _activeRagdoll.PhysicalTorso.constraints = 0;
                    break;

                case BalanceMode.StabalizerJoint:
                    var jointDrive = (JointDrive) JointDriveConfig.ZERO;
                    _stabilizerJoint.angularXDrive = _stabilizerJoint.angularYZDrive = jointDrive;
                    break;

                case BalanceMode.ManualTorque:
                    break;

                default: break;
            }
        }

        public void ManualTorqueInput(Vector2 torqueInput) {
            _torqueInput = torqueInput;
        }
    }
} // ManualTorque ActiveRagdoll