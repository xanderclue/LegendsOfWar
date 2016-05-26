Bullet Creator 2.0 by Pixelatto
=============================
Bullet Creator hels you create advanced weapon/bullet behaviours without code.
You can make any kind of weapon with it: shotguns, framethrowers, rifles, pistols, and whatever you came up with.

Features:
 	- Create gunshots for guns, grenade launchers, flamethrowers, shotguns, rifles, machineguns or whatever you imagine.
	- Trajectory realtime preview: see how will be your weapon shoots without even running your game.
	- Fully customizable: Bullets per shot, Shot speed, Rate of fire, Clip size, Reload Time, Shoot shape, Gravity & Wind and more.
	- No scripting needed: Just drag the component over your weapon model and start customizing how it shoots.
	- Scene view integrated editor: Drag the values directly in the in-scene graphic inspector.
	- Sound support: add syncronised sounds to the shoot, reload, etc...
	- Advanced editable gizmos: Realtime ammo & reloading scene display and handles.
	- Unity physics compatible: Perfect continous collision detection for bullets at even infinite speed. No pass-thru-wall even at high speeds.
	- Source code included
	- 100% compatible with Unity 5

DEMO SCENE CONTENTS:
CCD & FastBullets Demo - A simple demo scene showing the continous collision detection in a FPS game

QUICK START GUIDE:
1. Import Bullet Creator package into your project
2. Drag the BulletCreatorWeapon prefab as a child of your weapon model GameObject
3. Adjust the position and rotation of this prefab transform so the arrow gizmo points right out of the weapon cannon.
4. Adjust the settings to your needs
5. You can test the weapon behabiour by checking "Autofire Test" in the inspector
6. You can fire the weapon by assigning a game input axis (any button) from wherever you want or by calling the Shoot() method from scripts.

FIRING FROM SCRIPT (EXAMPLE):
public class SomeObject : MonoBehaviour {
	public GameObject BcWeaponObject; //(Don't forget to assign the BcWeaponObject variable to the BcWeapon Object in the inspector window)
	void Update() {
		BcWeaponObject.GetComponent<BcWeapon>().Shoot();
	}
}
//NOTE: Calling the Shoot() method is equivalent to pulling the trigger of a gun: if there is no bullet loaded the shoot will not fire until its ready.
//This keeps the rate of fire coherent and also handles the reloading.
//If you want to override this and force bullets to spawn use the SpawnBullet() method instead.

TIPS:
- The "ruler" gizmo near the weapon shows how many bullets are left before reloading and the firing/reloading times. You can edit it's size to adjust the capacity/rof of the magazine.
- Experiment! You can add more BcWeapon components to the same weapon if you want multiple-shoot-type guns, i.e: a machinegun with a grenade launcher.

Get more assets at: www.pixelatto.com

If you need any help: support@pixelatto.com