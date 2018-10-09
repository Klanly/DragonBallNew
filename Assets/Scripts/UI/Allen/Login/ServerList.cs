/*
 * Created By Allen Wu. All rights reserved.
 */

using System;
using UnityEngine;
using System.Collections.Generic;

using UObj = UnityEngine.Object;

public class ServerList : MonoBehaviour {

	private const string LIST_PATH = "Allen/ServerList";
	private const string ITEM_PATH = "Allen/ServerItem";

	public UITable ScrollOffset;
	public UILabel txt_lastLogin;
	public UILabel btnLastLogin;

    //推荐的服务器位置
    private const int maxCount = 2;
    public GameObject btnRecommandLeft;
    public GameObject btnRecommandRight;
    public UILabel txtRecommandLeft;
    public UILabel txtRecommandRight;

	private List<Server> ListOfServer = null;
    private List<Server> ListOfRecommandServer = null;
    private Server lastedLogin = null;

	private UObj ItemObj;

	public Action<Server> onClick;


	void Start() {

		RED.AddChild(gameObject, Camera.main.gameObject);

		if(ScrollOffset != null) {
            ScrollOffset.transform.localPosition = new Vector3(-247.9f, 159.49f, 0f);
		}
        //txt_lastLogin.text = Core.Data.stringManager.getString(7);

		ItemObj = PrefabLoader.loadFromUnPack(ITEM_PATH);
		showServerList();
        showRecommmandList();
	}

	public void setLastLoginServer(Server lastServer) {
		if(lastServer != null) {
            lastedLogin = lastServer;
			btnLastLogin.text = lastServer.name;
		}
	}

    public void onLastLoginBtnClick() {
        OnButttonClick(lastedLogin);
    }

	public void setServerList(Server[] serList) {
		if(serList == null || serList.Length == 0) {
			ConsoleEx.DebugLog("No Server is avaliable");
		} else {
			ListOfServer = new List<Server>(serList);
            ListOfRecommandServer = new List<Server>();

            foreach(Server server in ListOfServer) {
                if(server != null && server.status == Server.STATUS_RECOMMAND) {
                    ListOfRecommandServer.Add(server);
                }
            }
		}
	}

	void showServerList() {
		foreach(Server server in ListOfServer) {
			if(server != null) {
				GameObject item = Instantiate(ItemObj) as GameObject;
				RED.AddChild(item, ScrollOffset.gameObject);
				ServerItem scItem = item.GetComponent<ServerItem>();
				scItem.setItem(server);
				scItem.onClick = OnButttonClick;
			}
		}
		ScrollOffset.Reposition();
	}

    void showRecommmandList() {
        int curCount = ListOfRecommandServer.Count;

        if(curCount >= maxCount) {
            btnRecommandLeft.SetActive(true);
            btnRecommandRight.SetActive(true);

            curCount = maxCount;
        } else if(curCount < maxCount) {
            if(curCount == 0) {
                btnRecommandLeft.SetActive(false);
                btnRecommandRight.SetActive(false);
            } else {
                btnRecommandLeft.SetActive(true);
                btnRecommandRight.SetActive(false);
            }
        }

        for(int i = 0; i < curCount; ++ i) {
            Server server = ListOfRecommandServer[i];
            ServerItem scItem = null;
            if(i == 0) {
                txtRecommandLeft.text = server.name;
                scItem = btnRecommandLeft.GetComponent<ServerItem>();
            } else {
                txtRecommandRight.text = server.name;
                scItem = btnRecommandRight.GetComponent<ServerItem>();
            }

            scItem.setItem(server);
            scItem.onClick = OnButttonClick;
        }

    }


	private static ServerList _instance;
	public static ServerList Instance() {

		if(_instance == null) {
			_instance = (Instantiate(PrefabLoader.loadFromUnPack(LIST_PATH)) as GameObject).GetComponent<ServerList>();
		}

		return _instance;

	}

	void OnDestory() {
		GameObject.Destroy(ItemObj);
		ItemObj = null;
	}

	void OnButttonClick(Server server) {
		if(onClick != null) {
			onClick(server);
		}
		DestroyUI();
	}

	void DestroyUI()
	{
		Destroy(gameObject);
		_instance = null;
	}
}