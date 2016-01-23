using UnityEngine;
using System.Collections;

public class GeoGenCharacterController : MonoBehaviour {

	public GameObject playerMesh;
	public GameObject playerRing;

	public IList BulletEmitters;

	//Movement
	public float fMoveSpeedMultiplier;
	float fMaxMoveVelocity = 9.0f;
	float moveSpeed;

	//Jumping
	public float jumpForce = 10.0f;
	bool bGrounded;

	//Weapon
	int nWeaponTier = 0;
	protected GameObject playerWeapon;

	// Use this for initialization
	void Start () {
		bGrounded = true;
		playerWeapon = GGWeaponFactory.Instance.getWeaponAtTier (nWeaponTier);
	}

	void Update () {
		updateFacadeChildObjects ();
	}

	void FixedUpdate () {
	
		//get the camera relative forward
		Transform cameraTransform = Camera.main.transform;
		Vector3 cameraForward = transform.position - cameraTransform.position; 
		cameraForward.y = 0.0f;
		cameraForward.Normalize();
		Vector3 cameraRight = Vector3.Cross (Vector3.up, cameraForward);
		cameraRight.Normalize ();

		//get the force vector from the input
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");
		
		// Target direction relative to the camera
		Vector3 targetDirection = (h * cameraRight + v * cameraForward) * fMoveSpeedMultiplier;
		Rigidbody playerBody = GetComponent<Rigidbody> ();

		if (bGrounded && Input.GetKeyDown(KeyCode.Space)) {
			playerBody.AddForce(Vector3.up * jumpForce);
			bGrounded = false;
		}
		else if (playerBody.GetPointVelocity(playerBody.position).magnitude < fMaxMoveVelocity) {
			//Debug.Log ("PlayerRigidBody Velocity Mag:" + playerBody.GetPointVelocity (playerBody.position).magnitude);
			playerBody.AddForce (targetDirection);
		}
	}

	void updateFacadeChildObjects(){
		//ring
		playerRing.transform.position = transform.position;
		playerRing.transform.Rotate (Vector3.up, 45.0f * Time.deltaTime);

		//currentWeapon
		Vector3 cameraLook = GGGameManager.Instance.mainCamera.GetComponent<KBThirdPersonCamera_Freeform> ().getCameraToTargetVector ();
		cameraLook.y = 0;
		cameraLook.Normalize ();
		playerWeapon.GetComponent<GGPlayerWeapon> ().fireVector = cameraLook;
		playerWeapon.transform.position = transform.position + cameraLook;
	}

	public void upgradeWeapon(){
		playerWeapon.GetComponent<GGPlayerWeapon>().upgradeWeapon ();
		if (playerWeapon.GetComponent<GGPlayerWeapon>().weaponRequiresTierUpgrade()) {
			bool bPreviousAutoFireSetting = playerWeapon.GetComponent<GGPlayerWeapon>().bAutoFire;
			Destroy(playerWeapon);
			nWeaponTier++;
			playerWeapon = GGWeaponFactory.Instance.getWeaponAtTier (nWeaponTier);
			playerWeapon.GetComponent<GGPlayerWeapon>().bAutoFire = bPreviousAutoFireSetting;
		}
	}

	public GGPlayerWeapon playerWeaponComponent {
		get 
		{
			return playerWeapon.GetComponent<GGPlayerWeapon>();
		}
	}
}
