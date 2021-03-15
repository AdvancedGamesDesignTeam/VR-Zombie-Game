using UnityEngine.XR.Interaction.Toolkit;

namespace WeaponInteract
{
    public class HandguardInteract : HandHold
    {
        protected override void BeginAction(ActivateEventArgs args)
        {
            base.BeginAction(args);
        }

        protected override void EndAction(DeactivateEventArgs args)
        {
            base.EndAction(args);
        }

        protected override void Grab(SelectEnterEventArgs args)
        {
            base.Grab(args);
            Weapon.SetGuardHand(args);
            TryHideHand(args.interactor, wasHandHidden);
        }

        protected override void Drop(SelectExitEventArgs args)
        {
            base.Drop(args);
            Weapon.ClearGuardHand();
            TryHideHand(args.interactor, wasHandHidden);
        }
    }
}
