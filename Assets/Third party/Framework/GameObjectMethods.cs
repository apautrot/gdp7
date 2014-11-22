using UnityEngine;
using System.Collections.Generic;

public enum GetChildOption
{
	ChildOnly,
	FullHierarchy
}

public enum GetBoundsOption
{
	ChildOnly,
	FullHierarchy
}

public static class GameObjects
{
	public static void BroadcastMessageToScene ( string messageName, System.Object messageParameter = null )
	{
		GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType ( typeof ( GameObject ) );
		foreach ( GameObject go in gos )
		{
			if ( go && go.transform.parent == null )
			{
				go.gameObject.BroadcastMessage ( messageName, messageParameter, SendMessageOptions.DontRequireReceiver );
			}
		}
	}
}

public static class GameObjectExtensions
{
	public static void MoveChildTo ( this GameObject self, GameObject to )
	{
		List<Transform> childs = new List<Transform> ();
		for ( int i = 0; i < self.transform.childCount; i++ )
			childs.Add ( self.transform.GetChild ( i ) );

		foreach ( Transform child in childs )
			child.parent = to.transform;
	}

	public static T GetOrCreateComponent<T> ( this GameObject gameObject ) where T : Component
	{
		T component = gameObject.GetComponent<T> ();
		if ( component == null )
			component = gameObject.AddComponent<T> ();

		return component;
	}

	public static T GetComponentOfChildUnderPoint<T> ( this GameObject gameObject, Vector2 point ) where T : MonoBehaviour
	{
		GameObject go = gameObject.GetChildUnderPoint ( point );
		if ( go != null )
			return go.GetComponent<T> ();
		else
			return null;
	}

	public static GameObject GetChildUnderPoint ( this GameObject gameObject, Vector2 point )
	{
		int count = gameObject.transform.childCount;
		for ( int i = 0; i < count; i++ )
		{
			GameObject child = gameObject.transform.GetChild ( i ).gameObject;
			BoxCollider2D boxCollider2D = child.GetComponent<BoxCollider2D> ();
			if ( boxCollider2D != null )
			{
				Rect r = RectUtil.FromCollider ( boxCollider2D );
				// Debug.Log ( "Rect : " + r.ToString() + " - Point : " + point.ToString() );
				if ( r.Contains ( point ) )
					return child;
			}
		}

		return null;
	}

	public static bool IsUnderPoint ( this GameObject gameObject, Vector2 point )
	{
		BoxCollider2D boxCollider2D = gameObject.GetComponent<BoxCollider2D> ();
		if ( boxCollider2D != null )
		{
			Rect r = RectUtil.FromCollider ( boxCollider2D );
			return r.Contains ( point );
		}

		CircleCollider2D circleCollider2D = gameObject.GetComponent<CircleCollider2D> ();
		if ( circleCollider2D != null )
		{
			Vector2 dist = ( (Vector2)circleCollider2D.transform.position + circleCollider2D.center ) - point;
			return ( dist.magnitude < circleCollider2D.radius );
		}

		Debug.LogWarning ( "IsUnderPoint : No collider on object " + gameObject.name );
		return false;
	}

	public static bool IsUnderMouse ( this GameObject gameObject )
	{
		int count = Camera.allCameras.Length;
		for ( int i = 0; i < count; i++ )
		{
			Camera camera = Camera.allCameras[i];
			int layerMask = 1 << gameObject.layer;
			if ( ( layerMask & camera.cullingMask ) != 0 )
				return gameObject.IsUnderPoint ( camera.ScreenToWorldPoint ( Input.mousePosition ) );
		}

		return false;
	}

	public static Bounds GetBounds ( this GameObject gameObject, GetBoundsOption option = GetBoundsOption.ChildOnly )
	{
		if ( option == GetBoundsOption.ChildOnly )
		{
			if ( gameObject.renderer != null )
				return gameObject.renderer.bounds;
			else
				return new Bounds ();
		}
		else
		{
			Bounds b = gameObject.GetChildBounds ();
			if ( gameObject.renderer != null )
				b.Encapsulate ( gameObject.renderer.bounds );

			return b;
		}
	}

