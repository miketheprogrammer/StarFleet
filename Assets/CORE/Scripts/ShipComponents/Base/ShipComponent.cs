using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ShipComponents
{
    public class ShipComponent : MonoBehaviour
    {
        protected Chassis chassis;
        // Start is called before the first frame update
        protected void Start()
        {
            EnsureReferences();
        }

        protected void EnsureReferences()
        {
            if (chassis == null)
            {
                chassis = transform.parent.parent.gameObject.GetComponent<Chassis>();
            }
        }

        protected void Update()
        {
            EnsureReferences();
        }
    }
}