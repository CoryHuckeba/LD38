using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZone : MonoBehaviour {

    public EnemyPlanet planet;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pluto")
        {
            Debug.Log("WTF GUY");
            BossBar.Instance.SetInformation(planet);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pluto")
            BossBar.Instance.ClearInfo();
    }
}
