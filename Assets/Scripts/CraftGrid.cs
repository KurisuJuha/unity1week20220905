using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftGrid : MonoBehaviour
{
    public GameObject Materials;
    public List<MaterialGrid> grids;
    public Image item;
    public int id;
    public GameObject GridPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        item.sprite = GameManager.Instance.settings.items[id].Sprite;
        var materials = GameManager.Instance.settings.items[id].Materials.ToList();

        // 数合わせ
        while (grids.Count > materials.Count)
        {
            Destroy(grids[grids.Count - 1].gameObject);
            grids.RemoveAt(grids.Count - 1);
        }
        while (grids.Count < materials.Count)
        {
            MaterialGrid grid = Instantiate(GridPrefab).GetComponent<MaterialGrid>();
            grid.transform.parent = Materials.transform;
            grid.transform.localScale = Vector3.one;
            grids.Add(grid);
        }

        // 量とidを代入
        for (int i = 0; i < materials.Count; i++)
        {
            grids[i].id = materials[i].id;
            grids[i].quantity = materials[i].quantity;
        }
    }
}
