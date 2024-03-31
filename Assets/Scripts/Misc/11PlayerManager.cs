// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Mirror;
// using UnityEngine;
//
// public class PlayerManager : NetworkBehaviour
// {
//     public GameObject Card;
//     public GameObject PlayerHand;
//     public GameObject OpponentHand;
//     public GameObject Table;
//     
//
//     private List<GameObject> cards = new List<GameObject>();
//
//     public override void OnStartClient()
//     {
//         base.OnStartClient();
//         PlayerHand = GameObject.Find("PlayerHand");
//         OpponentHand = GameObject.Find("OpponentHand");
//         Table = GameObject.Find("Table");
//     }
//     
//     [Server]
//     public override void OnStartServer()
//     {
//         base.OnStartServer();
//         
//         cards.Add(Card);
//         // Debug.Log(cards);
//     }
//
//     [Command]
//     public void CmdDealCards()
//     {
//         for (int i = 0; i < 5; i++)
//         {
//             GameObject card = Instantiate(Card, new Vector2(0, 0), Quaternion.identity);
//             NetworkServer.Spawn(card, connectionToClient);
//             card.transform.SetParent(PlayerHand.transform, false);
//             RpcShowCard(card, "Dealt");
//         }
//     }
//
//     [Command]
//     public void CmdPlayCard(GameObject card)
//     {
//         RpcShowCard(card, "Played");
//     }
//
//     [ClientRpc]
//     private void RpcShowCard(GameObject card, string type)
//     {
//         if (type == "Dealt")
//         {
//             if (isOwned)
//             {
//                 card.transform.SetParent(PlayerHand.transform, false);        
//             }
//             else
//             {
//                 card.transform.SetParent(OpponentHand.transform, false);
//             }
//         }
//         else if (type == "Played")
//         {
//             card.transform.SetParent(Table.transform, false);
//         }
//     }
// }
