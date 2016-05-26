using UnityEngine;
using System.Collections;

public class CasterE : AbilityEBase
{
	[SerializeField]
	GameObject m_Burn = null;
	[SerializeField]
	GameObject m_targetingSystem = null;
	ParticleSystem m_targetingEffect = null;
	RaycastHit m_targetHit;

	protected override void Start()
	{
		base.Start();
		m_Burn.GetComponent<ParticleSystem>().Stop();
		m_Burn.GetComponent<ParticleSystem>().Clear();
		m_targetingEffect = m_targetingSystem.GetComponent<ParticleSystem>();
		m_targetingEffect.Stop();
		m_targetingEffect.Clear();
	}
	void FixedUpdate()
	{
		if ( aimingSkill )
		{
			if ( Physics.SphereCast( transform.parent.position, 5.0f, transform.forward, out m_targetHit, 150.0f, 1 ) )
			{
				if ( m_targetHit.collider.gameObject.GetComponentInParent<Info>().team == Team.RED_TEAM )
				{
					m_targetingSystem.transform.position = m_targetHit.collider.gameObject.transform.position;
					m_targetingEffect.Play();
				}
				else if ( m_targetingEffect.isPlaying )
					ResetSystem();
			}
			else if ( m_targetingEffect.isPlaying )
				ResetSystem();
		}
		else if ( m_targetingEffect.isPlaying )
			ResetSystem();

	}

	void ResetSystem()
	{
		if ( m_targetingEffect.isPlaying )
			m_targetingEffect.Stop();
		m_targetingEffect.Clear();
		m_targetingSystem.transform.localPosition = Vector3.zero;
	}

	protected override void Update()
	{

		if ( Input.GetMouseButtonDown( 0 ) && aimingSkill )
		{
			aimingSkill = false;
			TryCast();
			ResetSystem();
		}
		else if ( Input.GetMouseButtonDown( 1 ) && aimingSkill )
		{
			aimingSkill = false;
		}
		else if ( ( ( Input.GetKeyDown( KeyCode.E ) && !HeroCamScript.onHero ) ||
				   Input.GetKeyDown( KeyCode.Alpha3 ) ||
		Input.GetKeyDown( KeyCode.Keypad3 ) ) && !aimingSkill && cooldownTimer <= 0.0f )
			aimingSkill = true;
		// <BUGFIX: Test Team #32>
		if ( !EnoughMana )
			aimingSkill = false;
		// </BUGFIX: Test Team #32>
	}

	protected override void AbilityActivate()
	{
		if ( m_targetHit.collider != null )
		{
			GameObject tmp = Instantiate( m_Burn, m_targetHit.transform.position, m_targetHit.transform.rotation ) as GameObject;
			tmp.GetComponent<CasterEBehavior>().Activate = true;
			tmp.transform.parent = m_targetHit.transform;
			StatusEffects.Inflict( m_targetHit.collider.gameObject, Effect.CreateEffect() );
			base.AbilityActivate();
		}
	}
}
