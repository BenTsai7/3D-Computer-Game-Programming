using System.Collections;
using System.Collections.Generic;
namespace PAD
{
	public class Node
	{
		public int P, D;
		public bool B; //True代表在左
		public List<Node> neighbour;
		public Node(int P, int D, bool B)
		{
			this.P = P;
			this.D = D;
			this.B = B;
			this.neighbour = new List<Node>();
		}
		//深拷贝
		public Node(Node n)
		{
			this.P = n.P;
			this.D = n.D;
			this.B = n.B;
			this.neighbour = new List<Node>(n.neighbour);
		}
		public void Connect(Node node)
		{
			foreach (Node n in neighbour)
			{
				if (isEqual(n, node))
				{
					return;
				}
			}
			neighbour.Add(node);
		}
		public static bool isEqual(Node first, Node second)
		{
			if (first.P == second.P && first.D == second.D
				&& first.B == second.B)
			{
				return true;
			}
			return false;
		}
	}
}