	public static Bounds GetChildBounds ( this GameObject gameObject )
	{
		Bounds bounds = new Bounds ();
		if ( gameObject.transform.childCount > 0 )
		foreach ( Transform t in gameObject.transform )
		{
			Bounds childBound = t.gameObject.GetBounds ( GetBoundsOption.FullHierarchy );
			if ( childBound.size != Vector3.zero )
				if ( bounds.size == Vector3.zero )
					bounds = childBound;
				else
					bounds.Encapsulate ( childBound );
		}

		if ( bounds.size == Vector3.zero )
			return new Bounds ( gameObject.transform.position, Vector3.zero );

		return bounds;
	}

// 	public static Vector3 GetChildBounds ( this GameObject gameObject )
// 	{
// 		Vector3 max = new Vector3 ( float.MinValue, float.MinValue, float.MinValue );
// 		Vector3 min = new Vector3 ( float.MaxValue, float.MaxValue, float.MaxValue );
// 
// 		int count = gameObject.transform.childCount;
// 		for ( int i = 0; i < count; i++ )
// 		{
// 			GameObject child = gameObject.transform.GetChild ( i ).gameObject;
// 			BoxCollider2D boxCollider2D = child.GetComponent<BoxCollider2D> ();
// 			if ( boxCollider2D != null )
// 			{
// 				Vector3 position = child.transform.localPosition;
// 				float left = position.x - ( boxCollider2D.size.x / 2 ) + boxCollider2D.center.x;
// 				float right = position.x + ( boxCollider2D.size.x / 2 ) + boxCollider2D.center.x;
// 				float bottom = position.y - ( boxCollider2D.size.y / 2 ) + boxCollider2D.center.y;
// 				float top = position.y + ( boxCollider2D.size.y / 2 ) + boxCollider2D.center.y;
// 
// 				min.x = System.Math.Min ( min.x, left );
// 				max.x = System.Math.Max ( max.x, right );
// 				min.y = System.Math.Min ( min.y, bottom );
// 				max.y = System.Math.Max ( max.y, top );
// 			}
// 		}
// 
// 		return max - min;
// 	}

	public static List<GameObject> GetChilds<T> ( this GameObject gameObject, GetChildOption option = GetChildOption.ChildOnly ) where T : MonoBehaviour
	{
		List<GameObject> childs = new List<GameObject> ();
		int count = gameObject.transform.childCount;
		for ( int i = 0; i < count; i++ )
		{
			GameObject child = gameObject.transform.GetChild ( i ).gameObject;
			if ( child.GetComponent<T>() != null )
			{
				childs.Add ( child );
				if ( option == GetChildOption.FullHierarchy )
					childs.AddRange ( child.GetChilds ( option ) );
			}
		}
		return childs;
	}

	public static List<GameObject> GetChilds ( this GameObject gameObject, GetChildOption option = GetChildOption.ChildOnly )
	{
		List<GameObject> childs = new List<GameObject> ();
		int count = gameObject.transform.childCount;
		for ( int i = 0; i < count; i++ )
		{
			GameObject child = gameObject.transform.GetChild ( i ).gameObject;
			childs.Add ( child );
			if ( option == GetChildOption.FullHierarchy )
				childs.AddRange ( child.GetChilds ( option ) );
		}
		return childs;
	}

	public static GameObject FindChildByName ( this GameObject gameObject, string name )
	{
		int count = gameObject.transform.childCount;
		for ( int i = 0; i < count; i++ )
		{
			GameObject child = gameObject.transform.GetChild ( i ).gameObject;
			if ( child.name.Equals ( name ) )
				return child;
		}
		return null;
	}

	public static List<GameObject> FindChildsByName ( this GameObject gameObject, string name )
	{
		List<GameObject> list = new List<GameObject> ();
		int count = gameObject.transform.childCount;
		for ( int i = 0; i < count; i++ )
		{
			GameObject child = gameObject.transform.GetChild ( i ).gameObject;
			if ( child.name.Equals ( name ) )
				list.Add ( child );
		}
		return list;
	}

