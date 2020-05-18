using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;		
	[HideInInspector]
	public bool jump;				    

	public float moveForce = 365f;		
	public float maxSpeed = 5f;			
	public AudioClip[] jumpClips;		
	public float jumpForce = 1000f;		
	public AudioClip[] taunts;			
	public float tauntProbability = 50f;
	public float tauntDelay = 1f;		

	private int tauntIndex;				
	private Transform groundCheck;		
	private bool grounded = false;		
	private Animator anim;				

    protected float tilt;
    private readonly List<KeyCode> actions = new List<KeyCode>();
    private Transform pivot;

    public Gun gun;
    public bool IAmAnEnemy;

    [HideInInspector]
    public bool HasTurn;

	private void Awake()
	{
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();

        pivot = transform.Find("Pivot");
    }

	private void Update()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (!HasTurn)
            return;

        if (!IAmAnEnemy)
        {
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
                jump = true;
            
            if (Input.GetMouseButton(0))
                gun.FireDown();
            else if (Input.GetMouseButtonUp(0))
                gun.FireUp();


            ActualizarAccionTeclado(KeyCode.LeftArrow);
            ActualizarAccionTeclado(KeyCode.RightArrow);
            ActualizarAccionTeclado(KeyCode.UpArrow);
            ActualizarAccionTeclado(KeyCode.DownArrow);
            ActualizarAccionTeclado(KeyCode.W);
            ActualizarAccionTeclado(KeyCode.S);
            // NEW
        }
    }

    private void FixedUpdate()
	{
        if (!HasTurn)
            return;

        var h = Input.GetAxis("Horizontal");

        if (h == 0)
        {
            if (actions.Contains(KeyCode.LeftArrow))
                h = -1;

            if (actions.Contains(KeyCode.RightArrow))
                h = 1;
        }
        if (actions.Contains(KeyCode.UpArrow) || actions.Contains(KeyCode.W))
            tilt += 1.0f;
        if (actions.Contains(KeyCode.DownArrow) || actions.Contains(KeyCode.S))
            tilt -= 1.0f;

        tilt = Mathf.Clamp(tilt, 0, 75);
        pivot.rotation = Quaternion.Euler(0, 0, facingRight? tilt : -tilt);

        if (!IAmAnEnemy)
        {
            anim.SetFloat("Speed", Mathf.Abs(h));

            if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);
            }

            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
                GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }

        if (!IAmAnEnemy)
        {
            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();
        }

        if (!IAmAnEnemy)
        {
            if (jump)
            {
                anim.SetTrigger("Jump");

                int i = Random.Range(0, jumpClips.Length);

                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

                jump = false;
            }
        }
	}

	protected void Flip ()
	{
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }

	public IEnumerator Taunt()
	{
		float tauntChance = Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			yield return new WaitForSeconds(tauntDelay);

			if(!GetComponent<AudioSource>().isPlaying)
			{
				tauntIndex = TauntRandom();

				GetComponent<AudioSource>().clip = taunts[tauntIndex];
				GetComponent<AudioSource>().Play();
			}
		}
	}

	int TauntRandom()
	{
		int i = Random.Range(0, taunts.Length);

		if(i == tauntIndex)
			return TauntRandom();
		else
			return i;
	}

    public void Jump()
    {
        jump = true;
    }

    #region InputControls
    private void ActualizarAccionDown(KeyCode code)
    {
        if (!actions.Contains(code))
            actions.Add(code);
    }

    private void ActualizarAccionUp(KeyCode code)
    {
        if (actions.Contains(code))
            actions.Remove(code);
    }

    private void ActualizarAccionTeclado(KeyCode code)
    {
        if (Input.GetKeyDown(code))
            ActualizarAccionDown(code);

        if (Input.GetKeyUp(code))
            ActualizarAccionUp(code);
    }

    #region Pressed Buttons
    public void MueveDerechaDown()
    {
        ActualizarAccionDown(KeyCode.RightArrow);
    }
    public void MueveIzquierdaDown()
    {
        ActualizarAccionDown(KeyCode.LeftArrow);
    }
    public void RotaDerechaDown()
    {
        ActualizarAccionDown(KeyCode.DownArrow);
    }
    public void RotaIzquierdaDown()
    {
        ActualizarAccionDown(KeyCode.UpArrow);
    }
    #endregion

    #region Released Buttons
    public void MueveDerechaUp()
    {
        ActualizarAccionUp(KeyCode.RightArrow);
    }
    public void MueveIzquierdaUp()
    {
        ActualizarAccionUp(KeyCode.LeftArrow);
    }
    public void RotaDerechaUp()
    {
        ActualizarAccionUp(KeyCode.DownArrow);
    }
    public void RotaIzquierdaUp()
    {
        ActualizarAccionUp(KeyCode.UpArrow);
    }
    #endregion
    #endregion

}
