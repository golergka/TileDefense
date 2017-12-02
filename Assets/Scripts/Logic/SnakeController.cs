using System;
using System.Collections.Generic;

public class SnakeController
{
	public int SnakeLength { get; set; }

	public void RegisterFloor(Floor floor)
	{
		floor.SnakeActivated += delegate
		{
			snake.Enqueue(floor);
			while (snake.Count > SnakeLength)
			{
				snake.Dequeue().DeactivateSnake();
			}
		};
	}

	private Queue<Floor> snake = new Queue<Floor>();

}
