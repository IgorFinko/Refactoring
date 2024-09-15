#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Example3
{
    public interface IPathfindingStrategy
    {
        List<Vector2>? FindPath(Vector2 start, Vector2 target);
    }

    public class CustomPathfindingStrategy : IPathfindingStrategy
    {
        public List<Vector2>? FindPath(Vector2 start, Vector2 target)
        {
            return < много строк кода по созданию пути из start в target >;
        }
    }

    public class Player
    {
        private bool isMoving;
        private Vector2 currentPosition;
        private Player? currentEnemy;
        private IPathfindingStrategy pathfindingStrategy;

        public Player(IPathfindingStrategy strategy)
        {
            pathfindingStrategy = strategy;
        }

        public void Update()
        {
            if (currentEnemy == null) 
                return;

            var builtPath = pathfindingStrategy.FindPath(currentPosition, currentEnemy.currentPosition);
            UpdateMovementState(builtPath);
        }

        private void UpdateMovementState(List<Vector2>? path)
        {
            if (path == null)
            {
                currentEnemy = null;
                isMoving = false;
            }
            else
            {
                isMoving = true;
            }
        }
    }

    class Program
    {

        public static void Main(string[] args)
        {
            IPathfindingStrategy pathfindingStrategy = new CustomPathfindingStrategy();
            var player = new Player(pathfindingStrategy);

            // This method can now be called in the update loop:
            player.Update();
        }
    }
}

