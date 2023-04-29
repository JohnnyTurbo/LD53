using Unity.Entities;

namespace TMG.LD53
{
    public enum MoneyType
    {
        Coin,
        Bill,
        Stack
    }
    
    public struct DropMoneyOnDestroy : IComponentData
    {
        public MoneyType MoneyType;
        public float DropRate;
    }
}