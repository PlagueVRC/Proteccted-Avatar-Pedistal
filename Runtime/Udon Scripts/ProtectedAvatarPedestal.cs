// Protected avatar pedestal is a prefab for automatically decrypting avatars using GTAvacrypt through an avatar pedestal.
// Copyright (C) 2022 Adam Thomas
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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