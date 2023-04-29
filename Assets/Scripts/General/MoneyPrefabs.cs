using System;
using Unity.Entities;

namespace TMG.LD53
{
    public struct MoneyPrefabs : IComponentData
    {
        public Entity CoinPrefab;
        public Entity BillPrefab;
        public Entity StackPrefab;

        public Entity GetPrefab(MoneyType moneyType)
        {
            return moneyType switch
            {
                MoneyType.Coin => CoinPrefab,
                MoneyType.Bill => BillPrefab,
                MoneyType.Stack => StackPrefab,
                _ => Entity.Null
            };
        }
    }
}