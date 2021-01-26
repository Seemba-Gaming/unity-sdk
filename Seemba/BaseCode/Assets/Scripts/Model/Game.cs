using System;
using System.Collections.Generic;
using UnityEngine;

namespace SeembaSDK
{
	[CLSCompliant(false)]
	[Serializable]
	public class Game
	{
		public string _id, name, editorId, bundle_id, appstore_id, game_scene_name, game_level;
		public string icon, background_image;
		public Game(string _id, string name, string editorId, string bundle_id, string appstore_id)
		{

			this._id = _id;
			this.name = name;
			this.editorId = editorId;
			this.bundle_id = bundle_id;
			this.appstore_id = appstore_id;
		}
		public Game(string _id, string name, string editorId, string bundle_id, string appstore_id, string icon, string background_image)
		{
			this._id = _id;
			this.name = name;
			this.editorId = editorId;
			this.icon = icon;
			this.background_image = background_image;
			this.bundle_id = bundle_id;
			this.appstore_id = appstore_id;
		}
		public Game(string _id, string name, string editorId, string bundle_id, string appstore_id, string icon)
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
		public Game(string _id, string name, string game_scene_name, string game_level)
		{
			this._id = _id;
			this.name = name;
			this.game_scene_name = game_scene_name;
			this.game_level = game_level;
		}
		public Game() { }

	}
}

