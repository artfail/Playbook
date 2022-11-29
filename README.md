# Playbook
This is an example of moving, rotating, and scaling an object in 3D based on mouse dragging in 2D. Dragging the mouse is done via a ScreenPointToRay racast and the object's transform vectors are translated back using ProjectOnPlane and SignedAngle along with some Trig. This repo was originally created as a programming exercise for Playbook.

All code and implementation in Unity was developed by John Bruneau and is free to use with Attribution.
Attribution 3.0 Unported (CC BY 3.0) (https://creativecommons.org/licenses/by/3.0/)

To Run:
Download the repo and open the root folder in Unity. In Unity, open the "Main" scene under Assets/Scenes.

Instructions:
Click the Orange 3D button to generate a Cube. The Cube has 3 handles for each axis. Cone: Position, Sphere: Scale, Ring: Rotate. The handles can be dragged with the mouse to manipulate their respective transform property.
