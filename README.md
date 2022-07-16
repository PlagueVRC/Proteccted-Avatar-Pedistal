# Protected-Avatar-Pedistal
Protected avatar pedestal is a prefab for automatically decrypting avatars using GTAvacrypt through an avatar pedestal. 
This prefab is specifically for avatars encrypted with GTAvacrypt V2.2 from https://github.com/rygo6/GTAvaCrypt 
If you do not use this system, or you dont plan on letting other people use your avatar then this prefab has no use for you.
This prefab requires UdonSharp and VRChat SDK3 for Worlds. install those before doing anything.

In order for this Prefab to work properly you must install two files from the VRChat SDK3 for Avatars. These files are VRCSDK3A.dll, and
VRCAnimatorTrackingControlEditor.cs These files are not included with this package and you should only get them from the official VRChat website. 
https://vrchat.com/home/download After you install the World SDK, drag the Avatar SDK into your unity project and on the import screen uncheck 
evey file aside from the two previously mentiond

This package can then be directly installed through the unity package manager by adding via git url

![image](https://user-images.githubusercontent.com/429522/179337632-8819db36-c01a-4700-b378-e40d06778d77.png)


After installing, you can find a prefab called ProtectedAvatarPedistal located in Packages > Protected Avatar Pedistal > Runtime

![image](https://user-images.githubusercontent.com/429522/179337667-12640753-4c9c-4d29-8275-c2fb9921a14d.png)


Drag this prefab into the scene. Basically this is just a normal avatar pedistal, and a chair combined. Make sure you unpack the prefab.

![image](https://user-images.githubusercontent.com/429522/179337885-d771c274-b401-4881-aba3-f5a6a2af59ae.png)

Add your avatar ID to the avatar pedestal componenet as usual. For the VRC Station component, you need to make an animatior controller that will
automatically enter the bitkeys from Avacrypt to the avatar. To do this, there is a tool included to automatically generate the controller. 
Open this tool from LPD > Pap Generator on the menu bar at the top of your unity window

![image](https://user-images.githubusercontent.com/429522/179338050-2c5d0d4d-af44-4d87-8b31-3d0f87d17fad.png)

![image](https://user-images.githubusercontent.com/429522/179338351-8d2564be-f6d0-4f96-81e2-39c05f5ef78b.png)


The first time you open the window it will generate a folder to store all of the generated controllers in. 
This is located at Assets/LPD/Protected Avatar Pedistal/Controllers.
Enter a name for the controller you want to generate, typically the name of the avatar it corrisponds to. 
Then select all of the same Bitkey values that you used when encrypting your avatar.

If you want to hide your avatars distorted mesh, there is an optional paramter you can add to your avatar called LoadMesh that you can use to toggle the mesh.
Selecting this option will also add that bool paramter to the controller.

![image](https://user-images.githubusercontent.com/429522/179338495-e5a902ba-4b0b-4833-8d62-9a3f0134d0e1.png)


After you have selected your Bitkeys, and entered a name for the controller, click the Generate new animation controller button to create the controller.
The new controller will be placed in the previously mentioned folder Assets/LPD/Protected Avatar Pedistal/Controllers. 
Drag this controller into the "Animator Controller" slot on the VRC Station componenet on the Protected avatar pedistal.

And your all done! Now when somoene uses this avatar pedistal it will automatically enter the bitkeys for them. You do not have to give the keys out.
Once the avatar is fully decrypted it acts just like it does when you click Write Keys in GTAvaCrypt. This means you can favorite it and the decryption 
will persist between worlds, just make sure you dont reset the avatar.

There are some Quirks of this system to note though. Firstly there isnt a rock solid way for the world to know when you have finished downloading 
and changing into the new avatar when you use the pedistal. So you ahve to wait in the chair untill it has finished and the avatar is completely decrypted.

Secondly The VRC Station will constantly write keys to any avatar that is sitting in it. This means that if you have multiple avatars using this system you can 
accedentilly enter the wrong bitkeys by using a protected avatar pedistal while in a diffrent avatar. in order to keep the correct values saved to your 
vrc parameters, make sure you switch to an avatar that is not using GTAvaCrypt before using a pedistal.
