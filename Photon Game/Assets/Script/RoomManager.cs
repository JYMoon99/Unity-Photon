using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro.EditorUtilities;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Button roomCreate;
    public InputField roomName;
    public InputField roomPerson;
    public Transform roomContent;

    // �� ����� �����ϱ� ���� �ڷᱸ��
    Dictionary<string, RoomInfo> roomCatalog = new Dictionary<string, RoomInfo>();

    void Update()
    {
        if(roomName.text.Length > 0 && roomPerson.text.Length > 0)
            roomCreate.interactable = true;
        else
            roomCreate.interactable = false;
    }

    public override void OnJoinedRoom()
    {
        // �뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
        PhotonNetwork.LoadLevel("Photon Game");
    }

    public void CreateRoomObject()
    {
        // RoomCatalog�� ���� ���� Value���� ���ִٸ� RoomInfo�� �־��ش�
        foreach (RoomInfo info in roomCatalog.Values)
        {
            // ���� �����Ѵ�
            GameObject room = Instantiate(Resources.Load<GameObject>("Room"));

            // RoomContect�� ���� ������Ʈ�� �����Ѵ�
            room.transform.SetParent(roomContent);

            // �� ������ �Է��Ѵ�
            room.GetComponent<Information>().SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
        }
    }

    public void OnClickCreateRoom()
    {
        // �� �ɼ��� ����
        RoomOptions option = new RoomOptions();

        // �ִ� ���ӻ��� ���� ����
        option.MaxPlayers = byte.Parse(roomPerson.text);

        // ���� ���� ���θ� ����
        option.IsOpen = true;

        // �κ񿡼� �� ����� �����ų�� ����
        option.IsVisible = true;

        // ���� �����ϴ� �Լ�
        PhotonNetwork.CreateRoom(roomName.text, option);
    }

    public void AllDeleteRoom()
    {
        // Transform ������Ʈ�� �ִ� ���� ������Ʈ�� �����Ͽ� ��ü ������ �õ�
        foreach(Transform trans in roomContent)
        {
            // Transform�� ������ �ִ� ���� ������Ʈ�� ����
            Destroy(trans.gameObject);
        }
    }

    // �ش� �κ� �� ����� ���� ������ ������ ȣ��(�߰�, ����, ����)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        AllDeleteRoom();
       // UpdateRoom(roomList);
        CreateRoomObject();
    }

    public void UpdateRoom(List<RoomInfo>roomList)
    {
        
        for (int i = 0; i < roomList.Count; i++)
        {
            // �ش� �̸��� RoomCatalog�� key ������ �����Ǿ� �ִٸ�
            if (roomCatalog.ContainsKey(roomList[i].Name) == true)
            {
                // RemovedFromList : (true) �뿡�� ������ �Ǿ��� ��
                if (roomList[i].RemovedFromList)
                {
                    roomCatalog.Remove(roomList[i].Name);
                    continue;
                }
            }

            
            roomCatalog[roomList[i].Name] = roomList[i];    
        }
    }

}
