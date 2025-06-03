using System;
using UnityEngine;

public class QuadTree<T> where T : MonoBehaviour
{
    private ushort Count = 0;
    private readonly ushort Capacity;
    public object[] Elements;

    public readonly Vector2 m_position;
    public readonly float m_size;
    private bool m_isLeaf = true;

    public QuadTree(Vector2 _pos, float _size, ushort _cap = 4)
    {
        m_position = _pos;
        m_size = _size;
        Capacity = _cap;
        Elements = new object[Capacity];
    }

    public bool InsertItem(T _item)
    {
        if (IsInBounds(_item.transform.position))
            if (m_isLeaf)
                if (Count < Capacity)
                {
                    Elements[Count] = _item;
                    Count++;
                    return true;
                }
                else
                    return Subdivide(_item);
            else
            {
                AddToLeaf(_item);
                return true;
            }
        else
            return false;
    }

    private bool IsInBounds(Vector2 _objPos)
    {
        return
            (_objPos.x >= m_position.x - m_size) && (_objPos.x <= m_position.x + m_size) &&
            (_objPos.y >= m_position.y - m_size) && (_objPos.y <= m_position.y + m_size);
    }

    private bool Subdivide(T _item)
    {
        T[] currentObjects = new T[Capacity + 1];
        currentObjects[Capacity] = _item;
        Array.Copy(Elements, currentObjects, Capacity);

        Elements[0] = new QuadTree<T>(new Vector2(m_position.x + (m_size * 0.5f), m_position.y + (m_size * 0.5f)), m_size * 0.5f, Capacity);
        Elements[1] = new QuadTree<T>(new Vector2(m_position.x + (m_size * 0.5f), m_position.y - (m_size * 0.5f)), m_size * 0.5f, Capacity);
        Elements[2] = new QuadTree<T>(new Vector2(m_position.x - (m_size * 0.5f), m_position.y - (m_size * 0.5f)), m_size * 0.5f, Capacity);
        Elements[3] = new QuadTree<T>(new Vector2(m_position.x - (m_size * 0.5f), m_position.y + (m_size * 0.5f)), m_size * 0.5f, Capacity);

        foreach (T obj in currentObjects)
        {
            AddToLeaf(obj);
        }

        m_isLeaf = false;
        return true;
    }

    private void AddToLeaf(T _obj)
    {
        for (int i = 0; i < Elements.Length; i++)
        {
            QuadTree<T> tree;
            tree = Elements[i] as QuadTree<T>;

            if (tree.InsertItem(_obj))
                break;
        }
    }

    public bool GetItems(out T[] foundElements)
    {
        foundElements = new T[0];
        return true;
    }

}
