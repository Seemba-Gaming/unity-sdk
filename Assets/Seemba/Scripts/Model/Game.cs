using System;
using UnityEngine;

public class Game
{
	public string _id, name, editorId, bundle_id, appstore_id;
	public string icon;
	public Game (string _id, string name, string editorId, string bundle_id, string appstore_id)
	{
        
        this._id = _id;
		this.name = name;
		this.editorId = editorId;
		this.bundle_id = bundle_id;
		this.appstore_id = appstore_id;
	}
	public Game (string _id, string name, string editorId, string bundle_id, string appstore_id,string icon)
	{
		this._id = _id;
		this.name = name;
		this.editorId = editorId;
		this.icon = icon;
		this.bundle_id = bundle_id;
		this.appstore_id = appstore_id;
	}
    public Game(string _id, string name)
    {
        this._id = _id;
        this.name = name;
        
    }
}