	public static void SetPosition ( this GameObject gameObject, float x, float y )
	{
		gameObject.transform.position = new Vector3 ( x, y, gameObject.transform.position.z );
	}

	public static GameObject GetRootParent ( this GameObject self )
	{
		Transform parent = self.transform.parent;
		
		if ( parent == null )
			return null;
	
		while ( parent.transform.parent != null )
			parent = parent.transform.parent;

		return parent.gameObject;
	}

// 	public static void SetAlpha ( this GameObject self, float alpha )
// 	{
// 		SpriteRenderer sr = self.GetComponent<SpriteRenderer> ();
// 		if ( sr != null )
// 			sr.color = ColorUtils.fromColor ( sr.color, alpha );
// 	}

	public static void SetScale ( this GameObject self, float scale )
	{
		self.transform.localScale = new Vector3 ( scale, scale, scale );
	}

	public static void SetAlpha ( this GameObject self, float alpha, bool recursiveOnChildren = false )
	{
		if ( self.renderer != null )
			if ( self.renderer.material != null )
				self.renderer.material.SetAlpha ( alpha );

		if ( recursiveOnChildren )
			for ( int i = 0; i < self.transform.childCount; i++ )
				self.transform.GetChild ( i ).gameObject.SetAlpha ( alpha, recursiveOnChildren );
	}

	public static void SetColor ( this GameObject self, Color color, bool recursiveOnChildren = false )
	{
		if ( self.renderer != null )
			if ( self.renderer.material != null )
				self.renderer.material.SetColor ( color );

		if ( recursiveOnChildren )
			for ( int i = 0; i < self.transform.childCount; i++ )
				self.transform.GetChild ( i ).gameObject.SetColor ( color, recursiveOnChildren );
	}

	public static AbstractGoTween colorTo ( this GameObject self, float duration, Color endValue, GoEaseType easeType = GoEaseType.Linear )
	{
		List<GameObject> all = self.GetChilds ( GetChildOption.FullHierarchy );
		all.Add ( self );
		AbstractGoTween tween = null;
		GoTweenFlow tweens = null;

		foreach ( var v in all )
		{
			if ( v.renderer != null )
				if ( v.renderer.material != null )
				{
					if ( tween != null )
						if ( tweens == null )
						{
							tweens = new GoTweenFlow ();
							tweens.insert ( 0, tween );
						}

					tween = v.renderer.material.colorTo ( duration, endValue );
					if ( tweens != null )
						tweens.insert ( 0, tween );
				}
		}

		if ( tweens != null )
		{
			Go.addTween ( tweens );
			return tweens;
		}
		else
			return tween;
	}

	public static AbstractGoTween alphaTween ( this GameObject self, bool directionTo, float duration, float endValue, GoEaseType easeType, float delay )
	{
		List<GameObject> all = self.GetChilds ( GetChildOption.FullHierarchy );
		all.Add ( self );
		AbstractGoTween tween = null;
		GoTweenFlow tweens = null;

		foreach ( var v in all )
		{
			if ( v.renderer != null )
				if ( v.renderer.material != null )
				{
					if ( tween != null )
						if ( tweens == null )
						{
							tweens = new GoTweenFlow ();
							tweens.insert ( 0, tween );
						}

					if ( directionTo )
						tween = v.renderer.material.alphaTo ( duration, endValue ).delays ( delay );
					else
						tween = v.renderer.material.alphaFrom ( duration, endValue ).delays ( delay );

					if ( tweens != null )
						tweens.insert ( 0, tween );
				}
		}

		if ( tweens != null )
		{
			Go.addTween ( tweens );
			return tweens;
		}
		else
			return tween;
	}

	public static AbstractGoTween alphaFrom ( this GameObject self, float duration, float endValue, GoEaseType easeType = GoEaseType.Linear, float delay = 0 )
	{
		return self.alphaTween ( false, duration, endValue, easeType, delay );
	}

