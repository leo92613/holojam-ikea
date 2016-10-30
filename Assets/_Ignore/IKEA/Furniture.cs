using UnityEngine;
//Furniture.cs
//Created by Wenbo Lan on 29.10.16

using Holojam.Network;
using System.Collections.Generic;

namespace Holojam.Tools
{
    public class Furniture : Synchronizable
    {
        [SerializeField]
        private int state = 0; //for the state of the funiture: 0 idle, 1 select, 2 moving
        private static List<Phonecontroller> Controllers
        {
            get
            {
                return GetCopyOfControllers();
            }
        }
        private GameObject cursor;
        private int controller_index;
        private List<Actor> Lookon = new List<Actor>();// Every Actor who's looking on the furniture 
        private Phonecontroller Curbutton;
        // Use this for initialization
        public static List<Phonecontroller> GetCopyOfControllers()
        {
            List<Phonecontroller> pc = new List<Phonecontroller>();
            GameObject go = GameObject.Find("ActorManager");
            Phonecontroller[] controllers = go.GetComponentsInChildren<Phonecontroller>();
            foreach (Phonecontroller c in controllers)
            {
                pc.Add(c);
            }
            return pc;
        }

        public void AddLookon(Actor a)
        {
            Lookon.Add(a);
        }
        public void DeleteLookon(Actor a)
        {
            foreach (Actor ac in Lookon)
                if (ac.tag == a.tag)
                {
                    Lookon.Remove(ac);
                    break;
                }
        }
        protected override void Sync()
        {
            //Debug.Log(Lookon.Count);
            bool follow = false;
            //controller_index = -1;
            //state = 0;
            if (Lookon.Count > 0)
            {
                state = 1;
            }else
            {
                state = 0;
            }
            
            foreach(Phonecontroller c in Controllers)
            {
                controller_index = -1;
                if (c.transform.position.z > 0.1)
                {
                    controller_index = c.index;
                    Curbutton = c;
                    break;
                }
            }
            foreach(Actor a in Lookon)
            {
                if ((int)a.trackingTag == controller_index)
                {
                    follow = true;
                    cursor = a.gameObject;
                    state = 2;
                }
            }
            if (sending)
            {
                synchronizedInt = state;
                if (follow)
                {
                    transform.position = cursor.GetComponent<IkeaCursor>().CursorLocation;
                    synchronizedVector3 = transform.position;
                    //synchronizedQuaternion = transform.rotation;
                }
                synchronizedVector3 = transform.position;
            }
            else
            {
                transform.position = synchronizedVector3;
                state = synchronizedInt;
            }

            //Debug.Log(controller_index);
            Debug.Log(state);
        }
    }
}
