using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class BoidSpawner : MonoBehaviour
{
    static public QuadTree<Boids> m_qtree;
    [SerializeField] GameObject m_prefab;
    public int spawnAmount;

    public float QuadTreeSize = 10f;
    private float spawnpos;

    static List<Boids> spawnedBoids = new List<Boids>();

    private void Awake()
    {
        spawnpos = QuadTreeSize / 4;
    }

    void Update()
    {
        m_qtree = new QuadTree<Boids>(Vector2.zero, QuadTreeSize);
        foreach (Boids boid in spawnedBoids)
            m_qtree.InsertItem(boid);

        if (spawnedBoids.Count < spawnAmount)
            spawnedBoids.Add(Instantiate(m_prefab, new Vector2(Random.Range(-spawnpos, spawnpos), Random.Range(-spawnpos, spawnpos)), Quaternion.identity).GetComponent<Boids>());
    }

    private void DrawElements(object[] elems)
    {
        foreach (object element in elems)
        {
            if (element is not null)
                if (element is QuadTree<Boids>)
                {
                    QuadTree<Boids> tree = (QuadTree<Boids>)element;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireCube(tree.m_position, Vector3.one * tree.m_size * 2);
                    DrawElements(tree.Elements);
                }
        }
    }

    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    private void OnDrawGizmos()
    {
        if (m_qtree is not null)
            DrawElements(m_qtree.Elements);
    }
}
