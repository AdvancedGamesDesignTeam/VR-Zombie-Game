using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace WeaponInteract
{
    public class GripInteract : HandHold
    {
        private bool _isPressed;
        private bool _secondaryButton;
        protected override void BeginAction(ActivateEventArgs args)
        {
            base.BeginAction(args);
            Controller = args.interactor.GetComponent<XRController>();
            Controller.inputDevice.IsPressed(InputHelpers.Button.Trigger, out _isPressed);
            if(_isPressed) Weapon.PullTrigger();
        }

        protected override void EndAction(DeactivateEventArgs args)
        {
            base.EndAction(args);
            Controller = args.interactor.GetComponent<XRController>();
            Controller.inputDevice.IsPressed(InputHelpers.Button.Trigger, out _isPressed);
            if (_isPressed) return;
            Weapon.ReleaseTrigger();
        }

        protected override void Grab(SelectEnterEventArgs args)
        {
            base.Grab(args);
            Weapon.SetGripHand(args);
        }

        protected override void Drop(SelectExitEventArgs args)
        {
            base.Drop(args);
            Weapon.ClearGripHand(args);
        }

        public void Vibrate()
        {
            Controller.inputDevice.SendHapticImpulse(0, 1f);
        }

        public bool SwitchFireMode(XRController controller)
        {
            if (!controller) return false;
            controller.inputDevice.IsPressed(InputHelpers.Button.SecondaryButton, out _secondaryButton);
            if (_secondaryButton == false) Weapon.Switched = false;
            return _secondaryButton;
        }
    }
}
