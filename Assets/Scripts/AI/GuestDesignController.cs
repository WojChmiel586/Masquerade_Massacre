using System.Collections.Generic;
using UnityEngine;

public class GuestDesignController : MonoBehaviour
{
	[ Header( "Guest Elements" ) ]
	[ SerializeField ] SpriteRenderer m_Mask;
	[ SerializeField ] SpriteRenderer m_Body;
	[ SerializeField ] SpriteRenderer m_Hands;
	Color m_MaskColour;
	Color m_TrimColour;

	void Start()
	{

	}
	
	// Update is called once per frame
	void Update()
	{

	}

	public void SetGuestElements( Sprite xMask, Sprite xBody, Color xMaskColor, Color xTrim )
	{
		m_Mask.sprite = xMask;
		m_Body.sprite = xBody;
		m_Mask.color = xMaskColor;
	}
}
