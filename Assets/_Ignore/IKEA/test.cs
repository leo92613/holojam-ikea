using UnityEngine;
using System.Collections;
namespace Holojam.Tools
{ 
public class test : MonoBehaviour {
    public GameObject aaa;
        public IkeaCursor ac;
    // Use this for initialization
    void Start() {
            ac = aaa.GetComponent<IkeaCursor>();
    }

    // Update is called once per frame
    void Update() {
            this.transform.position = ac.CursorLocation;
    }
}
}
