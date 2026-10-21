using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activity1DelaPena
{
    internal class VacuumCleaning
    {
    }

    public abstract class Environment
    {
        public abstract void ExecuteAction(Agent agent, object action);
    }

    public abstract class Agent
    {
        public int Performance { get; set; } = 0;
        public abstract object Program(object percept);
    }

    public class VacuumEnvironment : Environment
    {
        private int[,] grid = new int[2, 2];
        private int agentX = 0;
        private int agentY = 0;

        private Random rand = new Random();
        public VacuumEnvironment()
        {
            for(int i = 0; i<2; i++)
            {
                for(int j = 0; j<2; j++)
                {
                    grid[i,j] = rand.Next(0,2);
                }
            }

            agentX = 0;
            agentY = 0;
        }
        public int[] AgentLoc()
        {
            int[] loc = new int[2];
            return loc;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Grid State: ");
            
            for(int i = 0; i<2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    sb.Append(grid[i, j] + " ");
                }
                sb.AppendLine();
            }

            sb.AppendLine($"Agent position: ({agentX}, {agentY})");
            return sb.ToString();
        }

        public object Percept(Agent agent)
        {
            bool isDirty = grid[agentX, agentY] == 1;
            return Tuple.Create(agentX, agentY, isDirty);
        }

        public override void ExecuteAction(Agent agent, object action)
        {
            string act = action as String;
            if(act == null)
            {
                agent.Performance -= 1;
                return;
            }

            if(act == "Suck")
            {
                if (grid[agentX,agentY] == 1)
                {
                    grid[agentX, agentY] = 0;
                    agent.Performance += 10;
                }
            }else if(act == "Up" && agentY > 0){
                agentY -= 1;
                agent.Performance -= 1;
            }else if(act == "Down" && agentY < 1)
            {
                agentY += 1;
                agent.Performance -= 1;
            }else if(act == "Left" && agentX > 0)
            {
                agentX -= 1;
                agent.Performance -= 1;
            }else if(act == "Right" && agentX < 1)
            {
                agentX += 1;
                agent.Performance -= 1;
            }
            else
            {
                agent.Performance -= 1;
            }
        }
    }

    //public class SimpleReflexAgent : Agent
    //{
    //    private readonly Random rand = new Random();

    //    public override object Program(object percept)
    //    {
    //        var tup = percept as Tuple<int, int, bool>;
    //        if (tup == null)
    //        {
    //            return null;
    //        }

    //        bool isDirty = tup.Item3;
    //        if (isDirty)
    //        {
    //            return "Suck";
    //        }

    //        string[] choices = { "Up", "Down", "Left", "Right" };
    //        return choices[rand.Next(choices.Length)];
    //    }
    //}

    //Simple movement algorithm
    public class MovementReflexAgent: Agent
    {
        private int[,] storeMovement = new int[2, 2];
        public override object Program(object percept)
        {
            var tup = percept as Tuple<int, int, bool>;
            int x = tup.Item1;
            int y = tup.Item2;

            if (tup == null && storeMovement[x,y] == 1)
            {
                return null;
            }
            
            bool isDirty = tup.Item3;
            if (isDirty)
            {
                storeMovement[x, y] = 1;
                return "Suck";
            }

            storeMovement[x, y] = 1;
            if (x == 0)
            {
                if(y == 0)
                {
                    return "Right";
                }
                else
                {
                    return "Up";
                }
            }
            else
            {
                if (y == 0)
                {
                    return "Down";
                }
                else
                {
                    return "Left";
                }
            }
        }
    }


}
