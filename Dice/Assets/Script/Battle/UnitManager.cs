using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitManager : MonoBehaviour
{
     [SerializeField] private Transform enemiesPivot;
     [SerializeField] private Transform playersPivot;

     [SerializeField] private GameObject enemyPrefab;
     [SerializeField] private GameObject playerPrefab;

     private readonly Dictionary<string, GameObject> _currentEnemies = new Dictionary<string, GameObject>();
     private readonly Dictionary<string, GameObject> _currentPlayers = new Dictionary<string, GameObject>();


     public void AddUnit(string id)
     {
          bool isPlayer = B.IsPlayerUnit(id);
          
          var newUnit = isPlayer ? Instantiate(playerPrefab, playersPivot) :Instantiate(enemyPrefab, enemiesPivot);
          if (newUnit.TryGetComponent<UIView_PlayerUnit>(out var pui))
          {
               pui.Init(id);
          }
          if (isPlayer)
          {
               _currentPlayers.Add(id, newUnit);
          }
          else
          {
               _currentEnemies.Add(id, newUnit);
          }
     }

     public void RemoveUnit(string id)
     {
          if (_currentEnemies.TryGetValue(id, out var enemyUnit))
          {
               // play animation
               _currentEnemies.Remove(id);
               Destroy(enemyUnit);
          }
          
          if (_currentPlayers.TryGetValue(id, out var playerUnit))
          {
               // play animation
               _currentPlayers.Remove(id);
               Destroy(playerUnit);
          }
     }
}
