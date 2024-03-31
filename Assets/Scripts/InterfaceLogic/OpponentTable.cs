using UnityEngine;

public class OpponentTable : MonoBehaviour
{
    private void Update()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            GameObject go = this.gameObject.transform.GetChild(i).gameObject;

            Card tempCard = go.GetComponent<Card>();

            if (tempCard != null)
            {
                tempCard.State = CardState.OnTable;
                tempCard.Team = Team.Opponent;
            }
        }
    }
}