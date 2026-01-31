using System.Collections.Generic;
using UnityEngine;

public class GuestDesignController : MonoBehaviour
{
	[ Header( "Guest Elements" ) ]
	[ SerializeField ] SpriteRenderer m_Mask;
	[ SerializeField ] SpriteRenderer m_Trim;
	[ SerializeField ] SpriteRenderer m_Body;
	[ SerializeField ] SpriteRenderer m_HandL;
	[ SerializeField ] SpriteRenderer m_HandR;
	Color m_MaskColour;
	Color m_TrimColour;

	void Start()
	{

	}
	
	// Update is called once per frame
	void Update()
	{

	}

	public void SetGuestElements( Sprite xMask, Sprite xBody, Sprite xHandL, Sprite xHandR, Color xMaskColor, Color xTrim )
	{
		m_Mask.sprite = xMask;
		m_Trim.sprite = xMask;
		m_Body.sprite = xBody;
		m_HandL.sprite = xHandL;
		m_HandR.sprite = xHandR;
		m_Mask.color = xMaskColor;
		m_Trim.color = xTrim;
	}

	public bool CompareFeatures( Sprite xMask, Sprite xBody,Color xMaskColor, Color xTrim )
	{
		if ( m_Mask.sprite == xMask &&
		m_Trim.sprite == xMask &&
		m_Body.sprite == xBody &&
		m_Mask.color == xMaskColor &&
		m_Trim.color == xTrim )
		{
			return true;
		}
		return false;
		
	}
}
