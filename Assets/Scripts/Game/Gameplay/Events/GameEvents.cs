using System;
using Game.Gameplay.Entity;
using UniRx;

namespace Game.Gameplay.Events
{
    public static class GameEvents
    {
        public static readonly ISubject<Figure> DraggedOffSubject = new Subject<Figure>();

        public static IObservable<Figure> DraggedOff => DraggedOffSubject;
    }
}