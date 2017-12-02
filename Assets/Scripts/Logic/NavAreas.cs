using System;

[Flags]
public enum NavArea : int
{
	Walkable = 0,
	NonWalkable,
	Jump,
	TestingBuilding
}
