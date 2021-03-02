using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace WeaponInteract
{
    [Serializable]
    public class Weapon : XRGrabInteractable
    {
        private bool switched;
        public float breakDistance = 0.25f;
        public int recoilAmount = 25;
        public bool oneHanded;
        private GripInteract _gripInteract;
        private XRBaseInteractor _gripHand;
        private XRController _gripController;
        private HandguardInteract _handguardInteract;
        private XRBaseInteractor _handguardHand;
        private Barrel _barrel;
        private Rigidbody rb;
        private readonly Vector3 gripRot = new Vector3(45,0,0);
        private Quaternion lookRotation;
        [SerializeField] private Collider reloadArea;
        public bool Switched
        {
            get => switched;
            set => switched = value;
        }

        protected override void Awake()
        {
            base.Awake();
            SetUpHolds();
            SetUpExtra();
            selectEntered.AddListener(SetIniRotation);
            reloadArea = GameObject.Find("ReloadArea").GetComponent<Collider>(); //Change this ASAP when variables work in inspector
        }

        private void SetUpHolds()
        {
            _gripInteract = GetComponentInChildren<GripInteract>();
            _gripInteract.Setup(this);
            if (oneHanded) return; //Leave this for when i can change variables in inspector again
            _handguardInteract = GetComponentInChildren<HandguardInteract>();
            if (!_handguardInteract) return; //Remove when variables in inspector back
            _handguardInteract.Setup(this);
        }

        private void SetUpExtra()
        {
            rb = GetComponent<Rigidbody>();
            _barrel = GetComponentInChildren<Barrel>();
            _barrel.Setup(this);
        }

        private void Update()
        {
            if (!_gripInteract) return;
            SwitchMode();
        }

        private void SwitchMode()
        {
            if (_barrel.isOnlySingleFireMode) return;
            if (!_gripInteract.SwitchFireMode(_gripController)) return;
            if (switched) return;
            switched = true;
            _barrel.isOnSingleFireMode = !_barrel.isOnSingleFireMode;
        }

        private void OnDestroy()
        {
            selectEntered.RemoveListener(SetIniRotation);
        }

        private void SetIniRotation(SelectEnterEventArgs args)
        {
            var newRotation = Quaternion.Euler(gripRot);
            args.interactor.attachTransform.localRotation = newRotation;
        }

        public void SetGripHand(SelectEnterEventArgs arg)
        {
            _gripHand = arg.interactor;
            _gripController = _gripHand.GetComponent<XRController>();
            OnSelectEntering(arg);
        }
    
        public void ClearGripHand(SelectExitEventArgs args)
        {
            _gripHand = null;
            _gripController = null;
            try
            {
                _barrel.StopFire();
            }
            catch
            {
                // ignored
            }
            OnSelectExiting(args);
        }
    
        public void SetGuardHand(SelectEnterEventArgs args)
        {
            _handguardHand = args.interactor;
        }
    
        public void ClearGuardHand()
        {
            _handguardHand = null;
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);
            if (_gripHand && _handguardHand) SetGripRotation(GetGripRotation());
            CheckDistance(_gripHand, _gripInteract);
            CheckDistance(_handguardHand, _handguardInteract);
        }

        private Quaternion GetGripRotation()
        {
            var target = _handguardHand.attachTransform.position - _gripHand.attachTransform.position;
            lookRotation = Quaternion.LookRotation(target);
            return lookRotation;
        }

        private void SetGripRotation(Quaternion _lookRotation)
        {
            var gripRotation = Vector3.zero;
            gripRotation.z = _gripHand.transform.eulerAngles.z;
            _lookRotation *= Quaternion.Euler(gripRotation);
            _gripHand.attachTransform.rotation = _lookRotation;
            _handguardHand.transform.rotation = _gripHand.transform.rotation;
        }
    
        private void CheckDistance(XRBaseInteractor interactor, HandHold handHold)
        {
            if (!interactor) return;
            var disSqr = GetDistanceSqrToInteractor(interactor);
            if (disSqr > breakDistance) handHold.BreakHold(new SelectExitEventArgs());
        }

        public void PullTrigger()
        {
            _barrel.Fire();
        }

        public void ReleaseTrigger()
        {
            _barrel.StopFire();
            _barrel.SingleFired = false;
        }

        public void Recoil()
        {
            //rb.AddRelativeForce(Vector3.back*recoilAmount, ForceMode.Impulse); not nice
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != reloadArea) return;
            _barrel.Reload();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other != reloadArea) return;
            _barrel.StopReload();
        }

        public void UseVibration()
        {
            _gripInteract.Vibrate();
        }


        /*IDEAS
     * Reset position when hand distance breaks or release handguard in awkward position
     * POSES
     * Second hand Attach Transform so user can set where it's placed
     */
    }
}
