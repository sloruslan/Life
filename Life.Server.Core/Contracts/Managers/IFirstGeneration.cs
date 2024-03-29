﻿using Life.Server.Core.Contracts.Domain;

namespace Life.Server.Core.Contracts.Managers
{
    public interface IFirstGeneration<T> where T : ICellIsLife, new()
    {
        public static abstract T[,] StartGeneration(int perHorizontal, int perVertical, int density);
    }
}
