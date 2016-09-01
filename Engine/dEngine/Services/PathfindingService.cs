// PathfindingService.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using Neo.IronLua;

#pragma warning disable 1591

namespace dEngine.Services
{
    /// <summary>
    /// A service used to calculate a <see cref="Path" /> between two points.
    /// </summary>
    public class PathfindingService
    {
        public Path ComputeRawPathAsync(Vector3 start, Vector3 finish, float maxDistance)
        {
            throw new NotImplementedException();
        }

        public Path ComputeSmoothPathAsync(Vector3 start, Vector3 finish, float maxDistance)
        {
            throw new NotImplementedException();
        }

        /*
        /// <summary>
        /// Method that switfly finds the best path from start to end.
        /// </summary>
        /// <returns>The starting breadcrumb traversable via .next to the end or null if there is no path</returns>        
        public static SearchNode FindPath(World world, Point3D start, Point3D end)
        {
            //note we just flip start and end here so you don't have to.            
            return FindPathReversed(world, end, start);
        }

        /// <summary>
        /// Method that switfly finds the best path from start to end. Doesn't reverse outcome
        /// </summary>
        /// <returns>The end breadcrump where each .next is a step back)</returns>
        private static SearchNode FindPathReversed(IWorld world, Vector3 start, Vector3 end)
        {
            var startNode = new SearchNode(start, 0, 0, null);

            var openList = new MinHeap();
            openList.Add(startNode);

            int sx = world.Right;
            int sy = world.Top;
            int sz = world.Back;
            bool[] brWorld = new bool[sx * sy * sz];
            brWorld[start.X + (start.Y + start.Z * sy) * sx] = true;

            while (openList.HasNext())
            {
                var current = openList.ExtractFirst();

                if (current.position.GetDistanceSquared(end) <= 3)
                {
                    return new SearchNode(end, current.pathCost + 1, current.cost + 1, current);
                }

                for (int i = 0; i < surrounding.Length; i++)
                {
                    Surr surr = surrounding[i];
                    var tmp = current.Position + surr.Point;
                    int brWorldIdx = tmp.X + (tmp.Y + tmp.Z * sy) * sx;

                    if (world.PositionIsFree(tmp) && brWorld[brWorldIdx] == false)
                    {
                        brWorld[brWorldIdx] = true;
                        int pathCost = current.pathCost + surr.Cost;
                        int cost = pathCost + tmp.GetDistanceSquared(end);
                        SearchNode node = new SearchNode(tmp, cost, pathCost, current);
                        openList.Add(node);
                    }
                }
            }
            return null; //no path found
        }

        internal class Surr
        {
            public Surr(int x, int y, int z)
            {
                Point = new Vector3(x, y, z);
                Cost = x * x + y * y + z * z;
            }

            public Vector3 Point;
            public int Cost;
        }

        //Neighbour options
        private static Surr[] surrounding = new Surr[]{                        
            //Top slice (Y=1)
            new Surr(-1,1,1), new Surr(0,1,1), new Surr(1,1,1),
            new Surr(-1,1,0), new Surr(0,1,0), new Surr(1,1,0),
            new Surr(-1,1,-1), new Surr(0,1,-1), new Surr(1,1,-1),
            //Middle slice (Y=0)
            new Surr(-1,0,1), new Surr(0,0,1), new Surr(1,0,1),
            new Surr(-1,0,0), new Surr(1,0,0), //(0,0,0) is self
            new Surr(-1,0,-1), new Surr(0,0,-1), new Surr(1,0,-1),
            //Bottom slice (Y=-1)
            new Surr(-1,-1,1), new Surr(0,-1,1), new Surr(1,-1,1),
            new Surr(-1,-1,0), new Surr(0,-1,0), new Surr(1,-1,0),
            new Surr(-1,-1,-1), new Surr(0,-1,-1), new Surr(1,-1,-1)
        };
        */

        /// <summary>
        /// A path between two points.
        /// </summary>
        public class Path
        {
            private readonly Vector3[] _points;

            /// <summary />
            public Path(PathStatus status, Vector3[] points)
            {
                _points = points;
                Status = status;
            }

            /// <summary>
            /// The status of the path.
            /// </summary>
            public PathStatus Status { get; }

            /// <summary>
            /// Returns a table of <see cref="Vector3" />s that comprise the path.
            /// </summary>
            /// <returns></returns>
            public LuaTable GetPointCoordinates()
            {
                return LuaTable.pack(_points);
            }
        }

        internal class SearchNode
        {
            public Vector3 Position;
            public int Cost;
            public int PathCost;
            public SearchNode Next;
            public SearchNode NextListElem;

            public SearchNode(Vector3 position, int cost, int pathCost, SearchNode next)
            {
                Position = position;
                Cost = cost;
                PathCost = pathCost;
                Next = next;
            }
        }

        internal class MinHeap
        {
            private SearchNode listHead;

            public bool HasNext()
            {
                return listHead != null;
            }

            public void Add(SearchNode item)
            {
                if (listHead == null)
                {
                    listHead = item;
                }
                else if ((listHead.Next == null) && (item.Cost <= listHead.Cost))
                {
                    item.NextListElem = listHead;
                    listHead = item;
                }
                else
                {
                    var ptr = listHead;
                    while ((ptr.NextListElem != null) && (ptr.NextListElem.Cost < item.Cost))
                        ptr = ptr.NextListElem;
                    item.NextListElem = ptr.NextListElem;
                    ptr.NextListElem = item;
                }
            }

            public SearchNode ExtractFirst()
            {
                var result = listHead;
                listHead = listHead.NextListElem;
                return result;
            }
        }
    }
}