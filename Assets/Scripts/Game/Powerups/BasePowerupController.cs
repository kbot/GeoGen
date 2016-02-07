using UnityEngine;
using System.Collections;

public class BasePowerupController : MonoBehaviour
{
	public GameObject particleRing;
	public float fRotationsPerSecond;

	public Light powerupGlow;

	public Color powerupColor;
	public float fEmissionIntesityOveride;
	private Color emissiveModifier;

	//In order tiers for powerup - 0 = lower tier
	public GameObject[] powerupIconsTiers;
	private GameObject currentPowerupTierPrefab;

	//public tier for testing
	public int nCurrentTier;
	public float fTierDurationSeconds;

	// Use this for initialization
	void Start ()
	{
		emissiveModifier = Color.white - powerupColor;
		powerupGlow.color = powerupColor;
		particleRing.GetComponent<Renderer>().material.SetColor("_Color",powerupColor);
		particleRing.GetComponent<Renderer>().material.SetColor("_EmissionColor",Color.white * fEmissionIntesityOveride - emissiveModifier);
		int initialTier = Random.Range (0, powerupIconsTiers.Length);
		changeTier (initialTier);
		StartCoroutine (downgradeTierCoRoutine ());
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		float fSinOffset = Mathf.Sin (Time.time) * Time.smoothDeltaTime;
		float fSinOffsetWithDelay = Mathf.Sin (Time.time) * Time.smoothDeltaTime * 0.8f;
		//move the ring at the opposite sin of the glow
		particleRing.transform.Translate(Vector3.up * -fSinOffset * 0.15f, transform);
		//move the powerup icon the same as the ring but wil a slight lag
		currentPowerupTierPrefab.transform.Translate(Vector3.up * -fSinOffsetWithDelay * 0.15f, transform);

		//rotate the ring
		float fOrbitDegrees = 360 * fRotationsPerSecond * Time.smoothDeltaTime;
		particleRing.transform.Rotate (Vector3.forward, fOrbitDegrees);
		//rotate powerup icon opposite direction
		currentPowerupTierPrefab.transform.Rotate (Vector3.up, fOrbitDegrees);

		powerupGlow.intensity += fSinOffset * 2.0f;
		powerupGlow.transform.position += Vector3.up * 0.25f * fSinOffset;
	}

	public void changeTier(int nTier) {
		if (nTier > powerupIconsTiers.Length) {
			Debug.Log("Tier greater than prefab list");
			return;
		}
		nCurrentTier = nTier;
		if (currentPowerupTierPrefab) {
			GameObject.Destroy (currentPowerupTierPrefab);
		}
		//instantiate and set parent
		currentPowerupTierPrefab = GameObject.Instantiate (powerupIconsTiers [nCurrentTier]);
		currentPowerupTierPrefab.transform.parent = transform;
		currentPowerupTierPrefab.transform.localPosition = Vector3.zero;
	}

	IEnumerator downgradeTierCoRoutine() {
		while (nCurrentTier > 0) {
			yield return new WaitForSeconds (fTierDurationSeconds);
			changeTier (nCurrentTier - 1);
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
			//collided with player, apply effect destroy self
			//start shrink anim
			StartCoroutine(shrinkAfterCollisionTriggerCoRoutine());
			//stop downgrade
			StopCoroutine(downgradeTierCoRoutine());
		}
	}
		
	IEnumerator shrinkAfterCollisionTriggerCoRoutine() {
		float lerpTime = 0.0f;
		while (transform.localScale.magnitude > 0) {
			yield return new WaitForFixedUpdate ();
			lerpTime += Time.smoothDeltaTime;
			transform.localScale = Vector3.one * Mathf.SmoothStep(1.0f,0.0f,lerpTime * 2.5f);
			powerupGlow.range -= Mathf.SmoothStep(0.0f,1.0f,lerpTime * 0.5f);
			powerupGlow.intensity -= Mathf.SmoothStep(0.0f,1.0f,lerpTime);
		}
		Destroy(this.gameObject);
	}
}

