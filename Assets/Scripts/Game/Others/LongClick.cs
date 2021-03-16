using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/*zz
public class LongClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

	private bool pointerDown;
	private float pointerDownTimer;

	[SerializeField]
	private float requiredHoldTime;
	[SerializeField]
	private PlayerController playerC;
	public UnityEvent onLongClick;


	public void OnPointerDown(PointerEventData eventData)
	{
		pointerDown = true;
		playerC.PlayerJump(true);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Reset();
		playerC.PlayerJump(false);
	}

	private void Update()
	{
		if (pointerDown)
		{
			pointerDownTimer += Time.deltaTime;
			if (pointerDownTimer >= requiredHoldTime)
			{
				if (onLongClick != null)
					onLongClick.Invoke();

				Reset();
			}
		}
	}

	private void Reset()
	{
		pointerDown = false;
		pointerDownTimer = 0;
		playerC.PlayerJump(false);
	}

}
*/