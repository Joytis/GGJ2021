

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveRagdoll {
    // Author: Sergio Abreu García | https://sergioabreu.me

    [Serializable]
    public struct JointDriveConfig {
        // Variables are exposed in the editor, but are kept readonly from code since
        // changing them would have no effect until assigned to a JointDrive.
        [SerializeField] private float _positionSpring; 
        [SerializeField] private float _positionDamper;
        [SerializeField] private float _maximumForce;

        public float PositionSpring => _positionSpring;
        public float PositionDamper => _positionDamper;
        public float MaximumForce => _maximumForce;

        // Dummy constructor
        public JointDriveConfig(float dummy) 
        {
            _positionSpring = dummy;
            _positionDamper = dummy;
            _maximumForce = dummy;
        }

        public static explicit operator JointDrive(JointDriveConfig config) {
            JointDrive jointDrive = new JointDrive {
                positionSpring = config._positionSpring,
                positionDamper = config._positionDamper,
                maximumForce = config._maximumForce
            };

            return jointDrive;
        }

        public static explicit operator JointDriveConfig(JointDrive jointDrive) {
            JointDriveConfig jointDriveConfig = new JointDriveConfig {
                _positionSpring = jointDrive.positionSpring,
                _positionDamper = jointDrive.positionDamper,
                _maximumForce = jointDrive.maximumForce
            };

            return jointDriveConfig;
        }

        public readonly static JointDriveConfig ZERO = new JointDriveConfig
                               { _positionSpring = 0, _positionDamper = 0, _maximumForce = 0};

        public static JointDriveConfig operator *(JointDriveConfig config, float multiplier) {
            return new JointDriveConfig {
                _positionSpring = config.PositionSpring * multiplier,
                _positionDamper = config.PositionDamper * multiplier,
                _maximumForce = config.MaximumForce * multiplier
            };
        }
    }

    [Serializable]
    public class BodyPart {

        [SerializeField] private List<ConfigurableJoint> _joints;
        private List<JointDriveConfig> XjointDriveConfigs;
        private List<JointDriveConfig> YZjointDriveConfigs;

        public BodyPart(string name, List<ConfigurableJoint> joints) {
            _joints = joints;
        }

        public void Init() {
            XjointDriveConfigs = new List<JointDriveConfig>();
            YZjointDriveConfigs = new List<JointDriveConfig>();

            foreach (ConfigurableJoint joint in _joints) {
                XjointDriveConfigs.Add((JointDriveConfig) joint.angularXDrive);
                YZjointDriveConfigs.Add((JointDriveConfig) joint.angularYZDrive);
            }

        }

        public void SetStrengthScale(float scale) {
            for (int i = 0; i < _joints.Count; i++) {
                _joints[i].angularXDrive = (JointDrive) (XjointDriveConfigs[i] * scale);
                _joints[i].angularYZDrive = (JointDrive) (YZjointDriveConfigs[i] * scale);
            }

        }
    }

    [Serializable] public struct JointMotionsConfig {
        public ConfigurableJointMotion angularXMotion, angularYMotion, angularZMotion;
        public float angularXLimit, angularYLimit, angularZLimit;

        public void ApplyTo(ref ConfigurableJoint joint) {
            joint.angularXMotion = angularXMotion;
            joint.angularYMotion = angularYMotion;
            joint.angularZMotion = angularZMotion;

            var softJointLimit = new SoftJointLimit();

            softJointLimit.limit = angularXLimit / 2;
            joint.highAngularXLimit = softJointLimit;

            softJointLimit.limit = -softJointLimit.limit;
            joint.lowAngularXLimit = softJointLimit;

            softJointLimit.limit = angularYLimit;
            joint.angularYLimit = softJointLimit;

            softJointLimit.limit = angularZLimit;
            joint.angularZLimit = softJointLimit;
        }
    }

    public static class Auxiliary {
        /// <summary>
        /// Calculates the normalized projection of the Vector3 'vec'
        /// onto the horizontal plane defined by the orthogonal vector (0, 1, 0)
        /// </summary>
        /// <param name="vec">The vector to project</param>
        /// <returns>The normalized projection of 'vec' onto the horizontal plane</returns>
        public static Vector3 GetFloorProjection(in Vector3 vec) {
            return Vector3.ProjectOnPlane(vec, Vector3.up).normalized;
        }
    }

}
