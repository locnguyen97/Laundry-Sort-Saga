using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentLevel = 1;
    public List<Color> listColor;
    [SerializeField] private List<ObjectMoveByDrag> listPrefabs;
    [SerializeField] private List<ObjectMoveByDrag> listInGame;
    [SerializeField] private List<GameObject> listMachine;
    [SerializeField] private List<GameObject> listMachineUse;
    private float minX = -1.8f;
    private float minY = -3.5f;
    private float maxX =2f;
    private float maxY = 0f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        InitItem();
    }

    public void RemoveListObjMove(ObjectMoveByDrag obj)
    {
        listInGame.Remove(obj);
        if (listInGame.Count <= 0)
        {
            Invoke(nameof(NextLevel),1.0f);
        }
    }
    
    void NextLevel()
    {
        currentLevel++;
        if (currentLevel >= 4)
            currentLevel = 1;
        listInGame.Clear();
        listMachineUse.Clear();
        
        Invoke(nameof(InitItem),0.25f);
    }

    public List<int> GetListIdByLevel()
    {
        if(currentLevel == 1) return new List<int>(){0,1};
        else if(currentLevel == 2) return new List<int>(){0,1,2};
        else return new List<int>(){0,1,2,3};
    }

    public void InitItem()
    {
        int soluong = (int) (1.5f * currentLevel) + 3;
        var list = GetListIdByLevel();
        foreach (var machine in listMachine)
        {
            machine.SetActive(false);
            machine.tag = "NotUse";
        }

        foreach (var id in list)
        {
            var mc = listMachine[Random.Range(0, listMachine.Count)];
            while (listMachineUse.Exists(l=>l== mc))
            {
                mc = listMachine[Random.Range(0, listMachine.Count)];
            }
            
            listMachineUse.Add(mc);
            mc.SetActive(true);
            mc.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = listColor[id];
            SetTagById(mc,id);
            
        }

        

        
        for (int i = 0; i < soluong; i++)
        {
            int id = list[Random.Range(0, list.Count)];
            var x = Instantiate(listPrefabs[Random.Range(0, listPrefabs.Count)],
                new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0), new Quaternion());
            x.SetData(id);
            listInGame.Add(x);
        }
    }

    void SetTagById(GameObject g,int id)
    {
        switch (id)
        {
            case 0: g.tag = "b1";break;
            case 1: g.tag = "b2";break;
            case 2: g.tag = "b3";break;
            case 3: g.tag = "b4";break;
            default: g.tag = "b1";break;
        }
    }
    
}