//Synchronizable.cs
//Created by Aaron C Gaudette on 11.07.16

using UnityEngine;
using Holojam.Network;

namespace Holojam.Tools
{
    [ExecuteInEditMode, RequireComponent(typeof(HolojamView))]
    public class Phonecontroller : MonoBehaviour
    {
        public string label = "controller0";
        public int index;
        bool useMasterPC = false;
        bool sending = false;
        
        //Manage view
        public HolojamView view
        {
            get
            {
                if (holojamView == null) holojamView = GetComponent<HolojamView>();
                return holojamView;
            }
        }
        HolojamView holojamView = null;

        protected void UpdateView()
        {
            view.Label = label;
            sending = sending && (Utility.IsMasterPC() || !useMasterPC);
            view.IsMine = sending;
        }

        public Vector3 synchronizedVector3
        {
            get { return view.RawPosition; }
            set { if (sending) view.RawPosition = value; }
        }
        public Quaternion synchronizedQuaternion
        {
            get { return view.RawRotation; }
            set { if (sending) view.RawRotation = value; }
        }
        public int synchronizedInt
        {
            get { return view.Bits; }
            set { if (sending) view.Bits = value; }
        }
        public string synchronizedString
        {
            get { return view.Blob; }
            set { if (sending) view.Blob = value; }
        }

        protected virtual void Update()
        {
            UpdateView(); //Mandatory initialization call

            //Optional check--you probably don't want to run this code in edit mode
            if (!Application.isPlaying) return;

            Sync();
        }

        //Override this in derived classes
        protected virtual void Sync()
        {
            //By default syncs transform data
            if (sending)
            {
                synchronizedVector3 = transform.position;
                synchronizedQuaternion = transform.rotation;
            }
            else
            {
                transform.position = synchronizedVector3;
                transform.rotation = synchronizedQuaternion;
            }
        }

     void OnDrawGizmos(){
         //DrawGizmoGhost();
         Gizmos.color = Color.red;
         Vector3 look = transform.rotation * Vector3.forward;
         Vector3 eyes = transform.position;
         Vector3 up = transform.rotation * Vector3.up;
         Vector3 left = transform.rotation * Vector3.left;
         Vector3 offset = eyes+look*0.015f;

         Drawer.Circle(offset+left*0.035f,look,up,0.03f);
         Drawer.Circle(offset-left*0.035f,look,up,0.03f);
         //Reference forward vector
         Gizmos.DrawRay(offset,look);
      }
      void OnDrawGizmosSelected(){}
    }
}