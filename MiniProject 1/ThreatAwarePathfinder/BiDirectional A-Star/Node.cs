﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BiDirectional_A_Star
{
    public class Node
    {
        public float? G { get; set; }
        public Vector2 Pos { get; set; }
        public List<Node> Neighbors { get; private set; }
        public List<Node> CameFrom { get; private set; }

        public Node()
        {
            Neighbors = new List<Node>();
            CameFrom = new List<Node>();
            G = null;
        }

        public float h(Node destNode, List<Agent> enemies, List<Agent> allies)
        {
            return AStarHelper.CalcDistByThreat(this, destNode, enemies, allies);
        }

        public bool Equals(Node n)
        {
            return this.Pos.X == n.Pos.X && this.Pos.Y == n.Pos.Y;
        }
    }
}