using UnityEngine;
using System.Collections;

public class AI_Seeker : AI_Behavior {

	public GameObject target;


//	public override void InitAI () {
//		if(target == null)
//		{
//			Debug.LogError("No Target Set for AI_SEEKER");
//		}
//	}
//	public override void UpdateAI () {
//
//	}
//	public override void FixedUpdateAI () {
//
//	}
//	public override void ExitAI (){
//
//	}

	public Vector3 getVectorToTarget() {
		return target.transform.position - transform.position;
	}
}
