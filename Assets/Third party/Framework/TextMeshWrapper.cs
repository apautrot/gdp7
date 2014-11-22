using UnityEngine;
using System.Collections;

public enum WrapStringOption
{
	WrapBefore,
	WrapAfter
};

public class TextMeshWrapper : MonoBehaviour
{
	public int LineLength = 0;
	public WrapStringOption WrappingMode = WrapStringOption.WrapBefore;

	public void UpdateText ()
	{
	}
}
