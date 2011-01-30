﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BiDirectional_A_Star
{
    public class BiDirectionAStar
    {
        private Node startNode;
        private Node destNode;

        public List<Agent> Allies
        {
            set {
                startToDest.Allies = value;
                destToStart.Allies = value;
            }
        }
        public List<Agent> Enemies
        {
            set
            {
                startToDest.Enemies = value;
                destToStart.Enemies = value;
            }
        }

        private AStar startToDest;
        private AStar destToStart;

        public List<Node> answer { get; private set; }
        public List<Node> stdSoFar { get; private set; }
        public List<Node> stdFrontier { get; private set; }
        public List<Node> dtsSoFar { get; private set; }
        public List<Node> dtsFrontier { get; private set; }

        public BiDirectionAStar(Node start, Node dest)
        {
            this.startNode = start;
            this.destNode = dest;
            this.startToDest = new AStar(start, dest);
            this.destToStart = new AStar(dest, start);

            answer = new List<Node>();
            stdSoFar = new List<Node>();
            dtsSoFar = new List<Node>();
            stdFrontier = this.startToDest.FrontierNodes;
            dtsFrontier = this.destToStart.FrontierNodes;
        }

        public List<Node> Step()
        {
            if (answer.Count > 0) { return answer; }

            float stdBest = startToDest.CalcBestDistance();
            float dtsBest = destToStart.CalcBestDistance();

            Node expandedNode = null;
            String chosen = "";
            if (stdBest <= dtsBest)
            {
                chosen = "std";
                expandedNode = startToDest.Step(chosen);
                stdFrontier = startToDest.FrontierNodes;
            }
            else
            {
                chosen = "dts";
                expandedNode = destToStart.Step(chosen);
                dtsFrontier = destToStart.FrontierNodes;
            }

            foreach (Node n in startToDest.FrontierNodes) {
                if (destToStart.FrontierNodes.Contains(n))
                {
                    expandedNode = n;
                    break;
                }
            }

            List<Node> soFar = new List<Node>();
            int count = 0;
            foreach (Node cameFrom in expandedNode.CameFrom.Values)
            {
                if (count == 0)
                {
                    soFar.Add(expandedNode);
                    soFar.AddRange(getCameFrom(cameFrom, count));

                    if (chosen.Equals("std"))
                    {
                        stdSoFar = soFar;
                    }
                    else
                    {
                        dtsSoFar = soFar;
                    }
                }
                else if (count == 1)
                {
                    soFar.AddRange(getCameFrom(cameFrom, count));
                }
                count++;
            }
            if (count > 1)
            {
                answer = soFar;
            }

            // reconstruct path based on frontier nodes from the two AStar objects
            return soFar;
        }

        private List<Node> getCameFrom(Node n, int count)
        {
            List<Node> temp = new List<Node>();
            if (count == 1)
            {
                temp.Add(n);
            }
            if (n.CameFrom.Count > 0)
            {
                temp.AddRange(getCameFrom(n.CameFrom.First().Value, count));
            }
            if (count == 0)
            {
                temp.Add(n);
            }
            return temp;
        }

        public List<Node> Solve()
        {
            while (answer.Count == 0)
            {
                Step();
            }
            return answer;
        }
    }
}
