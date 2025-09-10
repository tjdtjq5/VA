using UnityEngine;


public class GoodsSpawn : UIFrame
{
	private Transform _tr;
	
	private Vector3 _destPos;
	private Vector3 _spreadPos;
	private readonly float _destSpeed = 10f;
	private readonly float _spreadSpeed = 10f;
	
	private bool _isSpreadFlag = false;
	private bool _isInit = false;
	
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }

    public void Initialize(Vector3 spawnPos ,Vector3 destPos, float spreadAngle, float spreadDistance)
    {
	    this._tr = GetImage(UIImageE.Goods).transform;
	    

	    _tr.position = spawnPos;
	    
	    this._destPos = destPos;
	    this._spreadPos = _tr.position.GetAngleMovePos(spreadAngle, spreadDistance);

	    _isSpreadFlag = false;
	    this._isInit = true;
    }

    private void FixedUpdate()
    {
	    if (_isInit)
	    {
		    if (_isSpreadFlag)
		    {
			    _tr.position = Vector3.Lerp(_tr.position, this._destPos, Managers.Time.FixedDeltaTime * _destSpeed);

			    if (_tr.position.GetDistance(_destPos) < 0.1f)
				    Destroy();
		    }
		    else
		    {
			    _tr.position = Vector3.Lerp(_tr.position, this._spreadPos, Managers.Time.FixedDeltaTime * _spreadSpeed);

			    if (_tr.position.GetDistance(_spreadPos) < 0.1f)
				    _isSpreadFlag = true;
		    }
	    }
    }

    void Destroy()
    {
	    _isInit = false;
	    Managers.Resources.Destroy(this.gameObject);
    }

    public enum UIImageE
    {
		Goods,
    }
}