
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace LoliPoliceDepartment.Utilities.ProtectedAvatarPedistal
{
    public class ProtectedAvatarPedestal : UdonSharpBehaviour
    {
        VRC_AvatarPedestal avatarPedestal;
        private void Start()
        {
            avatarPedestal = (VRC_AvatarPedestal)GetComponent(typeof(VRC_AvatarPedestal));
        }
        public override void Interact()
        {
            avatarPedestal.SetAvatarUse(Networking.LocalPlayer);
            Networking.LocalPlayer.UseAttachedStation();
        }
    }
}