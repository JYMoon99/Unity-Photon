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

    // 룸 목록을 저장하기 위한 자료구조
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
        // 룸에 입장한 후 호출되는 콜백 함수
        PhotonNetwork.LoadLevel("Photon Game");
    }

    public void CreateRoomObject()
    {
        // RoomCatalog에 여러 개의 Value값이 들어가있다면 RoomInfo에 넣어준다
        foreach (RoomInfo info in roomCatalog.Values)
        {
            // 룸을 생성한다
            GameObject room = Instantiate(Resources.Load<GameObject>("Room"));

            // RoomContect의 하위 오브젝트로 설정한다
            room.transform.SetParent(roomContent);

            // 룸 정보를 입력한다
            room.GetComponent<Information>().SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
        }
    }

    public void OnClickCreateRoom()
    {
        // 룸 옵션을 설정
        RoomOptions option = new RoomOptions();

        // 최대 접속사의 수를 설정
        option.MaxPlayers = byte.Parse(roomPerson.text);

        // 룸의 오픈 여부를 설정
        option.IsOpen = true;

        // 로비에서 룸 목록을 노출시킬지 설정
        option.IsVisible = true;

        // 룸을 생성하는 함수
        PhotonNetwork.CreateRoom(roomName.text, option);
    }

    public void AllDeleteRoom()
    {
        // Transform 오브젝트에 있는 하위 오브젝트에 접근하여 전체 삭제를 시도
        foreach(Transform trans in roomContent)
        {
            // Transform이 가지고 있는 게임 오브젝트를 삭제
            Destroy(trans.gameObject);
        }
    }

    // 해당 로비에 방 목록의 변경 사항이 있으면 호출(추가, 삭제, 참가)
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
            // 해당 이름이 RoomCatalog의 key 값으로 설정되어 있다면
            if (roomCatalog.ContainsKey(roomList[i].Name) == true)
            {
                // RemovedFromList : (true) 룸에서 삭제가 되었을 때
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
