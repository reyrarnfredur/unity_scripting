// DrawLineFromTo
using UnityEngine;
using UnityEngine.Rendering;

public class DrawLineFromTo : MonoBehaviour
{
	private Transform myTransform;

	private LineRenderer myLineRenderer;

	public Transform fromTransform;

	public Transform toTransform;

	[Header("line settings")]
	public Material lineMaterial;

	public float lineWidth;

	public int numCapVertices;

	[Header("shadows")]
	public bool castShadows;

	public bool receiveShadows;

	private void Start()
	{
		if (fromTransform != null && toTransform != null)
		{
			myTransform = base.transform;
			GameObject gameObject = new GameObject("lineFromToObject");
			Transform transform = gameObject.transform;
			transform.parent = myTransform;

            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;

			if (!(gameObject != null))
			{
				return;
			}
			myLineRenderer = gameObject.AddComponent<LineRenderer>();
			if (myLineRenderer != null)
			{
				myLineRenderer.positionCount = 2;
				myLineRenderer.startWidth = lineWidth;
				myLineRenderer.endWidth = lineWidth;
				myLineRenderer.material = lineMaterial;
				if (numCapVertices > 0)
				{
					myLineRenderer.numCapVertices = numCapVertices;
				}
				myLineRenderer.receiveShadows = receiveShadows;
				myLineRenderer.shadowCastingMode = (castShadows ? ShadowCastingMode.TwoSided : ShadowCastingMode.Off);
			}
		}
		else
		{
			Object.Destroy(this);
		}
	}

	private void FixedUpdate()
	{
		if (myLineRenderer != null)
		{
			myLineRenderer.SetPosition(0, fromTransform.position);
			myLineRenderer.SetPosition(1, toTransform.position);
		}
	}
}
