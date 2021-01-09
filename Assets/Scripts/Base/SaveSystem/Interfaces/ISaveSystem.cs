﻿namespace Base.SaveSystem.Interfaces
{
    public interface ISaveSystem
    {
        void Save<T>(T data) where T : class;
        bool Load<T>(out T data) where T : class;
    }
}
