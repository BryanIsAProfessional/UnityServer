using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public const int TICKS_PER_SECOND = 30;
    public const int MS_PER_TICK = 1000 / TICKS_PER_SECOND;
    public const int MAX_PLAYERS = 50;
    public const int PORT = 3002;
    public const float GRAVITY = -9.81f;
    public const float DEFAULT_MOVE_SPEED = 5f;
    public const float DEFAULT_JUMP_SPEED = 5f;
    public const int NUM_INPUTS = 5;
}