using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core.World
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField]
        public uint AsteroidID = 0; // 4bytes
        public Vector3 positionLastFrame = Vector3.zero;
        public Quaternion rotationLastFrame;
        public Rigidbody rb;
        // Start is called before the first frame update
        void Start()
        {
            positionLastFrame = rb.position;
            rotationLastFrame = rb.rotation;

        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public bool IsDirty()
        {
            return (rb.velocity.magnitude > 0 || rb.angularVelocity.magnitude > 0);
        }

        // Update is called once per frame
        void Update()
        {
            positionLastFrame = rb.position;
            rotationLastFrame = rb.rotation;
        }
    }
}