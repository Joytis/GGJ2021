using UnityEngine;

namespace ActiveRagdoll {
    // Author: Sergio Abreu García | https://sergioabreu.me

    public class Gripper : MonoBehaviour {
        public GripModule GripMod { get; set; }

        /// <summary> If the component is activated after colliding with something, it won't grip
        /// to it unless the collision enters again. This variable hold the last collision to avoid
        /// skipping it. </summary>
        private Rigidbody _lastCollision;

        private ConfigurableJoint _joint = default;
        private Grippable _gripped = default;
        private Rigidbody _lastGripped = default;
        private AudioSource _source = default;
        
        public void Start() {
            // Start disabled is useful to avoid fake gripping something at the start
            _source = gameObject.AddComponent<AudioSource>();
            enabled = false;
        }



        private void Grip(Rigidbody whatToGrip) {
            if (!enabled) {
                _lastCollision = whatToGrip;
                return;
            }

            if (_joint != null)
                return;

            if (!GripMod.canGripYourself
                    && whatToGrip.transform.IsChildOf(GripMod.ActiveRagdoll.transform))
                return;

            _joint = gameObject.AddComponent<ConfigurableJoint>();
            _joint.connectedBody = whatToGrip;
            _joint.xMotion = ConfigurableJointMotion.Locked;
            _joint.yMotion = ConfigurableJointMotion.Locked;
            _joint.zMotion = ConfigurableJointMotion.Locked;

            _lastGripped = whatToGrip;
            if (whatToGrip.TryGetComponent(out _gripped))
                _gripped.jointMotionsConfig.ApplyTo(ref _joint);
            else
                GripMod.defaultMotionsConfig.ApplyTo(ref _joint);

            // Check for friend gripping
            var component = _lastGripped.GetComponentInParent<IGrippable>();
            if(component != null)
            {
                component.Grip();
            }

            // PLay grip sound
            _source.clip = GripMod.SoundOnGrip;
            _source.pitch = Random.Range(GripMod.GripPitchRange.x, GripMod.GripPitchRange.y);
            _source.volume = GripMod.GripVolume;
            _source.Play();
        }

        private void UnGrip() {
            if (_joint == null)
                return;

            Destroy(_joint);
            _joint = null;

            if(_lastGripped != null)
            {
                var component = _lastGripped.GetComponentInParent<IGrippable>();
                if(component != null)
                {
                    component.Ungrip();
                }
            }
            _gripped = null;

            // PLay Ungrip sound
            _source.clip = GripMod.SoundOnUngrip;
            _source.pitch = Random.Range(GripMod.UngripPitchRange.x, GripMod.UngripPitchRange.y);
            _source.volume = GripMod.UngripVolume;
            _source.Play();
        }



        private void OnCollisionEnter(Collision collision) {
            if (GripMod.onlyUseTriggers)
                return;

            if (collision.rigidbody != null)
                Grip(collision.rigidbody);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.attachedRigidbody != null)
                Grip(other.attachedRigidbody);
        }

        private void OnCollisionExit(Collision collision) {
            if (collision.rigidbody == _lastCollision)
                _lastCollision = null;
        }

        private void OnTriggerExit(Collider other) {
            if (other.attachedRigidbody == _lastCollision)
                _lastCollision = null;
        }



        private void OnEnable() {
            if (_lastCollision != null)
                Grip(_lastCollision);
        }

        private void OnDisable() {
            UnGrip();
        }
    }
} // namespace ActiveRagdoll
