using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAD
{
	public class StateMove
	{
		public int P;
		public int D;
		public StateMove(int P, int D)
		{
			this.P = P;
			this.D = D;
		}
	}
	//用于BFS搜索
	public class SearchNode
	{
		public Node startNode;
		public Node realNode;
		public SearchNode(Node startNode, Node realNode)
		{
			this.startNode = startNode;
			this.realNode = realNode;
		}
	}
	public class Graph
	{
		public List<Node> nodes;
		//可走状态
		public StateMove[] moves = { new StateMove(0, 1), new StateMove(1, 0), new StateMove(1, 1), new StateMove(2, 0), new StateMove(0, 2) };
		public int maxP;
		public int maxD;
		public Node endNode;
		public Graph(int maxP, int maxD)
		{
			this.maxP = maxP;
			this.maxD = maxD;
			nodes = new List<Node>();
			endNode = new Node(0, 0, false);
			construct();
		}
		private void construct()
		{
			//初始点
			nodes.Add(new Node(maxP, maxD, true));
			for (int i = 0; i < nodes.Count; ++i)
			{
				Node curNode = nodes[i];
				foreach (StateMove move in moves)
				{
					Node n = new Node(curNode.P, curNode.D, !curNode.B);
					//一开始在左岸
					if (curNode.B)
					{
						n.P -= move.P;
						n.D -= move.D;
					}
					else
					{
						n.P += move.P;
						n.D += move.D;
					}
					//验证该解是否有效
					if (isValid(n))
					{
						bool flag = false;
						foreach (Node tmp in nodes)
						{
							if (Node.isEqual(tmp, n))
							{
								flag = true;
								break;
							}
						}
						if (flag) continue;
						n.Connect(curNode);
						curNode.Connect(n);
						nodes.Add(n);
					}
				}
			}
		}
		private bool isValid(Node n)
		{
			if (n.D < 0 || n.P < 0) return false;
			if (n.D > maxD || n.P > maxP) return false;
			if (n.D > n.P && n.P != 0) return false;
			int P = maxP - n.P;
			int D = maxD - n.D;
			if (D > P && P != 0) return false;
			return true;
		}

		public StateMove getNextMove(Node curNode)
		{
			Node nextNode = BFS(curNode);
			if (nextNode == null)
			{
				return null;
			}
			else
			{
				return new StateMove(Mathf.Abs(curNode.P - nextNode.P), Mathf.Abs(curNode.D - nextNode.D));
			}
		}

		public Node getNodeFromGraph(Node node) {
			foreach (Node n in nodes) {
				if (Node.isEqual(n, node))
					return n;
			}
			return null;
		}
		private Node BFS(Node begin)
		{
			List<SearchNode> searchList = new List<SearchNode>();
			Node beginnode = getNodeFromGraph(begin);
			searchList.Add(new SearchNode(null, beginnode));
			foreach (Node n in beginnode.neighbour)
			{
				if (Node.isEqual(n, endNode))
				{
					return n;
				}
				else
				{
					searchList.Add(new SearchNode(n, n));
				}
			}
			for (int i = 1; i < searchList.Count; i++)
			{
				foreach (Node n in searchList[i].realNode.neighbour)
				{
					if ((Node.isEqual(n, endNode)))
					{
						return searchList[i].startNode;
					}
					else
					{
						bool flag = false;
						foreach (SearchNode sn in searchList)
						{
							if (Node.isEqual(n, sn.realNode))
							{
								flag = true;
								break;
							}
						}
						if (flag) continue;
						searchList.Add(new SearchNode(searchList[i].startNode, n));
					}
				}
			}
			return null;
		}
	}
}