	public static AbstractGoTween alphaTo ( this GameObject self, float duration, float endValue, GoEaseType easeType = GoEaseType.Linear, float delay = 0 )
	{
		return self.alphaTween ( true, duration, endValue, easeType, delay );
	}

	public static void DeleteAllChilds ( this GameObject self )
	{
		int count = self.transform.childCount;
		for ( int i = 0; i < count; i++ )
		{
			GameObject child = self.transform.GetChild ( i ).gameObject;
			GameObject.Destroy ( child );
		}
	}

	public static void DeleteAllChilds<T> ( this GameObject self ) where T : MonoBehaviour
	{
		List<GameObject> list = self.GetChilds<T>();
		for ( int i = 0; i < list.Count; i++ )
			GameObject.Destroy ( list[i] );
	}

	public static bool DeleteChilds ( this GameObject self, string name )
	{
		List<GameObject> childs = self.FindChildsByName ( name );
		foreach ( GameObject child in childs )
			GameObject.Destroy ( child );

		return ( childs.Count > 0 );
	}

	public static bool DeleteChild ( this GameObject self, string name )
	{
		GameObject child = self.FindChildByName ( name );
		if ( child != null )
		{
			GameObject.Destroy ( child );
			return true;
		}
		else
			return false;
	}

	public static GameObject Instantiate ( GameObject prefab, Vector3 position, string name = null )
	{
		GameObject go = GameObject.Instantiate ( prefab ) as GameObject;
		go.transform.position = position;
		if ( name != null )
			go.name = name;
		return go;
	}

	public static GameObject Instantiate ( this GameObject self, GameObject prefab, Vector3 position, string name = null )
	{
		GameObject child = GameObject.Instantiate ( prefab ) as GameObject;
		child.transform.localPosition = self.transform.position + position;
		if ( name != null )
			child.name = name;
		return child;
	}

	public static SpriteRenderer InstantiateSprite ( this GameObject self, Sprite sprite, Vector3 position, string name = null )
	{
		GameObject child = new GameObject();
		child.transform.parent = self.transform;
		child.transform.localPosition = position;
		if ( name != null )
			child.name = name;
		SpriteRenderer sr = child.GetOrCreateComponent<SpriteRenderer> ();
		if ( sprite == null )
			Debug.LogWarning ( "InstantiateSprite : Sprite parameter is null" );
		sr.sprite = sprite;
		return sr;
	}

	public static GameObject InstantiateChild ( this GameObject self, GameObject prefab, Vector3 position, string name = null )
	{
		GameObject child = GameObject.Instantiate ( prefab ) as GameObject;
		child.transform.parent = self.transform;
		child.transform.localPosition = position;
		if ( name != null )
			child.name = name;
		return child;
	}

	public static GameObject InstantiateReplace ( this GameObject self, GameObject prefab )
	{
		GameObject replacer = GameObject.Instantiate ( prefab ) as GameObject;
		replacer.transform.parent = self.transform.parent;
		replacer.transform.localPosition = self.transform.localPosition;
		replacer.transform.localRotation = self.transform.localRotation;
		replacer.transform.localScale = self.transform.localScale;
		replacer.name = self.name;
		GameObject.Destroy ( self );
		return replacer;
	}

	public static void DestroySelf ( this GameObject self )
	{
		GameObject.Destroy ( self );
	}

	public static bool SendMessageToChild ( this GameObject self, string childName, string message )
	{
		GameObject child = self.FindChildByName ( childName );
		if ( child != null )
		{
			child.SendMessage ( message );
			return true;
		}
		else return false;
	}

	public static void WaitAndDo ( this MonoBehaviour self, float duration, System.Action action )
	{
		self.StartCoroutine ( WaitDoCoroutine ( duration, action ) );
	}

	private static System.Collections.IEnumerator WaitDoCoroutine ( float duration, System.Action action )
	{
		yield return new WaitForSeconds ( duration );
		action ();
		yield break;
	}
}


