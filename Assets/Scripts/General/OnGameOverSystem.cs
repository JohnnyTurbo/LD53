using System;
using Unity.Entities;

namespace TMG.LD53
{
    public partial class OnGameOverSystem : SystemBase
    {
        public Action OnGameOver;
        
        protected override void OnCreate()
        {
            RequireForUpdate<GameOverTag>();
        }

        protected override void OnStartRunning()
        {
            OnGameOver?.Invoke();
        }

        protected override void OnUpdate()
        {
            
        }
    }
    
    public partial class OnWinSystem : SystemBase
    {
        public Action OnWin;
        
        protected override void OnCreate()
        {
            RequireForUpdate<WinTag>();
        }

        protected override void OnStartRunning()
        {
            OnWin?.Invoke();
        }

        protected override void OnUpdate()
        {
            
        }
    }
}