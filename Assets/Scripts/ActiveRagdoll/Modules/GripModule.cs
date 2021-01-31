using UnityEngine;

namespace ActiveRagdoll
{
    // Author: Sergio Abreu García | https://sergioabreu.me

    public class GripModule : Module {
        [Tooltip("What's the minimum weight the arm IK should have over the whole " +
        "animation to activate the grip")]
        public float leftArmWeightThreshold = 0.5f, rightArmWeightThreshold = 0.5f;
        public JointMotionsConfig defaultMotionsConfig;

        [Tooltip("Whether to only use Colliders marked as triggers to detect grip collisions.")]
        public bool onlyUseTriggers = false;
        public bool canGripYourself = false;

        private Gripper _leftGrip, _rightGrip;

        [SerializeField] AudioClip _soundOnGrip = default;
        [SerializeField] Vector2 _gripPitchRange = Vector2.one;
        [SerializeField] float _gripVolume = 1f;
        public AudioClip SoundOnGrip => _soundOnGrip;
        public Vector2 GripPitchRange => _gripPitchRange;
        public float GripVolume => _gripVolume;

        [SerializeField] AudioClip _soundOnUngrip = default;
        [SerializeField] Vector2 _ungripPitchRange = Vector2.one;
        [SerializeField] float _ungripVolume = 1f;
        public AudioClip SoundOnUngrip => _soundOnUngrip;
        public Vector2 UngripPitchRange => _ungripPitchRange;
        public float UngripVolume => _ungripVolume;

        private void Start() {
            var leftHand = _activeRagdoll.GetPhysicalBone(HumanBodyBones.LeftHand).gameObject;
            var rightHand = _activeRagdoll.GetPhysicalBone(HumanBodyBones.RightHand).gameObject;

            (_leftGrip = leftHand.AddComponent<Gripper>()).GripMod = this;
            (_rightGrip = rightHand.AddComponent<Gripper>()).GripMod = this;
        }


        public void UseLeftGrip(float weight) {
            _leftGrip.enabled = weight > leftArmWeightThreshold;
        }

        public void UseRightGrip(float weight) {
            _rightGrip.enabled = weight > rightArmWeightThreshold;
        }
    }
} // namespace ActiveRagdoll