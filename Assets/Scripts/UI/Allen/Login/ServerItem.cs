/*
 * Created By Allen Wu. All rights reserved.
 */
using System;
using UnityEngine;
using System.Collections;

public class ServerItem : MonoBehaviour {

	[HideInInspector]
	public Action<Server> onClick;

    public UISprite spStatus;
	public UILabel txtServerName;
	private Server curServer;

	public void setItem(Server server) {
		Utils.Assert(server == null, "Server can't be null");
		curServer = server;
		txtServerName.text = curServer.name;
        spStatus.spriteName = toSpriteName(server.status);
        spStatus.MakePixelPerfect();
	}

	public void OnServerSelected() {
		if(onClick != null) {
			onClick(curServer);
		}
	}


    private string toSpriteName(int status) {
        string name = null;

        switch(status) {
        case Server.STATUS_COMBINE:
            name = "hefu";
            break;
        case Server.STATUS_HOT:
            name = "login-0002";
            break;
        case Server.STATUS_NEW:
            name = "login-0001";
            break;
        case Server.STATUS_RECOMMAND:
            name = "login-0004";
            break;
        case Server.STATUS_FULL:
            name = "weihu";
            break;
        case Server.STATUS_STOP:
            name = "tingfu";
            break;
		default:
			name = "login-0002";
			break;
        }

        return name;
    }
}
