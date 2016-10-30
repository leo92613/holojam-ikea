using UnityEngine;
using System.Collections;
namespace Holojam.Tools
{
    public class IkeaCursor : MonoBehaviour
    {
        private GameObject preFur;
        // Use this for initialization
        public Vector3 CursorLocation = new Vector3();
        public GameObject sign;
        void Start()
        {
            preFur = null;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, this.GetComponent<Actor>().look, out hit))
            {
                sign.transform.position = hit.point;
                //sign.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green) ;
                if (hit.transform.gameObject != preFur)
                {
                    if (preFur != null)
                    {
                        preFur.GetComponent<Furniture>().DeleteLookon(this.GetComponent<Actor>());
                        preFur = null;
                    }
                    if (hit.transform.gameObject.tag == "furniture")
                    {
                        //sign.GetComponent<MeshRenderer>().enabled = true;
                        sign.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                        sign.transform.position = hit.point;
                        sign.transform.parent = hit.transform;
                        preFur = hit.transform.gameObject;
                        preFur.GetComponent<Furniture>().AddLookon(this.GetComponent<Actor>());
                    }
                    else
                    {
                        sign.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                    }
                }
               // Debug.Log(preFur.name);

            }
            else
            {
                if(preFur != null)
                    preFur.GetComponent<Furniture>().DeleteLookon(this.GetComponent<Actor>());
                preFur = null;
            }
            CursorLocation = GetHitOnFloor();
        }

        private Vector3 GetHitOnFloor()
        {
            int layerMask = 1 << 8;
            RaycastHit hit;
            if (Physics.Raycast(transform.position,this.GetComponent<Actor>().look, out hit, 10f, layerMask))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(this.GetComponent<Actor>().look) * hit.distance, Color.yellow);
                return hit.point;
            }
            return CursorLocation;
        }
    }
}
