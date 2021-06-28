using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour
{
	//初期位置
	private Vector3 startPosition;
	//目的地
	private Vector3 destination;

	void Start()
	{
		//　初期位置を設定
		startPosition = transform.position;
		SetDestination(transform.position);
	}

	//　ランダムな位置の作成
	public void CreateRandomPosition()
	{
		//　ランダムなVector2の値を得る
		var randDestination = Random.insideUnitCircle * 8;
		//　現在地にランダムな位置を足して目的地とする
		SetDestination(startPosition + new Vector3(randDestination.x, 0, randDestination.y));
	}

	//　目的地を設定する
	public void SetDestination(Vector3 position)
	{
		destination = position;
	}

	//　目的地を取得する
	public Vector3 GetDestination()
	{
		return destination;
	}
}