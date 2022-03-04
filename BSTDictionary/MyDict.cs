/*
 * File: Dictionary.cs
 * Author: St33l
 * Date: March 2022
 * Description: Implements a dictionary of <K, V> pairs using a binary search tree. Recreated from an old program I made
 *              using C++ to help learn C#
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSTDictionary
{
    class MyDict<K, V> where K: IComparable<K>
    {
        private Node<K, V> root;
        private int size;

        public MyDict()
        {
            this.size = 0;
            this.root = null;
        }


        //adds a new K,V pair to the dictionary or updates the value if a key already exists
        public void Add(K key, V value)
        {
            //not the most efficient way to check, navigates tree multiple times
            //but as this is a recreation of an old program I didn't change it.
            if (KeyExists(key))
            {
                var desiredNode = GetNode(root, key);
                desiredNode.value = value;
            } else
            {
                var newNode = new Node<K, V>(key, value);
                if (root == null)
                {
                    root = newNode;
                }
                else
                {
                    var parent = GetParent(root, key);
                    insertAsChild(parent, newNode);
                }
                size++;
            }
        }

        //returns the value of a K,V pair with the given key
        //throws an exception if the key is not present in the dictionary
        public V GetValue(K key)
        {
            var node = GetNode(root, key);
            if(node == null)
            {
                throw new KeyNotFoundException($"The key '{key}' does not exist");
            }
            return node.value;
        }

        //returns the key of the first K,V pair found whose value matches the input value
        //throws an exception if the key is not present in the dictionary
        public K GetKey(V value)
        {
            var node = GetKeyHelper(root, value);
            if(node == null)
            {
                throw new KeyNotFoundException($"The value '{value}' does not exist");
            }
            return node.key;
        }

        //returns true if the given key exists in the dictionary, or false otherwise
        public bool KeyExists(K key)
        {
            return (GetNode(root, key) != null);
        }

        //
        public void Display()
        {
            Console.Write("Dictionary{ ");
            DisplayHelper(root);
            Console.Write("}\n");
        }

        //returns the number of K,V pairs in the dictionary
        public int Size()
        {
            return this.size;
        }
        
        //deletes a node from the dictionary matching the given key
        public void Remove(K key)
        {
            var desiredNode = GetNode(root, key);
            if (desiredNode == null) { return; }
            else if (size == 1)
            {
                root = null;
            } 
            else if (desiredNode == root)
            {
                var leftSubtree = desiredNode.left;
                if (root.right != null)
                {
                    root = root.right;
                    if (leftSubtree != null)
                    {
                        var newLeftParent = GetParent(root, leftSubtree.key);
                        insertAsChild(newLeftParent, leftSubtree);
                    }
                } 
                else
                {
                    root = root.left;
                }
                
            }
            else
            {
                var parent = GetParent(root, desiredNode.key);
                if(desiredNode.key.CompareTo(parent.key) < 0)
                {
                    parent.left = null;
                } 
                else
                {
                    parent.right = null;
                }
                var leftSubtree = desiredNode.left;
                var rightSubtree = desiredNode.right;
                if(leftSubtree != null) {
                    var newLeftParent = GetParent(root, leftSubtree.key);
                    insertAsChild(newLeftParent, leftSubtree);
                }
                if (rightSubtree != null)
                {
                    var newRightParent = GetParent(root, rightSubtree.key);
                    insertAsChild(newRightParent, rightSubtree);
                }
            }
            size--;
        }

        //recursively prints the BST from left to right
        private void DisplayHelper(Node<K, V> node)
        {
            if(node != null)
            {
                DisplayHelper(node.left);
                Console.Write("(" + node.key + " -> " + node.value + ") ");
                DisplayHelper(node.right);
            }
        }

        //returns a reference to the node matching the given key, or null if it DNE
        //calls itself recursively until it reaches the end or a matching key
        private Node<K, V> GetNode(Node<K,V> curr, K key)
        {
            if(curr == null)
            {
                return null;
            } 
            else if(key.Equals(curr.key))
            {
                return curr;
            }
            else if(key.CompareTo(curr.key) > 0) {
                return GetNode(curr.right, key);
            }
            return GetNode(curr.left, key);
        }

        //returns the node that should be the parent of the new node added/node removed
        //recursively calls itself as it navigates the tree
        private Node<K, V> GetParent(Node<K, V> curr, K key)
        {
            //this case should never occur, but in the event it does return the root
            if(curr == null)
            {
                return this.root;
            }
            //navigate the left side of the tree
            else if (key.CompareTo(curr.key) < 0)
            {
                if(curr.left == null || curr.left.key.Equals(key))
                {
                    return curr;
                } 
                else
                {
                    return GetParent(curr.left, key);
                }
            }
            //navigate the right side
            else
            {
                if(curr.right == null || curr.right.key.Equals(key))
                {
                    return curr;
                } 
                else
                {
                    return GetParent(curr.right, key);
                }
            }
        }

        //helper for searching for a key whose value matches a given input
        private Node<K, V> GetKeyHelper(Node<K, V> curr, V value)
        {
            if (curr == null)
            {
                return null;
            }
            else if (curr.value.Equals(value))
            {
                return curr;
            }
            else
            {
                var leftSideSearch = GetKeyHelper(curr.left, value);
                if(leftSideSearch == null)
                {
                    return GetKeyHelper(curr.right, value);
                } 
                else
                {
                    return leftSideSearch;
                }
            }
        }

        //inserts a node as either the left or right child of a parent node depending on its key
        private void insertAsChild(Node<K, V> parent, Node<K, V> child)
        {
            if (child.key.CompareTo(parent.key) < 0)
            {
                parent.left = child;
            }
            else
            {
                parent.right = child;
            }
        }

        //container class for storing Key-Value pairs
        //since i'm not familiar with the standard for inner class access modifiers, everything is 
        //accessible to the Dictionary class
        private class Node<Key, Val>
        {
            public Node<Key, Val> left, right;
            public Key key;
            public Val value;
            public Node(Key _key, Val _value)
            {
                this.key = _key;
                this.value = _value;
                this.left = this.right = null;
            }
        }
    }
}
