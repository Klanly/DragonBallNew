using UnityEngine;
using System.Collections;

public class UISendMessageDialog : MonoBehaviour
{
	public UIInput input;
	public UILabel wordsCountLabel;
	public OtherUserInfo otherUserInfo;

	void onSubmit()
	{
		input.isSelected = false;
		wordsCountLabel.text = input.value.Length.ToString() + "/80";
	}

	public static UISendMessageDialog getSendMessageDialog(Transform parent)
	{
		GameObject sendMessageDialogPrb;

		sendMessageDialogPrb = PrefabLoader.loadFromPack("WHY/pbUISendMessageDialog") as GameObject;


		GameObject sendMessageDialogObj = Instantiate(sendMessageDialogPrb) as GameObject;

		sendMessageDialogObj.transform.parent = parent;
		sendMessageDialogObj.transform.localPosition = Vector3.zero;
		sendMessageDialogObj.transform.localScale = Vector3.one;
		return sendMessageDialogObj.GetComponent<UISendMessageDialog>();
	}

	void onClose()
	{
		Destroy(gameObject);
	}

	void onSendMessage()
	{
		if(input.value == "")
		{
			return;
		}
		ComLoading.Open();
		Core.Data.FriendManager.sendMessageRequestCompletedDelegate = sendMessageRequestCompleted;
		Core.Data.FriendManager.sendMessageRequest(otherUserInfo.g, input.value);
	}


	void sendMessageRequestCompleted(bool isSucce)
	{
		Core.Data.FriendManager.sendMessageRequestCompletedDelegate = null;
		if(isSucce)
		{
			this.input.value = "";
		}
		else
		{
		}

		this.input.isSelected = true;
	}
}
